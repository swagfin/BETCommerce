using BetCommerce.Entity.Core;
using BetCommerce.Entity.Core.Requests;
using BetCommerce.WebClient.Extensions;
using BetCommerce.WebClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BetCommerce.WebClient.Pages.Cart
{
    public class AddItemModel : PageModel
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<AddItemModel> _logger;
        private readonly IShoppingCartService _shoppingCartService;

        public AddItemModel(IHttpService httpService, ILogger<AddItemModel> logger, IShoppingCartService shoppingCartService)
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
                if (!response.IsSucess)
                    throw new Exception($"Unable to Retrive product information, Error: {response.ResponseBody}");
                //Lets Retrieve the IP Address. Remember if its coming from (Reverse Proxy) wil defintely fetch the device IP from Header
                Product product = response.Message;
                _shoppingCartService.AddProduct(Request.GetClientIPHash(), new OrderItemRequest
                {
                    ProductBarcode = product.ProductBarcode,
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Rate = product.SellingPrice,
                    TotalCost = product.SellingPrice,
                    Quantity = 1,
                });
                return new OkObjectResult($"Successfully added {product.ProductName} to Shopping Cart");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

    }
}
