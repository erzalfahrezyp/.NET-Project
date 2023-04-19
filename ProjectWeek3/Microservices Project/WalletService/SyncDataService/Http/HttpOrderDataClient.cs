using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using WalletService.Dtos;
using WalletService.Models;

namespace WalletService.SyncDataService.Http
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

        public async Task<IEnumerable<Wallet>> ReturnAllWallet()
        {
            var response = await _httpClient.GetAsync(_configuration["OrderService"]);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                Console.WriteLine($"{content}");

                var wallet = JsonSerializer.Deserialize<List<Wallet>>(content);
                if (wallet != null)
                {
                    Console.WriteLine($"{wallet.Count()} wallet returned from Order Service");
                    return wallet;
                }
                throw new Exception("No wallet found!");
            }
            else
            {
                throw new Exception("Unable to reach Wallet Service");
            }
        }
        public async Task SendWalletToOrder(WalletReadDto walletReadDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(walletReadDto),
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