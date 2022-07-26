using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BetCommerce.Entity.Core
{
    public class ProductCategory
    {
        public int Id { get; set; }
        [StringLength(255)]
        public string CategoryName { get; set; }

        [Column(TypeName = "text")]
        public string MoreInfo { get; set; }
    }
}
