using System;
using System.ComponentModel.DataAnnotations;

namespace BetCommerce.Entity.Core.Responses
{
    public class UserIdentityResponse
    {
        public string Id { get; set; }
        [StringLength(255)]
        public string FullName { get; set; } = string.Empty;
        [StringLength(90)]
        public string MobilePhone { get; set; } = string.Empty;
        public string EmailAddress { get; set; }
        public string ProfileImage { get; set; } = "no_image.png";
        public DateTime? LastLogin { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;
        public bool IsMobilePhoneConfirmed { get; set; } = false;
        public string JwtTokenKey { get; set; }
    }
}
