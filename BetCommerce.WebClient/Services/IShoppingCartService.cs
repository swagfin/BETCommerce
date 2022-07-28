using BetCommerce.Entity.Core.Requests;
using System.Collections.Generic;

namespace BetCommerce.WebClient.Services
{
    public interface IShoppingCartService
    {
        void AddProduct(string sessionId, OrderItemRequest orderItemRequest, int Quantity = 1);
        void EmptyShoppingCart(string sessionId);
        List<OrderItemRequest> GetShoppingList(string sessionId, bool initIfNotFound = true);
        int GetShoppingItemsCount(string sessionId);
        void ReduceItem(string sessionId, int itemId);
        string VerifyCanCheckoutMessage(string sessionId);
        void RemoveItem(string sessionId, int productId, out OrderItemRequest existItem);
    }
}
