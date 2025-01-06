using AspireApplication.Models.Models;
using Newtonsoft.Json;
using System.Text;

namespace AspireApplications.Web.Service
{
    public interface IProductInterface
    {
        Task<List<Product>> GetProductRecords(int offset = 1, int limit = 20, string? searchQuery = null);
        Task<MyResponses> CreateProductRecords(Product product);
        Task<Product> GetProductPerId(int productId);
        Task<MyResponses> UpdateProductRecords(Product product, int productId);
        Task<MyResponses> DeleteProductRecord(int productId);
    }

    public class ProductService : IProductInterface
    {
        private readonly IHttpClient httpService;
        public ProductService(IHttpClient httpService)
        {
            this.httpService = httpService;
        }

        public async Task<MyResponses> CreateProductRecords(Product product)
        {
            MyResponses responses = new MyResponses();
            var client = httpService.CreateHttpClient();
            var jsonStrings = JsonConvert.SerializeObject(product);
            var httpContent = new StringContent(jsonStrings, Encoding.UTF8, "application/json");
            var responseTask = client.PostAsync("Product/CreateProductDetails", httpContent);

            responseTask.Wait();
            var result = responseTask.Result;
            int statusCode = (int)result.StatusCode;

            if (result.IsSuccessStatusCode)
            {
                var body = await result.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeAnonymousType(body, responses);
                return responseObject;
            }
            else
            {
                var body = await result.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeAnonymousType(body, responses);
                return responseData;
            }
        }

        public async Task<MyResponses> DeleteProductRecord(int productId)
        {
            ProductData employeeData = new ProductData();
            var client = httpService.CreateHttpClient();
            var responseTask = client.DeleteAsync("Product/DeleteProductRecords?id=" + productId);

            responseTask.Wait();
            var result = responseTask.Result;
            int statusCode = (int)result.StatusCode;
            if (result.IsSuccessStatusCode)
            {
                var body = await result.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<MyResponses>(body);
                return await Task.FromResult(responseObject);
            }
            else
            {
                var body = await result.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<MyResponses>(body);
                return await Task.FromResult(responseData);
            }
        }

        public Task<Product> GetProductPerId(int productId)
        {
            Product res = new Product();
            var client = httpService.CreateHttpClient();
            var responseTask = client.GetAsync("Product/GetProductPerId?id=" + productId);
            responseTask.Wait();
            var result = responseTask.Result;
            int statusCode = (int)result.StatusCode;
            //CHECK STATUS CODE
            if (result.IsSuccessStatusCode)
            {
                var body = result.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeAnonymousType(body, res);
                var response = Task.FromResult(responseData);
                return response;
            }
            return Task.FromResult(res);
        }

        public Task<List<Product>> GetProductRecords(int offset = 1, int limit = 20, string? searchQuery = null)
        {
            List<Product> res = new List<Product>();
            var client = httpService.CreateHttpClient();
            var responseTask = client.GetAsync($"Product/GetProductList?offset={offset}&limit={limit}&searchQuery={searchQuery}");
            responseTask.Wait();
            var result = responseTask.Result;
            int statusCode = (int)result.StatusCode;
            if (result.IsSuccessStatusCode)
            {
                var body = result.Content.ReadAsStringAsync().Result;
                var responseData = JsonConvert.DeserializeAnonymousType(body, res);
                var response = Task.FromResult(responseData);
                return response;
            }
            return Task.FromResult(res);
        }

        public async Task<MyResponses> UpdateProductRecords(Product product, int productId)
        {
            var client = httpService.CreateHttpClient();
            var jsonStrings = JsonConvert.SerializeObject(product);
            var httpContent = new StringContent(jsonStrings, Encoding.UTF8, "application/json");
            var responseTask = client.PutAsync("Product/EditProductRecords?id=" + productId, httpContent);

            responseTask.Wait();
            var result = responseTask.Result;
            int statusCode = (int)result.StatusCode;
            if (result.IsSuccessStatusCode)
            {
                var body = await result.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<MyResponses>(body);
                return responseObject;
            }
            else if ((int)result.StatusCode == 400)
            {
                var body = await result.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<MyResponses>(body);
                return responseObject;
            }
            else
            {
                //check if response data is empty
                var body = await result.Content.ReadAsStringAsync();
                var responseData = JsonConvert.DeserializeObject<MyResponses>(body);
                return responseData;
            }
        }
    }
}
