using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetCommerce.Entity.Core
{
    public class Order
    {
        public int Id { get; set; }
        [StringLength(250)]
        public string OrderRef { get; set; }
        public int? UserAccountId { get; set; }
        public string UserAccountName { get; set; }
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
        public DateTime OrderDateUtc { get; set; } = DateTime.UtcNow;
        public DateTime LastStatusChangeDateUtc { get; set; } = DateTime.UtcNow;
        [Column(TypeName = "text")]
        public string OrderNote { get; set; }
        [StringLength(50)]
        public string OrderStatus { get; set; } = "COMPLETED";
        public UserAccount UserAccount { get; set; }
        public List<OrderItem> OrderItems { get; set; }

    }
}
