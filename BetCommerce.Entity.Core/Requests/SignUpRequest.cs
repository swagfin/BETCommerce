using System.ComponentModel.DataAnnotations;

namespace BetCommerce.Entity.Core.Requests
{
    public class SignUpRequest
    {
        [Required, EmailAddress]
        [StringLength(255)]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(255), MinLength(6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password must be at list 6 characters, 1 Uppercase Letter")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password), MinLength(6)]
        [Compare("Password")]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password must be at list 6 characters, 1 Uppercase Letter")]
        public string ConfirmPassword { get; set; }
    }
}
