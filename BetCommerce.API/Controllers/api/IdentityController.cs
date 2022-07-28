using AutoMapper;
using BetCommerce.Entity.Core;
using BetCommerce.Entity.Core.Requests;
using BetCommerce.Entity.Core.Responses;
using BetCommerce.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUserAccountService _userAccountService;
        private readonly IMapper _mapper;

        public IConfiguration Configuration { get; }

        public IdentityController(ILogger<IdentityController> logger, IUserAccountService userAccountService, IConfiguration configuration, IMapper mapper)
        {
            this._logger = logger;
            this._userAccountService = userAccountService;
            Configuration = configuration;
            this._mapper = mapper;
        }


        [AllowAnonymous]
        [HttpPost("SignIn")]
        public async Task<Response<UserIdentityResponse>> PostSignIn([FromBody] SignInRequest signInRequest)
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
        public async Task<Response<UserIdentityResponse>> PostSignUp([FromBody] SignUpRequest userIdentityRequest)
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
                await _userAccountService.AddAsync(model);
                UserIdentityResponse responseModel = _mapper.Map<UserIdentityResponse>(model);
                responseModel.JwtTokenKey = GenerateJwtToken(model);
                return new Response<UserIdentityResponse>(responseModel, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<UserIdentityResponse>(null, false, ex.Message);
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
    }
}
