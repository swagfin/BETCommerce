using BetCommerce.Entity.Core;
using BetCommerce.WebClient.Extensions;
using BetCommerce.WebClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BetCommerce.WebClient.Pages.Cart
{
    public class GetCartCountModel : PageModel
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<GetCartCountModel> _logger;
        private readonly IShoppingCartService _shoppingCartService;

        public GetCartCountModel(IHttpService httpService, ILogger<GetCartCountModel> logger, IShoppingCartService shoppingCartService)
        {
            this._httpService = httpService;
            this._logger = logger;
            this._shoppingCartService = shoppingCartService;
        }
        public async Task<ActionResult> OnGetAsync(int id)
        {
            try
            {
                var response = await _httpService.GetAsync<Response<Product>>($"api/products/{id}");
                int itemsCount = _shoppingCartService.GetShoppingItemsCount(Request.GetClientIPHash());
                return new OkObjectResult(itemsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
