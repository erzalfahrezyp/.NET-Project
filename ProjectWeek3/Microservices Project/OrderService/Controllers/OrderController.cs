using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OrderService.Data;
using OrderService.Dtos;
using OrderService.Models;
using AutoMapper;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepo _repo;

        public OrderController(IMapper mapper, IOrderRepo repo)
        {
            _mapper = mapper;
            _repo = repo;
        }
        
        [HttpGet]
        public async Task <ActionResult<IEnumerable<OrderReadDto>>> GetOrderAll()
        {
            var orders = await _repo.GetOrderAll();
            var listOrders = _mapper.Map<IEnumerable<OrderReadDto>>(orders);
            return Ok(listOrders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _repo.GetOrderById(id);
            var readOrderDto = _mapper.Map<OrderReadDto>(order);
            return Ok(readOrderDto);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto createOrderDto)
        {
            var order = _mapper.Map<Order>(createOrderDto);
            await _repo.CreateOrder(order);
            _repo.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
        }
    }
}