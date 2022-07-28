using System.Text;
using System.Web;

namespace BetCommerce.WebClient.Extensions
{
    public static class StringExtensions
    {
        public static string UrlEncoded(this string str)
        {
            return UrlEncodedString(str);
        }

        public static string UrlEncodedString(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return HttpUtility.UrlEncode(value, Encoding.UTF8).Trim();
            return value;
        }
    }
}
