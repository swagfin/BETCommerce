using BetCommerce.Entity.Core;
using BetCommerce.Entity.Core.Requests;
using BetCommerce.WebClient.Extensions;
using BetCommerce.WebClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetCommerce.WebClient.Pages.Cart
{
    public class CheckoutModel : PageModel
    {
        private readonly IHttpService _httpService;
        private readonly ILogger<CheckoutModel> _logger;
        private readonly IShoppingCartService _shoppingCartService;
        public List<OrderItemRequest> CartItems { get; set; }
        public double TotalAmount { get { return (CartItems == null) ? 0 : CartItems.Sum(x => x.TotalCost); } }
        public int CartItemsCount { get { return (CartItems == null) ? 0 : CartItems.Sum(x => x.Quantity); } }
        public string ErrorResponse { get; set; } = null;
        public int LastTransactionNo { get; private set; }

        public CheckoutModel(IHttpService httpService, ILogger<CheckoutModel> logger, IShoppingCartService shoppingCartService)
        {
            this._httpService = httpService;
            this._logger = logger;
            this._shoppingCartService = shoppingCartService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ErrorResponse = null;
                CartItems = _shoppingCartService.GetShoppingList(Request.GetClientIPHash());
                if (CartItems.Sum(x => x.Quantity) <= 0)
                    return Redirect("/cart"); //No items redirect this guy
                if (!User.Identity.IsAuthenticated)
                    return Redirect(string.Format("/account/login/?returnUrl={0}", "/cart/checkout/".UrlEncodedString()));
                //Proceed
                OrderRequest newOrder = new OrderRequest
                {
                    TotalItems = CartItemsCount,
                    OrderItems = CartItems,
                    SubTotal = TotalAmount,
                    DueAmount = TotalAmount,
                    PaidAmount = TotalAmount,
                    DeliveryCost = 0,
                    Discount = 0,
                    Tax = 0,
                    TaxRate = 0,
                    PayMethod = "CREDIT-CARD"
                };
                Response<int> response = await _httpService.PostAsync<Response<int>>("api/orders", newOrder);
                if (!response.IsSucess)
                    throw new Exception($"Error Completing Order, {response.ResponseBody}");
                this.LastTransactionNo = response.Message;
                //Clear Shopping Cart
                _shoppingCartService.EmptyShoppingCart(Request.GetClientIPHash());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ErrorResponse = ex.Message;
            }
            return Page();
        }
    }
}
