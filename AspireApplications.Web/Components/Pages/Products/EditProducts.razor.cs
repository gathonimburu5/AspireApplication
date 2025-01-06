using AspireApplication.Models.Models;
using AspireApplications.Web.Service;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace AspireApplications.Web.Components.Pages.Products
{
    public partial class EditProducts : ComponentBase
    {
        public Product product { get; set; }
        [Parameter]
        public int ProdId { get; set; }
        [Inject]
        public IProductInterface productService { get; set; }
        [Inject]
        public IToastService toastService { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            product = await productService.GetProductPerId(ProdId);
        }

        public async Task UpdateProductRecords()
        {
            var result = await productService.UpdateProductRecords(product, ProdId);
            if(result.Code == 200)
            {
                toastService.ShowSuccess(result.Message);
                navigationManager.NavigateTo("/product");
            }
            else
            {
                toastService.ShowSuccess(result.Message);
            }
        }
    }
}
