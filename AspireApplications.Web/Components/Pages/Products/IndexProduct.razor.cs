using AspireApplication.Models.Models;
using AspireApplications.Web.Components.BaseComponents;
using AspireApplications.Web.Service;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AspireApplications.Web.Components.Pages.Products
{
    public partial class IndexProduct : ComponentBase
    {
        private List<Product> products = new();
        private string? SearchQuery = null;
        private int CurrentPage = 1;
        private int Pagesize = 10;
        private int TotalCount = 0;

        private int StartIndex => (CurrentPage - 1) * Pagesize + 1;
        private int EndIndex => Math.Min(CurrentPage * Pagesize, TotalCount);
        private int TotalPages => (int)Math.Ceiling((double)TotalCount / Pagesize);

        private bool IsFirstPage => CurrentPage == 1;
        private bool IsLastPage => CurrentPage == TotalPages;

        [Inject]
        public IProductInterface productService { get; set; }
        public IToastService toastService { get; set; }
        public AppModal Modal { get; set; }
        [Parameter]
        public int deleteId { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadProductRecords();
            await base.OnInitializedAsync();
        }

        public async Task LoadProductRecords()
        {
            products = await productService.GetProductRecords(CurrentPage, Pagesize, SearchQuery);
            TotalCount = products.Count;
        }

        public async Task DeleteProductRecord()
        {        
            var result = await productService.DeleteProductRecord(deleteId);
            if(result.Code == 200)
            {
                toastService.ShowSuccess("successfully deleted product detail");
                products = await productService.GetProductRecords();
                Modal.Close();
            }
        }

        private async Task PreviousPage()
        {
            if (!IsFirstPage)
            {
                CurrentPage--;
                await LoadProductRecords();
            }
        }

        private async Task NextPage()
        {
            if (!IsLastPage)
            {
                CurrentPage++;
                await LoadProductRecords();
            }
        }

        private async Task GoToPage(int page)
        {
            if (page != CurrentPage)
            {
                CurrentPage = page;
                await LoadProductRecords();
            }
        }

        private async Task SearchProducts()
        {
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                CurrentPage = 1;
                await LoadProductRecords();
            }
        }
    }
}
