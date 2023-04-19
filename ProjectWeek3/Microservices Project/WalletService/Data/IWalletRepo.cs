using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Models;

namespace WalletService.Data
{
    public interface IWalletRepo
    {
        IEnumerable<Wallet> GetAllWallet();
        Task Create(Wallet wallet);
        Task CreateWallet();
        Task Edit(string username, Wallet wallet);
        Task<Wallet> GetByUsername(string username);
        Task TopupWallet(string name, Wallet wallet);
        Task CashOutWallet(string name, Wallet wallet);
        string GenerateId();
        bool SaveChanges();
    }
}