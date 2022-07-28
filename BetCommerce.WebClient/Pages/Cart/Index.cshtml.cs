using BetCommerce.Entity.Core.Requests;
using BetCommerce.WebClient.Extensions;
using BetCommerce.WebClient.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BetCommerce.WebClient.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<AddItemModel> _logger;
        private readonly IShoppingCartService _shoppingCartService;
        public List<OrderItemRequest> CartItems { get; set; }
        public double TotalAmount { get { return (CartItems == null) ? 0 : CartItems.Sum(x => x.TotalCost); } }
        public string ErrorResponse { get; set; } = null;
        public IndexModel(ILogger<AddItemModel> logger, IShoppingCartService shoppingCartService)
        {
            this._logger = logger;
            this._shoppingCartService = shoppingCartService;
        }
        public IActionResult OnGetAsync()
        {
            try
            {
                ErrorResponse = null;
                CartItems = _shoppingCartService.GetShoppingList(Request.GetClientIPHash());
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
