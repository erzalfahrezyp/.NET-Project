using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, OrderReadDto>();
            //.ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product.ProductId));
            CreateMap<Order, AllOrderReadDto>();
            CreateMap<Product, ProductReadDto>();
            CreateMap<Wallet, WalletReadDto>();
        }
    }
}