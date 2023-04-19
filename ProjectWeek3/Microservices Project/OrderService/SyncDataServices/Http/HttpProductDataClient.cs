using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderService.Models;

namespace OrderService.SyncDataServices.Http
{
    public class HttpProductDataClient : IProductDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        
        public HttpProductDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<IEnumerable<Product>> ReturnAllProduct()
        {
            var response = await _httpClient.GetAsync(_configuration["ProductService"]);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                Console.WriteLine($"{content}");

                var product = JsonSerializer.Deserialize<List<Product>>(content);
                if (product != null)
                {
                    Console.WriteLine($"{product.Count()} product returned from Product Service");
                    return product;
                }
                throw new Exception("No product found!");
            }
            else
            {
                throw new Exception("Unable to reach Product Service");
            }
        }
    }
}