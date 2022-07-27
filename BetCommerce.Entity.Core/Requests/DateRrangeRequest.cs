using System;
using System.ComponentModel.DataAnnotations;

namespace BetCommerce.Entity.Core.Requests
{
    public class DateRangeRequest
    {
        [Required]
        public DateTime DateFrom { get; set; }
        [Required]
        public DateTime DateTo { get; set; }
        public DateRangeRequest()
        {
            DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            DateTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
        }
    }
}
