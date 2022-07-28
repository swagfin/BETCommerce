using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace BetCommerce.API.Services
{
    public class EmailDispatcherBackgroundJob : IProcessorInitializable
    {
        private readonly ILogger<EmailDispatcherBackgroundJob> _logger;
        private readonly ApiConfigOptions _options;
        private ConcurrentQueue<EmailDispatchQueue> _queue;
        public EmailDispatcherBackgroundJob(ILogger<EmailDispatcherBackgroundJob> logger, IOptions<ApiConfigOptions> options)
        {
            this._logger = logger;
            this._options = options.Value;
        }
        public void Initialize()
        {
            _logger.LogInformation("initializing dispatch thread.....");
            this._queue = new ConcurrentQueue<EmailDispatchQueue>();
            StartEmailDispatchThread();
        }

        public Guid QueueRequest(EmailDispatchQueue emailDispatch)
        {
            emailDispatch.TraceId = Guid.NewGuid();
            _queue.Enqueue(emailDispatch);
            return emailDispatch.TraceId;
        }

        private void StartEmailDispatchThread()
        {
            var t = new Thread(async () =>
            {
                while (true)
                {
                    if (_queue.TryDequeue(out EmailDispatchQueue dispatch))
                        await SendEmailAsync(dispatch);
                    else
                        Thread.Sleep(100);
                    //Sleep
                    Thread.Sleep((this._options.DispatchEmailsAfterMilliseconds <= 0) ? 2000 : this._options.DispatchEmailsAfterMilliseconds);
                }
            });
            t.Start();
        }


        private async Task SendEmailAsync(EmailDispatchQueue emailDispatch)
        {
            if (emailDispatch.Destinations == null || emailDispatch.Destinations.Count == 0)
                return;
            if (string.IsNullOrWhiteSpace(_options.SMTPEmailAddress) || string.IsNullOrWhiteSpace(_options.SMTPEmailCredentials) || string.IsNullOrWhiteSpace(_options.SMTPHost))
                return;
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                using (MailMessage e_mail = new MailMessage())
                using (SmtpClient Smtp_Server = new SmtpClient())
                {
                    //Configs
                    Smtp_Server.UseDefaultCredentials = false;
                    Smtp_Server.Credentials = new System.Net.NetworkCredential(_options.SMTPEmailAddress, _options.SMTPEmailCredentials);
                    Smtp_Server.Port = _options.SMTPPort; //Use 587
                    Smtp_Server.EnableSsl = _options.SMTPEnableSSL;
                    Smtp_Server.Host = _options.SMTPHost;
                    //Other Configs
                    e_mail.From = new MailAddress(_options.SMTPEmailAddress, _options.SMTPDefaultSMTPFromName);
                    //This Configs Should be Placed here
                    e_mail.Subject = emailDispatch.Subject;
                    e_mail.IsBodyHtml = emailDispatch.IsBodyHtml;
                    e_mail.Body = emailDispatch.Body;
                    //Add Default
                    e_mail.To.Add(emailDispatch.Destinations[0]);
                    if (emailDispatch.Destinations.Count > 1)
                    {
                        int addIndex = 0;
                        foreach (string dest in emailDispatch.Destinations)
                        {
                            //Skip the First
                            if (addIndex != 0)
                                e_mail.Bcc.Add(dest);
                            addIndex++;
                        }
                    }
                    //Finally Send
                    await Smtp_Server.SendMailAsync(e_mail);
                    _logger.LogInformation($"successfully dispatched email to addresses");
                }
                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error dispatching email to address", ex.Message);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"dispatch task completed in {stopwatch.ElapsedMilliseconds:N0} Milliseconds");
            }
        }
    }

    public class EmailDispatchQueue
    {
        public Guid TraceId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; } = true;
        public List<string> Destinations { get; set; }
    }
}
