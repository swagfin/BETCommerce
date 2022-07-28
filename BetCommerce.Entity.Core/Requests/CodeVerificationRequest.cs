using System.ComponentModel.DataAnnotations;

namespace BetCommerce.Entity.Core.Requests
{
    public class CodeVerificationRequest
    {
        [Required]
        public string Code { get; set; }
    }
}
