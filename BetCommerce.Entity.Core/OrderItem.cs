using System;
using System.ComponentModel.DataAnnotations;

namespace BetCommerce.Entity.Core
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductBarcode { get; set; }
        [Range(0, 999999999)]
        public int Quantity { get; set; } = 1;
        [Range(0, 999999999)]
        public double Rate { get; set; } = 0;
        [Range(0, 999999999)]
        public double TotalCost
        {
            get { return this.Rate * this.Quantity; }
            set { }
        }
        public virtual Order Order { get; set; }
    }
}