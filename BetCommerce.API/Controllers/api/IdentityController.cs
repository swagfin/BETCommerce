using AutoMapper;
using BetCommerce.API.Extensions;
using BetCommerce.API.Services;
using BetCommerce.Entity.Core;
using BetCommerce.Entity.Core.Requests;
using BetCommerce.Entity.Core.Responses;
using BetCommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BetCommerce.API.Controllers.api
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly ILogger<IdentityController> _logger;
        private readonly EmailDispatcherBackgroundJob _emailDispatcherBackgroundJob;
        private readonly IUserAccountService _userAccountService;
        private readonly IMapper _mapper;
        public string AppBaseUrl { get { return $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}"; } }
        public HttpContext Current { get; }
        public IConfiguration Configuration { get; }

        public IdentityController(ILogger<IdentityController> logger, EmailDispatcherBackgroundJob emailDispatcherBackgroundJob, IUserAccountService userAccountService, IConfiguration configuration, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logger;
            this._emailDispatcherBackgroundJob = emailDispatcherBackgroundJob;
            this._userAccountService = userAccountService;
            Configuration = configuration;
            this._mapper = mapper;
            this.Current = httpContextAccessor.HttpContext;
        }


        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<Response<UserIdentityResponse>> PostSignInAsync([FromBody] SignInRequest signInRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                //Whitelisted IP Address
                UserAccount user = await _userAccountService.SignInAsync(signInRequest.EmailAddress, signInRequest.Password);
                UserIdentityResponse model = _mapper.Map<UserIdentityResponse>(user);
                model.JwtTokenKey = GenerateJwtToken(user);
                return new Response<UserIdentityResponse>(model, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<UserIdentityResponse>(null, false, ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task<Response<UserIdentityResponse>> PostSignUpAsync([FromBody] SignUpRequest userIdentityRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                //Whitelisted IP Address
                UserAccount model = _mapper.Map<UserAccount>(userIdentityRequest);
                model.PasswordHash = userIdentityRequest.Password;
                model.IsMobilePhoneConfirmed = false;
                model.IsEmailConfirmed = false;
                model.IsActive = true;
                model.FullName = string.Empty;
                model.DateRegisteredUtc = DateTime.UtcNow;
                model.OauthKey = new Random().Next(100001, 999999).ToString();
                await _userAccountService.AddAsync(model);
                //Send Email Confirmation Code
                UserIdentityResponse responseModel = _mapper.Map<UserIdentityResponse>(model);
                responseModel.JwtTokenKey = GenerateJwtToken(model);

                //Verify Email Confirmed
                if (!model.IsEmailConfirmed)
                    ResendEmailConfirmationEmail(model);
                //Proceed
                return new Response<UserIdentityResponse>(responseModel, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<UserIdentityResponse>(null, false, ex.Message);
            }
        }


        [Authorize]
        [HttpPost("VerifyCode")]
        public async Task<Response<UserIdentityResponse>> PostVerifyCodeAsync([FromBody] CodeVerificationRequest codeVerificationRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                string emailAddress = User.Identity.Name;
                UserAccount user = await _userAccountService.GetByEmailAsync(emailAddress);
                if (!user.OauthKey.Equals(codeVerificationRequest.Code))
                    throw new Exception("Invalid verification code provided");
                //Proceed
                user.IsEmailConfirmed = true;
                await _userAccountService.UpdateAsync(user);
                UserIdentityResponse responseModel = _mapper.Map<UserIdentityResponse>(user);
                responseModel.JwtTokenKey = GenerateJwtToken(user);
                return new Response<UserIdentityResponse>(responseModel, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<UserIdentityResponse>(null, false, ex.Message);
            }
        }


        [Authorize]
        [HttpGet("ResendEmailConfirmation")]
        public async Task<Response<Guid>> GetResendEmailConfirmationAsync()
        {
            try
            {
                string emailAddress = User.Identity.Name;
                UserAccount user = await _userAccountService.GetByEmailAsync(emailAddress);
                //Verify Email Confirmed
                if (user.IsEmailConfirmed)
                    throw new Exception("You account is already confirmed confirmed");
                Guid traceKey = ResendEmailConfirmationEmail(user);
                return new Response<Guid>(traceKey, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<Guid>(Guid.Empty, false, ex.Message);
            }
        }


        private string GenerateJwtToken(UserAccount user)
        {
            if (user == null)
                return null;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim(ClaimTypes.Name, user.EmailAddress ?? string.Empty),
                new Claim(ClaimTypes.GivenName, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.MobilePhone, user.MobilePhone),
                new Claim(nameof(user.IsEmailConfirmed), user.IsEmailConfirmed.ToString()),
                new Claim(nameof(user.ProfileImage), user.ProfileImage.ToString()),
            };
            //Skipping Adding User Group Roles
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JWTAuth:EncryptKey")));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            int expireInDays = Configuration.GetValue<int>("JWTAuth:ExpirationInDays");
            DateTime expires = DateTime.UtcNow.AddDays(expireInDays);
            var token = new JwtSecurityToken(
                issuer: Configuration.GetValue<string>("JWTAuth:Issuer"),
                audience: Configuration.GetValue<string>("JWTAuth:Audience"),
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private Guid ResendEmailConfirmationEmail(UserAccount model)
        {
            if (!model.IsEmailConfirmed)
            {
                string templateBody = model.GetEmailConfirmationHTMLTemplate(AppBaseUrl);
                if (!string.IsNullOrWhiteSpace(templateBody))
                    return _emailDispatcherBackgroundJob.QueueRequest(new EmailDispatchQueue
                    {
                        Subject = "BET E-COMMERCE | CONFIRM YOUR EMAIL ADDRESS",
                        Body = templateBody,
                        Destinations = new List<string> { model.EmailAddress },
                        IsBodyHtml = true,
                    });
            }
            return Guid.Empty;
        }
    }
}
