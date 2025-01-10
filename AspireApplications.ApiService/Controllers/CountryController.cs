using AspireApplication.Models.Models;
using AspireApplications.Services.Services;
using CountryData.Standard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspireApplications.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ILogger<CountryController> logger;
        private readonly ICountryInterface countryService;

        public CountryController(ILogger<CountryController> logger, ICountryInterface countryService)
        {
            this.logger = logger;
            this.countryService = countryService;
        }

        [HttpGet]
        [Route("GetAllCountryRecord")]
        public async Task<IActionResult> GetAllCountryRecord(int offset = 1, int limit = 20, string? searchQuery = null)
        {
            var country = await countryService.GetCountryAllData(offset, limit, searchQuery);
            logger.LogInformation("Class:CountryController | Method:GetAllCountryRecord | Start method | Params {0}", country.ToString());
            return Ok(country);
        }

        [HttpGet]
        [Route("GetCustomizedCountryRecord")]
        public async Task<IActionResult> GetCustomizedCountryRecord(int offset = 1, int limit = 20, string? searchQuery = null)
        {
            var country = await countryService.getCountryRecordsAsync(offset, limit, searchQuery);
            logger.LogInformation("Class:CountryController | Method:GetCustomizedCountryRecord | Start method | Params {0}", country.ToString());
            return Ok(country);
        }

        [HttpGet]
        [Route("GetCurrencyPerCountryCode")]
        public async Task<IActionResult> GetCurrencyPerCountryCode(string code)
        {
            var currency = await countryService.getCustomizedCurrencyAsync(code);
            logger.LogInformation("Class:CountryController | Method:GetCurrencyPerCountryCode | Start method | Params {0}", currency.ToString());
            return Ok(currency);
        }

        [HttpGet]
        [Route("GetRegionsPerCountryCode")]
        public async Task<IActionResult> GetRegionsPerCountryCode(string code, int offset = 1, int limit = 10, string? searchQuery = null)
        {
            var region = await countryService.getCustomizedRegionAsync(code, offset, limit, searchQuery);
            logger.LogInformation("Class:CountryController | Method:GetRegionsPerCountryCode | Start method | Params {0}", region.ToString());
            return Ok(region);
        }
    }
}
