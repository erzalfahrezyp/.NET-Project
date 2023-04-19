using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Models;

namespace ProductService.Data
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAllProduct();
        Task<Product> GetById(int id);
        Task<Product> GetByName(string name);
        Task CreateProduct(Product product);
        Task CreateProduct();
        Task Update(int id, Product product);
        Task DeleteProduct(int id);
        bool SaveChanges();
    }
}