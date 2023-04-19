using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OrderService.Data;

namespace OrderService.Controllers
{
    [Route("api/order/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepo _walletRepo;

        public WalletController(IWalletRepo walletRepo)
        {
            _walletRepo = walletRepo;
        }

        [HttpPost("Sync")]
        public async Task<ActionResult> SyncWallet()
        {
            try
            {
                await _walletRepo.CreateWallet();
                return Ok("Wallet Synced");
            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't sync wallet: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            Console.WriteLine("==> Getting wallet from Wallet Service");
            var walletItem = await _walletRepo.GetAllWallet();
            return Ok(walletItem);
        }
    }
}