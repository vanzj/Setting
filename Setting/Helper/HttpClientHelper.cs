

using System.Net.Http;

namespace Setting.Helper
{
    public class HttpClientHelper
    {



       private HttpClient client;

      static readonly object _object = new object();
        private HttpClientHelper()
        {
            client = new HttpClient();
            client.BaseAddress = new System.Uri("https://smart.9jodia.net");
        }

        public void setToken (string token)
        {
            client.DefaultRequestHeaders.Add("token", token);
        }
        public void RemoveToken()
        {
            client.DefaultRequestHeaders.Remove("token");
        }
        private static HttpClientHelper instance = null;
        public static HttpClientHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_object)
                    {
                        if (instance == null)
                        {

                            instance = new HttpClientHelper();
                        }
                    }
                }
                return instance;
            }
        }

        public   HttpClient GetHttpClient()
        {
            return client;
        }
    }
}
