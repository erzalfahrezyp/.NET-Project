using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WalletService.Dtos;
using WalletService.Models;

namespace WalletService.Profiles
{
    public class WalletProfiles : Profile
    {
        public WalletProfiles()
        {
            CreateMap<Wallet, WalletReadDto>();
            CreateMap<Wallet, EditWalletDto>();
            CreateMap<CreateWalletDto, Wallet>();
            CreateMap<EditWalletDto, Wallet>();
            CreateMap<TopupWalletDto, Wallet>();
            CreateMap<CashOutWalletDto, Wallet>();
        }
    }
}