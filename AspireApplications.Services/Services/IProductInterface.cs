using AspireApplication.Models.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireApplications.Services.Services
{
    public interface IProductInterface
    {
        List<Product> GetProductsAsync(int offset = 1, int limit = 20, string? searchQuery = null);
        MyResponses CreateProductRecords(Product product);
        MyResponses UpdateProductRecords(Product product);
        Product GetProductPerId(int productId);
        bool DeleteProductPerId(int productId);
    }

    public class ProductService : IProductInterface
    {
        private readonly ApplicationDbContext context;
        public ProductService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public MyResponses CreateProductRecords(Product product)
        {
            MyResponses response = new MyResponses();
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    var checkProductNameIfExist = context.Products.FirstOrDefault(x => x.ProductName == product.ProductName);
                    if(checkProductNameIfExist != null)
                    {
                        response.Code = 400;
                        response.Message = "product name already registered, try again!!";
                        return response;
                    }
                    Product pd = new Product
                    {
                        ProductName = product.ProductName,
                        Description = product.Description,
                        Quantity = product.Quantity,
                        Price = product.Price,
                        CreatedOn = DateTime.UtcNow
                    };
                    context.Products.Add(pd);
                    context.SaveChanges();
                    trans.Commit();
                    response.Code = 200;
                    response.Message = "successfully created product record";
                    return response;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    context.Dispose();
                    response.Code = 400;
                    response.Message = "failed to create product record!😒";
                    throw e;
                    return response;
                }
            }
        }

        public bool DeleteProductPerId(int productId)
        {
            var product = context.Products.FirstOrDefault(x => x.Id == productId);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Product GetProductPerId(int productId)
        {
            var product = context.Products.FirstOrDefault(x => x.Id == productId);
            if(product != null)
            {
                return product;
            }
            return null;
        }

        public List<Product> GetProductsAsync(int offset = 1, int limit = 20, string? searchQuery = null)
        {
            var product = context.Products.OrderBy(x => x.Id).AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                product = product.Where(x => x.ProductName.ToLower().Contains(searchQuery) || x.Description.ToLower().Contains(searchQuery) );
            }
            return product.OrderBy(x => x.Id).Skip((offset - 1) * limit).Take(limit).ToList();
        }

        public MyResponses UpdateProductRecords(Product product)
        {
            MyResponses response = new MyResponses();
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    var prod = context.Products.FirstOrDefault(x => x.Id == product.Id);
                    if (prod != null)
                    {
                        prod.ProductName = product.ProductName;
                        prod.Description = product.Description;
                        prod.Quantity = product.Quantity;
                        prod.Price = product.Price;

                        context.SaveChanges();
                        trans.Commit();
                        response.Code = 200;
                        response.Message = "successfully updated product records";
                        return response;
                    }
                    return null;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    context.Dispose();
                    response.Code = 400;
                    response.Message = "failed to update product records!!";
                    return response;
                    throw e;
                }
            }
        }
    }
}
