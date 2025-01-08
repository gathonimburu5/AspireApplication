using AspireApplication.Models.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AspireApplications.Web.Service
{
    public interface ICountryInterface
    {
        Task<List<CustomizedCountry>> GetCountryRecords(int offset = 1, int limit = 20, string? searchQuery = null);
        Task<List<CustomizedCurrency>> GetCurrencyRecordsPerCountryCode(string code);
        Task<List<CustomizedRegion>> GetRegionRecordsPerCountryCode(string code, int offset = 1, int limit = 10, string? searchQuery = null);
    }

    public class CountryService(IHttpClient httpService) : ICountryInterface
    {
        public Task<List<CustomizedCountry>> GetCountryRecords(int offset = 1, int limit = 20, string? searchQuery = null)
        {
            List<CustomizedCountry> res = new List<CustomizedCountry>();
            var client = httpService.CreateHttpClient();
            var responseTask = client.GetAsync($"Country/GetCustomizedCountryRecord?offset={offset}&limit={limit}&searchQuery={searchQuery}");
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

        public Task<List<CustomizedCurrency>> GetCurrencyRecordsPerCountryCode(string code)
        {
            List<CustomizedCurrency> res = new List<CustomizedCurrency>();
            var client = httpService.CreateHttpClient();
            var responseTask = client.GetAsync($"Country/GetCurrencyPerCountryCode?code={code}");
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

        public Task<List<CustomizedRegion>> GetRegionRecordsPerCountryCode(string code, int offset = 1, int limit = 10, string? searchQuery = null)
        {
            List<CustomizedRegion> res = new List<CustomizedRegion>();
            var client = httpService.CreateHttpClient();
            var responseTask = client.GetAsync($"Country/GetRegionsPerCountryCode?code={code}&offset={offset}&limit={limit}&searchQuery={searchQuery}");
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
    }
}
