using System.ComponentModel.DataAnnotations;

namespace BetCommerce.Entity.Core.Requests
{
    public class UserIdentityRequest
    {
        [Required, EmailAddress]
        [StringLength(255)]
        public string EmailAddress { get; set; }
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
