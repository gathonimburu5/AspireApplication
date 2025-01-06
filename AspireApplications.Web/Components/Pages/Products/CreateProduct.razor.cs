using AspireApplication.Models.Models;
using AspireApplications.Web.Service;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;

namespace AspireApplications.Web.Components.Pages.Products
{
    public partial class CreateProduct
    {
        [Inject]
        public IProductInterface ProductService { get; set; }
        [Inject]
        public IToastService toastService { get; set; }
        [Inject]
        public NavigationManager navigationManager { get; set; }
        public Product product { get; set; } = new Product();
        public async Task SaveProductRecords()
        {
            Console.WriteLine($"products: {product.ProductName}, {product.Description}, {product.Quantity}, {product.Price}");
            if (string.IsNullOrEmpty(product.ProductName))
            {
                Console.WriteLine("Product Name is required!!");
                return;
            }

            else if (string.IsNullOrEmpty(product.Description))
            {
                Console.WriteLine("Product Description is required!");
                return;
            }

            var result = await ProductService.CreateProductRecords(product);
            if (result.Code == 200)
            {
                toastService.ShowSuccess(result.Message);
                navigationManager.NavigateTo("/product");
            }
        }
    }
}
