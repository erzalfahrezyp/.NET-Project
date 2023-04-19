using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderService.Models;
using OrderService.SyncDataServices.Http;

namespace OrderService.Data
{
    public class WalletRepo : IWalletRepo
    {
        private readonly AppDbContext _context;
        private readonly IWalletDataClient _client;
        public WalletRepo(AppDbContext context, IWalletDataClient client)
        {
            _context = context;
            _client = client;
        }
        public async Task CreateWallet()
        {
            try
            {
                var existingWallet = _context.Wallets.ToList();
                foreach(var walletToDelete in existingWallet)
                {
                    _context.Wallets.Remove(walletToDelete);
                }

                var wallet = await _client.ReturnAllWallet();
                foreach (var item in wallet)
                {
                    _context.Wallets.Add(new Wallet
                    {
                        Username = item.Username,
                        Fullname = item.Fullname,
                        Cash = item.Cash
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
        public async Task<IEnumerable<Wallet>> GetAllWallet()
        {
            return _context.Wallets.ToList();
        }
    }
}