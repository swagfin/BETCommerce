using BetCommerce.Entity.Core.Requests;
using BetCommerce.WebClient.Extensions;
using BetCommerce.WebClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;

namespace BetCommerce.WebClient.Pages.Cart
{
    public class RemoveItemModel : PageModel
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<RemoveItemModel> _logger;
        private readonly IShoppingCartService _shoppingCartService;

        public RemoveItemModel(IHttpService httpService, ILogger<RemoveItemModel> logger, IShoppingCartService shoppingCartService)
        {
            this._httpService = httpService;
            this._logger = logger;
            this._shoppingCartService = shoppingCartService;
        }
        public ActionResult OnGet(int id)
        {
            try
            {
                _shoppingCartService.RemoveItem(Request.GetClientIPHash(), id, out OrderItemRequest _removedItem);
                if (_removedItem != null)
                    return new OkObjectResult($"Successfully added {_removedItem.ProductName} to Shopping Cart");
                return new OkObjectResult($"Successfully remove item to Shopping Cart");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

    }
}

