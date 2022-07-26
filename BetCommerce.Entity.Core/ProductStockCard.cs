using System;
using System.ComponentModel.DataAnnotations;

namespace BetCommerce.Entity.Core
{
    public class ProductStockCard
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string TransactionType { get; set; }
        public int? ProductId { get; set; }
        [StringLength(250)]
        public string Narration { get; set; }
        [StringLength(90)]
        public string RefNo { get; set; } = string.Empty;
        public double OpeningQuantity { get; set; } = 0;
        public double StockIn { get; set; } = 0;
        public double StockOut { get; set; } = 0;
        public double ClosingQuantity { get; set; } = 0;
        [StringLength(250)]
        public DateTime TransactionDateUtc { get; set; } = DateTime.UtcNow;
    }
}
