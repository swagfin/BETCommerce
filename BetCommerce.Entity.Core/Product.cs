using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetCommerce.Entity.Core
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(150)]
        public string ProductBarcode { get; set; } = new Random().Next(10011, 99999).ToString();
        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }
        [StringLength(225)]
        public string Category { get; set; }
        public string Description { get; set; }
        public double CurrentQuantity { get; set; } = 0;
        [Range(0, 999999999)]
        public double SellingPrice { get; set; } = 0;
        [Column(TypeName = "text")]
        public string ImageFile { get; set; } = "no_image.png";
        [DataType(DataType.DateTime)]
        public DateTime RegisteredDateUtc { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
