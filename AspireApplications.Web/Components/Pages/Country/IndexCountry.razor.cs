using AspireApplication.Models.Models;
using AspireApplications.Web.Service;
using Microsoft.AspNetCore.Components;

namespace AspireApplications.Web.Components.Pages.Country
{
    public partial class IndexCountry
    {
        private List<CustomizedCountry> CountryList = new();
        string? SearchQuery { get; set; } = null;
        private int CurrentPage = 1;
        private int PageSize = 10;
        private int TotalCount = 0;
        private int StartIndex => (CurrentPage - 1) * PageSize + 1;
        private int EndIndex => Math.Min(CurrentPage * PageSize, TotalCount);

        //private int TotalPages => (int)Math.Ceiling((decimal)TotalCount / (decimal)PageSize);
        //private bool IsFirstPage => CurrentPage == 1;
        //private bool IsLastPage => CurrentPage == PageSize;

        [Inject]
        public ICountryInterface countryService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadCountryRecords();
            await base.OnInitializedAsync();
        }

        private async Task LoadCountryRecords()
        {
            CountryList = await countryService.GetCountryRecords(CurrentPage, PageSize, SearchQuery);
            //TotalCount = CountryList.Count;
            //StateHasChanged();
        }

        private async Task PreviousPage()
        {
            CurrentPage--;
            await LoadCountryRecords();
        }

        private async Task NextPage()
        {
            CurrentPage++;
            await LoadCountryRecords();
        }

        private async Task GoToPage(int pageNumber)
        {
            CurrentPage = pageNumber;
            await LoadCountryRecords();
        }

        private async Task SearchProducts()
        {
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                CurrentPage = 1;
                await LoadCountryRecords();
            }
        }

        private void HandleAction(string action)
        {

        }
    }
}
