using BetCommerce.Entity.Core;
using BetCommerce.Entity.Core.Requests;
using BetCommerce.Entity.Core.Responses;
using BetCommerce.WebClient.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BetCommerce.WebClient.Pages.Account
{
    public class VerifyEmailModel : PageModel
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<VerifyEmailModel> _logger;

        [BindProperty]
        public CodeVerificationRequest VerificationRequest { get; set; }
        public VerifyEmailModel(IHttpService httpService, ILogger<VerifyEmailModel> logger)
        {
            this._httpService = httpService;
            this._logger = logger;
        }
        public string ErrorResponse { get; set; } = null;
        public async Task<IActionResult> OnGetAsync()
        {
            if (Request.Query.ContainsKey("resend-code"))
            {
                await ResendVerificationCodeAsync();
                return Redirect("/account/verifyEmail");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ErrorResponse = null;
                if (string.IsNullOrWhiteSpace(VerificationRequest.Code))
                    throw new Exception("Email/Username is required to Login");
                Response<UserIdentityResponse> response = await _httpService.PostAsync<Response<UserIdentityResponse>>("api/identity/verifycode", VerificationRequest);
                if (!response.IsSucess || string.IsNullOrWhiteSpace(response.Message.JwtTokenKey))
                    throw new Exception($"Code verification failed, {response.ResponseBody}");
                await SignInUserFromResponse(response);
                return Redirect("/");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ErrorResponse = ex.Message;
            }
            //Finnally
            return Page();

        }



        private async Task ResendVerificationCodeAsync()
        {
            try
            {
                ErrorResponse = null;
                Response<Guid> response = await _httpService.GetAsync<Response<Guid>>("api/identity/ResendEmailConfirmation");
                if (!response.IsSucess)
                    throw new Exception($"Resending verification code failed, {response.ResponseBody}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ErrorResponse = ex.Message;
            }
        }

        private async Task SignInUserFromResponse(Response<UserIdentityResponse> response)
        {
            JwtSecurityToken jwttoken = new JwtSecurityTokenHandler().ReadJwtToken(response.Message.JwtTokenKey);
            List<Claim> claims = jwttoken.Claims.ToList();
            //Add new Role Basedd Claim
            claims.Add(new Claim("betcommerce-token", response.Message.JwtTokenKey));
            //Prepare Selft Claims Identity
            ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
            identity.AddClaims(claims);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                ExpiresUtc = jwttoken.ValidTo,
                IsPersistent = true,
            };
            //Sign In User
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(principal), authProperties);
        }


    }
}
