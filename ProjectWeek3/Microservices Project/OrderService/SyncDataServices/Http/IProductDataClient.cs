using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.SyncDataServices.Http
{
    public interface IProductDataClient
    {
        Task<IEnumerable<Product>> ReturnAllProduct();
    }
}