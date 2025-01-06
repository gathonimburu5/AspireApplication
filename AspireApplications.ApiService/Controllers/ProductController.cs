using AspireApplication.Models.Models;
using AspireApplications.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AspireApplications.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductInterface productService;
        private readonly ILogger<ProductController> logger;
        public ProductController(IProductInterface productService, ILogger<ProductController> logger)
        {
            this.productService = productService;
            this.logger=logger;
        }

        [HttpGet]
        [Route("GetProductList")]
        public IActionResult GetProductList(int offset = 1, int limit = 20, string? searchQuery = null)
        {
            var product = productService.GetProductsAsync(offset, limit, searchQuery);
            logger.LogInformation("Class:ProductController | Method:GetProductList | Start method | Params {0}", product.ToString());
            return Ok(product);
        }

        [HttpGet]
        [Route("GetProductPerId")]
        public IActionResult GetProductPerId(int id)
        {
            var product = productService.GetProductPerId(id);
            logger.LogInformation("Class:ProductController | Method:GetProductPerId | Start method | Params {0}", product.ToString());
            return Ok(product);
        }

        [HttpPost]
        [Route("CreateProductDetails")]
        public IActionResult CreateProductDetails([FromBody]Product model)
        {
            if (!ModelState.IsValid)
            {
                logger.LogInformation("Class:ProductController | Method:CreateProductDetails | Start method | Params {0}", model.ToString());
                return Ok(new MyResponses { Code = 400, Message = "Fields cannot be empty, Please fill all Fields!!" });
            }

            var product = productService.CreateProductRecords(model);
            logger.LogInformation("Class:ProductController | Method:CreateProductDetails | Start method | Params {0}", product.Message);
            return Ok(product);
        }

        [HttpPut]
        [Route("EditProductRecords")]
        public IActionResult EditProductRecords([FromBody]Product model, int id)
        {
            if (!ModelState.IsValid)
            {
                logger.LogInformation("Class:ProductController | Method:EditProductRecords | Start method | Params {0}", model.ToString());
                return Ok(new MyResponses { Code = 400, Message = "Fields cannot be empty, Please fill all Fields!!" });
            }

            var products = productService.GetProductPerId(id);
            if (products != null)
            {
                var result = productService.UpdateProductRecords(model);
                logger.LogInformation("Class:ProductController | Method:EditProductRecords | Start method | Params {0}", result.Message);
                return Ok(result);
            }
            else
            {
                logger.LogInformation("Class:ProductController | Method:EditProductRecords | Start method | Params {0}", products.ToString());
                return Ok(new MyResponses { Code = 400, Message = $"failed to get product per id: {id}" });
            }
        }

        [HttpDelete]
        [Route("DeleteProductRecords")]
        public IActionResult DeleteProductRecords(int id)
        {
            MyResponses response = new MyResponses();
            if (!ModelState.IsValid)
            {
                response.Code = 400;
                response.Message = "Fields cannot be empty, Please fill all Fields 😒😒😒!!";
                logger.LogInformation("Class:ProductController | Method:DeleteProductRecords | Start method | Params {0}", response.Message);
                return Ok(response);
            }

            var product = productService.DeleteProductPerId(id);
            if (product)
            {
                response.Code = 200;
                response.Message = "successfully deleted product records ❤️";
                logger.LogInformation("Class:ProductController | Method:DeleteProductRecords | Start method | Params {0}", response.Message);
                return Ok(response);
            }
            else
            {
                response.Code = 200;
                response.Message = "failed to delete product records 😒😒!!";
                logger.LogInformation("Class:ProductController | Method:DeleteProductRecords | Start method | Params {0}", response.Message);
                return Ok(response);
            }
        }
    }
}
