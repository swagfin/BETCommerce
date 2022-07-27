using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetCommerce.Entity.Core.Requests
{
    public class OrderRequest
    {
        public int TotalItems { get; set; } = 0;
        public double SubTotal { get; set; } = 0;
        public double Tax { get; set; } = 0;
        public double TaxRate { get; set; } = 0;
        public double DeliveryCost { get; set; } = 0;
        public double Discount { get; set; } = 0;
        public double DueAmount { get; set; } = 0;
        public double PaidAmount { get; set; } = 0;
        [StringLength(240)]
        public string PayMethod { get; set; }
        [Column(TypeName = "text")]
        public string OrderNote { get; set; }
        public List<OrderItemRequest> OrderItems { get; set; }
    }
}
