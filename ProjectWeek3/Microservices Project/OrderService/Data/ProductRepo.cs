using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;
using OrderService.SyncDataServices.Http;

namespace OrderService.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;
        private readonly IProductDataClient _client;

        public ProductRepo(AppDbContext context, IProductDataClient client)
        {
            _context = context;
            _client = client;
        }
        public async Task CreateProduct()
        {
            try
            {
                var existingProduct = _context.Products.ToList();
                foreach (var productToDelete in existingProduct)
                {
                    _context.Products.Remove(productToDelete);
                }

                var product = await _client.ReturnAllProduct();
                foreach (var item in product)
                {
                    _context.Products.Add(new Product
                    {
                        ProductId = item.ProductId,
                        Name = item.Name,
                        Stock = item.Stock,
                        Description = item.Description,
                        Price = item.Price
                    });
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't save changes to the database: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return _context.Products.ToList();
        }
    }
}