using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductService.Models;
using ProductService.SyncDataService.Http;

namespace ProductService.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;
        private readonly IOrderDataClient _client;
        public ProductRepo(AppDbContext context, IOrderDataClient client)
        {
            _context = context;
            _client = client;
        }
        public Task CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Add(product);
            return Task.CompletedTask;
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

        public async Task DeleteProduct(int id)
        {

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
                if (product == null)
                {
                    throw new Exception($"produk id: {id} tidak tersedia di database");
                }
                _context.Products.Remove(product); 
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating product: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Product>> GetAllProduct()
        {
            return _context.Products.ToList();
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                throw new Exception($"Produk id: {id} tidak tersedia di database");
            }
            return product;
        }

        public async Task<Product> GetByName(string name)
        {
            var nameProduct = name.ToLower();
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == name);
            if (product == null)
            {
                throw new Exception($"Produk {name} tidak tersedia di database");
            }
            return product;
        }

        public async Task Update(int id, Product product)
        {
            try
            {
                var existingProduct = await GetById(product.ProductId);
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Stock = product.Stock;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating product: {ex.Message}");
            }
        }
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}