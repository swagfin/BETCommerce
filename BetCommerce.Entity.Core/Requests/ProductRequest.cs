using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetCommerce.Entity.Core.Requests
{
    public class ProductRequest
    {
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
    }
}
