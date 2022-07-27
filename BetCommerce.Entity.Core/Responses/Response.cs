using System;

namespace BetCommerce.Entity.Core
{
    public class Response<T>
    {
        public Response()
        {

        }
        public Response(T message, bool isSucces = true, string responseBody = null)
        {
            Message = message;
            IsSucess = isSucces;
            ResponseBody = responseBody;
        }
        public T Message { get; set; }
        public bool IsSucess { get; set; }
        public string ResponseBody { get; set; }
        public string CopyRight
        {
            get { return "BET Software © " + DateTime.Now.ToString("yyyy"); }
        }
    }
}
