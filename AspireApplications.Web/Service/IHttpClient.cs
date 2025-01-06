using AspireApplications.Web.Helper;

namespace AspireApplications.Web.Service
{
    public interface IHttpClient
    {
        HttpClient CreateHttpClient();
        string GetBaseAddress();
    }

    public class HttpClientFactory : IHttpClient
    {
        private string baseAddress = ConfigHelper.AppSetting("BASEURL");
        public HttpClient CreateHttpClient()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            var client = new System.Net.Http.HttpClient(clientHandler);
            SetupClientDefaults(client);
            return client;
        }

        private void SetupClientDefaults(HttpClient client)
        {
            Console.WriteLine("base address ======================================================================= " + baseAddress);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.BaseAddress = new Uri(baseAddress);
        }

        public string GetBaseAddress()
        {
            return baseAddress;
        }
    }
}
