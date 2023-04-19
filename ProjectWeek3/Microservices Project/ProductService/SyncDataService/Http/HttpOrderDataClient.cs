using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.SyncDataService.Http
{
    public class HttpOrderDataClient : IOrderDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public HttpOrderDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<IEnumerable<Product>> ReturnAllProduct()
        {
            var response = await _httpClient.GetAsync(_configuration["OrderService"]);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                Console.WriteLine($"{content}");

                var product = JsonSerializer.Deserialize<List<Product>>(content);
                if (product != null)
                {
                    Console.WriteLine($"{product.Count()} product returned from Order Service");
                    return product;
                }
                throw new Exception("No product found!");
            }
            else
            {
                throw new Exception("Unable to reach Order Service");
            }
        }
        public async Task SendProductToOrder(ProductReadDto prod)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(prod),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _httpClient.PostAsync($"{_configuration["OrderService"]}", httpContent);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("==> Sync POST to OrderService was OK!");
            }
            else
            {
                Console.WriteLine("==> Sync POST to OrderService was NOT OK!");
            }
        }
    }
}