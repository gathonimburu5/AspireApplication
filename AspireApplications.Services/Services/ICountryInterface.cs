using AspireApplication.Models.Models;
using CountryData.Standard;
using Newtonsoft.Json;

namespace AspireApplications.Services.Services
{
    public interface ICountryInterface
    {
        Task<string> GetCountryAllData(int offset = 1, int limit = 20, string? searchQuery = null);
        Task<string> GetCountryByCode(string code);
        Task<string> GetCountryEmojiFlag(string code);
        Task<List<Regions>> GetRegionByCountryCode(string countryCode);
        Task<IEnumerable<Currency>> GetCurrencyByCountryCode(string countryCode);
        Task<List<CustomizedCountry>> getCountryRecordsAsync(int offset = 1, int limit = 10, string? searchQuery = null);
        Task<List<CustomizedCurrency>> getCustomizedCurrencyAsync(string code);
        Task<List<CustomizedRegion>> getCustomizedRegionAsync(string code, int offset = 1, int limit = 10, string? searchQuery = null);
    }

    public class CountryService : ICountryInterface
    {
        private readonly CountryHelper countryService;
        public CountryService(CountryHelper countryService)
        {
            this.countryService = countryService;
        }
        public Task<string> GetCountryAllData(int offset = 1, int limit = 20, string? searchQuery = null)
        {
            var countries = countryService.GetCountryData();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                countries = countries.Where(x => x.CountryShortCode.ToLower().Contains(searchQuery)).ToList();
            }
            var pagination = countries.Skip((offset - 1) * limit).Take(limit).ToList();
            if (!pagination.Any())
            {
                return Task.FromResult("no record found");
            }
            return Task.FromResult(JsonConvert.SerializeObject(pagination, Formatting.Indented));
        }

        public Task<string> GetCountryByCode(string code)
        {
            var country = countryService.GetCountryByCode(code);
            if(country != null)
            {
                return Task.FromResult(JsonConvert.SerializeObject(country, Formatting.Indented));
            }
            return Task.FromResult("no record found");
        }

        public Task<string> GetCountryEmojiFlag(string code)
        {
            var flag = countryService.GetCountryEmojiFlag(code);
            return Task.FromResult(JsonConvert.SerializeObject(flag, Formatting.Indented));
        }

        public Task<List<CustomizedCountry>> getCountryRecordsAsync(int offset = 1, int limit = 10, string? searchQuery = null)
        {
            var countries = countryService.GetCountryData();
            var countryList = countries.Select(x => new CustomizedCountry
            {
                CountryName = x.CountryName,
                PhoneCode = x.PhoneCode,
                ShortCode = x.CountryShortCode,
                CountryFlag = x.CountryFlag
            }).ToList();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                countryList = countryList.Where(x => x.ShortCode.ToLower().Contains(searchQuery)).ToList();
            }
            var pagination = countryList.Skip((offset - 1) * limit).Take(limit).ToList();
            return Task.FromResult(pagination);
        }

        public Task<IEnumerable<Currency>> GetCurrencyByCountryCode(string countryCode)
        {
            var currency = countryService.GetCurrencyCodesByCountryCode(countryCode);
            return Task.FromResult(currency);
        }

        public Task<List<CustomizedCurrency>> getCustomizedCurrencyAsync(string code)
        {
            var currency = countryService.GetCurrencyCodesByCountryCode(code);
            var customizedCurrency = currency.Select(x => new CustomizedCurrency
            {
                CurrencyCode = x.Code,
                CurrencyName = x.Name
            }).ToList();
            return Task.FromResult(customizedCurrency);
        }

        public Task<List<CustomizedRegion>> getCustomizedRegionAsync(string code, int offset = 1, int limit = 10, string? searchQuery = null)
        {
            var region = countryService.GetRegionByCountryCode(code);
            var customizedRegions = region.Select(x => new CustomizedRegion
            {
                RegionCode = x.ShortCode,
                RegionName = x.Name
            }).ToList();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                customizedRegions = customizedRegions.Where(x => x.RegionCode.ToLower().Contains(searchQuery) || x.RegionName.ToLower().Contains(searchQuery) ).ToList();
            }
            var paginatedRegion = customizedRegions.Skip((offset - 1) * limit).Take(limit).ToList();
            return Task.FromResult(paginatedRegion);
        }

        public Task<List<Regions>> GetRegionByCountryCode(string countryCode)
        {
            var region = countryService.GetRegionByCountryCode(countryCode);
            return Task.FromResult(region);
        }
    }
}
