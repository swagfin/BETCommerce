using AutoMapper;
using BetCommerce.Entity.Core;
using BetCommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetCommerce.API.Controllers.api
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountsController : ControllerBase
    {
        private readonly ILogger<UserAccountsController> _logger;
        private readonly IUserAccountService _userAccountService;
        private readonly IMapper _mapper;

        public UserAccountsController(ILogger<UserAccountsController> logger, IUserAccountService userAccountService, IMapper mapper)
        {
            this._logger = logger;
            this._userAccountService = userAccountService;
            this._mapper = mapper;
        }


        [HttpGet]
        public async Task<Response<IEnumerable<UserAccount>>> GetAsync(int page = -1, int size = -1)
        {
            try
            {
                IEnumerable<UserAccount> dataFeedback = dataFeedback = await _userAccountService.GetAllAsync(page, size); //Page Size Implementation
                return new Response<IEnumerable<UserAccount>>(dataFeedback);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<IEnumerable<UserAccount>>(null, false, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<Response<UserAccount>> GetAsync(string id)
        {
            try
            {
                UserAccount record = await _userAccountService.GetAsync(id);
                return new Response<UserAccount>(record);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<UserAccount>(null, false, ex.Message);
            }
        }

        [HttpPost]
        public async Task<Response<string>> PostAsync([FromBody] UserAccount userAccount)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                await _userAccountService.AddAsync(userAccount);
                return new Response<string>(userAccount.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<string>(Guid.Empty.ToString(), false, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<Response<bool>> PutAsync(string id, [FromBody] UserAccount userAccount)
        {
            try
            {
                if (!ModelState.IsValid)
                    throw new Exception(string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                await _userAccountService.UpdateAsync(userAccount);
                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<bool>(false, false, ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<Response<bool>> DeleteAsync(string id)
        {
            try
            {
                await _userAccountService.RemoveAsync(id);
                return new Response<bool>(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response<bool>(false, false, ex.Message);
            }
        }
    }
}
