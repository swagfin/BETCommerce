using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace BetCommerce.WebClient.Extensions
{
    public static class HttpRequestExtensions
    {
        public static string GetClientIP(this HttpRequest httpRequest)
        {

            string deviceIPAddress = httpRequest.HttpContext.Connection.RemoteIpAddress.ToString();
            if (httpRequest.Headers.ContainsKey("X-Forwarded-For"))
                deviceIPAddress = httpRequest.Headers["X-Forwarded-For"][0];
            return deviceIPAddress;
        }
        public static string GetClientIPHash(this HttpRequest httpRequest)
        {
            string input = GetClientIP(httpRequest);
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    sb.Append(hashBytes[i].ToString("X2"));
                return sb.ToString();
            }
        }
    }
}
