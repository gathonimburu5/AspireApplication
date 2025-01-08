using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireApplication.Models.Models
{
    public class CustomizedCountry
    {
        public string CountryName { get; set; }
        public string PhoneCode { get; set; }
        public string ShortCode { get; set; }
        public string CountryFlag { get; set; }
    }

    public class CustomizedCurrency
    {
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class CustomizedRegion
    {
        public string RegionName { get; set; }
        public string RegionCode { get; set; }
    }
}
