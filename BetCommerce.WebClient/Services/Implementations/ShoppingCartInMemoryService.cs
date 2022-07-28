using BetCommerce.Entity.Core.Requests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BetCommerce.WebClient.Services.Implementations
{
    public class ShoppingCartInMemoryService : IShoppingCartService
    {
        private ConcurrentDictionary<string, List<OrderItemRequest>> ShoppingCart = new ConcurrentDictionary<string, List<OrderItemRequest>>();

        public List<OrderItemRequest> GetShoppingList(string sessionId, bool initIfNotFound = true)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
                throw new Exception("Session Id can't be Empty");
            if (ShoppingCart.TryGetValue(sessionId, out List<OrderItemRequest> _value))
                return _value;
            if (initIfNotFound)
            {
                _value = new List<OrderItemRequest>();
                ShoppingCart.TryAdd(sessionId, _value);
            }
            return _value;
        }
        public int GetShoppingItemsCount(string sessionId)
        {
            return GetShoppingList(sessionId, false).Count;
        }

        public void AddProduct(string sessionId, OrderItemRequest orderItemRequest, int Quantity = 1)
        {
            List<OrderItemRequest> cart = GetShoppingList(sessionId, true);
            var existItem = cart.FirstOrDefault(x => x.ProductId.Equals(orderItemRequest.ProductId));
            if (existItem != null)
            {
                existItem.Quantity += 1;
                existItem.Rate = orderItemRequest.Rate;
            }
            else
                cart.Add(orderItemRequest);
        }

        public void EmptyShoppingCart(string sessionId)
        {
            ShoppingCart.TryRemove(sessionId, out _);
        }


        public void ReduceItem(string sessionId, int itemId)
        {
            List<OrderItemRequest> cart = GetShoppingList(sessionId, true);
            var existItem = cart.FirstOrDefault(x => x.ProductId.Equals(itemId));
            if (existItem != null)
            {
                existItem.Quantity -= 1;
                if (existItem.Quantity <= 0)
                    cart.Remove(existItem);
            }
        }

        public string VerifyCanCheckoutMessage(string sessionId)
        {
            var cart = GetShoppingList(sessionId);
            if (cart.Count < 1)
                return ("No Items added In Shopping Cart");
            foreach (var item in cart)
            {
                if (item.Rate < 0)
                    return ($"Rate of {item.ProductName} can not be less than 0");
            }
            return null;
        }


    }
}
