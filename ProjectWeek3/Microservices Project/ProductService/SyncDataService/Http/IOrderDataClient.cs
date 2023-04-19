using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.SyncDataService.Http
{
    public interface IOrderDataClient
    {
        Task<IEnumerable<Product>> ReturnAllProduct();
        Task SendProductToOrder(ProductReadDto productReadDto);
    }
}