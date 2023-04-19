using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Dtos;
using WalletService.Models;

namespace WalletService.SyncDataService.Http
{
    public interface IOrderDataClient
    {
        Task<IEnumerable<Wallet>> ReturnAllWallet();
        Task SendWalletToOrder(WalletReadDto walletReadDto);
    }
}