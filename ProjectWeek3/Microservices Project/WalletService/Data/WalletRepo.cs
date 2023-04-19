using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WalletService.Models;
using WalletService.SyncDataService.Http;

namespace WalletService.Data
{
    public class WalletRepo : IWalletRepo
    {
        private readonly AppDbContext _context;
        private readonly IOrderDataClient _client;
        public WalletRepo(AppDbContext context, IOrderDataClient client)
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
        public Task Create(Wallet wallet)
        {
            if (wallet == null)
            {
                throw new ArgumentException(nameof(wallet));
            }
            _context.Wallets.Add(wallet);
            return Task.CompletedTask;
        }
        public async Task Edit(string username, Wallet wallet)
        {
            try
            {
                var editWallet = await GetByUsername(username);
                editWallet.Fullname = wallet.Fullname;
                wallet.Cash = editWallet.Cash;
            }
            catch (Exception ex)
            {
                throw new Exception($"Edited wallet error!");
            }
        }

        public string GenerateId()
        {
            var count = _context.Wallets.Count();
            count++;
            var usernameWallet = $"user{count}";
            return usernameWallet;
        }
        public IEnumerable<Wallet> GetAllWallet()
        {
            return _context.Wallets.ToList();
        }

        public async Task<Wallet> GetByUsername(string username)
        {
            var nameWallet = username.ToLower();
            var wallet = await _context.Wallets.FirstOrDefaultAsync(p => p.Username.ToLower() == username);
            if (wallet == null)
            {
                throw new Exception($"Wallet {username} is not found");
            }
            return wallet;
        }

        public async Task TopupWallet(string username, Wallet wallet)
        {
            try
            {
                var existingWallet = await GetByUsername(username);
                if (existingWallet == null)
                {
                    throw new Exception($"Wallet {username} is not found");
                }
                int topUp = existingWallet.Cash + wallet.Cash;
                existingWallet.Cash = topUp;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating product: {ex.Message}");
            }
        }

        public async Task CashOutWallet(string username, Wallet wallet)
        {
            try
            {
                var existingWallet = await GetByUsername(username);
                if (existingWallet == null)
                {
                    throw new Exception($"Wallet {username} is not found");
                }
                int cashOut = existingWallet.Cash - wallet.Cash;
                existingWallet.Cash = cashOut;
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