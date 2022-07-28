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

namespace BetCommerce.WebClient.Pages.Account.Login
{
    public class LoginModel : PageModel
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty]
        public SignInRequest signInRequest { get; set; }
        public LoginModel(IHttpService httpService, ILogger<LoginModel> logger)
        {
            this._httpService = httpService;
            this._logger = logger;
        }
        public string ErrorResponse { get; set; } = null;
        public IActionResult OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            try
            {
                returnUrl = returnUrl ?? Url.Content("~/");
                if (string.IsNullOrWhiteSpace(signInRequest.EmailAddress))
                    ErrorResponse = "Email/Username is required to Login";
                else if (string.IsNullOrWhiteSpace(signInRequest.Password))
                    ErrorResponse = "Password is required to Login";
                else
                {

                    Response<UserIdentityResponse> response = await _httpService.PostAsync<Response<UserIdentityResponse>>("api/identity/signin", signInRequest);
                    if (!response.IsSucess || string.IsNullOrWhiteSpace(response.Message.JwtTokenKey))
                        throw new Exception($"Authentication Failed, {response.ResponseBody}");
                    //Proceeed
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
                    return LocalRedirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ErrorResponse = ex.Message;
            }
            //Finnally
            return Page();

        }
    }
}
