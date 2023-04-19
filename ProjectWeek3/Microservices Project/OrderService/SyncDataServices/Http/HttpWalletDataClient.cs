using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using OrderService.Models;


namespace OrderService.SyncDataServices.Http
{
    public class HttpWalletDataClient : IWalletDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpWalletDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<IEnumerable<Wallet>> ReturnAllWallet()
        {
            var response = await _httpClient.GetAsync(_configuration["WalletService"]);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                Console.WriteLine($"{content}");

                var wallet = JsonSerializer.Deserialize<List<Wallet>>(content);
                if (wallet != null)
                {
                    Console.WriteLine($"{wallet.Count()} wallet returned from Wallet Service");
                    return wallet;
                }
                throw new Exception("No wallet found!");
            }
            else
            {
                throw new Exception("Unable to reach Wallet Service");
            }
        }
    }
}