using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WalletService.Data;
using WalletService.Dtos;
using WalletService.Models;
using WalletService.SyncDataService.Http;

namespace WalletService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletRepo _repo;
        private readonly IMapper _mapper;
        private readonly IOrderDataClient _orderDataClient;

        public WalletController(IWalletRepo repo, IMapper mapper, IOrderDataClient orderDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _orderDataClient = orderDataClient;
        }

        [HttpPost("Sync")]
        public async Task<ActionResult> SyncWallet()
        {
            try
            {
                await _repo.CreateWallet();
                return Ok("Wallet Synced");
            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't sync wallet: {ex.Message}");
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<WalletReadDto>> GetAllWallet()
        {
            var walletItem = _repo.GetAllWallet();
            var walletReadDtoList = _mapper.Map<IEnumerable<WalletReadDto>>(walletItem);
            return Ok(walletReadDtoList);
        }

        [HttpGet("{username}", Name = "GetWalletByUsername")]
        public async Task<ActionResult> GetWalletByUsername(string username)
        {
            var wallet = await _repo.GetByUsername(username);
            var walletRead = _mapper.Map<WalletReadDto>(wallet);
            return Ok(walletRead);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateWallet(CreateWalletDto createWalletDto)
        {
            var walletModel = _mapper.Map<Wallet>(createWalletDto);
            var usernameWallet = _repo.GenerateId();
            walletModel.Username = usernameWallet;
            walletModel.Cash = 0;
            await _repo.Create(walletModel);
            _repo.SaveChanges();

            var readWallet = _mapper.Map<WalletReadDto>(walletModel);
            try
            {
                await _orderDataClient.SendWalletToOrder(readWallet);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"==> Could not send synchronous data: {ex.Message}");
            }
            return Ok(readWallet);
        }

        [HttpPut("Edit/{username}")]
        public async Task<IActionResult> EditWallet(string username, EditWalletDto editWalletDto)
        {
            try
            {
                var walletModel = _mapper.Map<Wallet>(editWalletDto);
                walletModel.Username = username;
                await _repo.Edit(username, walletModel);
                _repo.SaveChanges();

                var readWalletDto = _mapper.Map<WalletReadDto>(walletModel);
                return Ok(readWalletDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("TopUp/{username}", Name = "TopupUsernameWallet")]
        public async Task<ActionResult> TopupWallet(string username, TopupWalletDto topupWalletDto)
        {
            try
            {
                var wallet = _mapper.Map<Wallet>(topupWalletDto);
                wallet.Username = username;
                await _repo.TopupWallet(username, wallet);
                _repo.SaveChanges();

                var returnWallet = await _repo.GetByUsername(username);
                var walletReadDto = _mapper.Map<WalletReadDto>(returnWallet);
                return Ok(walletReadDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Pay/{username}", Name = "OrderWithWallet")]
        public async Task<ActionResult> CashOutWallet(string username, CashOutWalletDto cashOut)
        {
            try
            {
                var wallet = _mapper.Map<Wallet>(cashOut);
                wallet.Username = username;
                await _repo.CashOutWallet(username, wallet);
                _repo.SaveChanges();

                var returnWallet = await _repo.GetByUsername(username);
                var walletReadDto = _mapper.Map<WalletReadDto>(returnWallet);
                return Ok(walletReadDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}