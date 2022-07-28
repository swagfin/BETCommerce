using BetCommerce.Entity.Core;
using System;
using System.IO;

namespace BetCommerce.API.Extensions
{
    public static class HtmlTemplatingExtensions
    {
        public static string GetEmailConfirmationHTMLTemplate(this UserAccount userDetails, string apiEndpointUrl, string template = "confirm-email.html")
        {
            try
            {
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EmailTemplates", template);
                if (!File.Exists(templatePath))
                    throw new Exception("Template doesn't seem to exists");
                string templateContents = File.ReadAllText(templatePath);
                return templateContents.Replace("{{EMAILADDRESS}}", userDetails.EmailAddress)
                                       .Replace("{{FULLNAME}}", userDetails.FullName)
                                       .Replace("{{MOBILEPHONE}}", userDetails.MobilePhone)
                                       .Replace("{{PROFILEIMAGE}}", userDetails.ProfileImage)
                                       .Replace("{{ID}}", userDetails.Id)
                                       .Replace("{{CODE}}", userDetails.OauthKey)
                                       .Replace("{{APIENDPOINT}}", apiEndpointUrl);
            }
            catch { return string.Empty; }
        }
    }
}
