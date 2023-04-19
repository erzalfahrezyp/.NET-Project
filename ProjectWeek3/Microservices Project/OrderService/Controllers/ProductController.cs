using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OrderService.Dtos;
using OrderService.Data;
using OrderService.Models;
using AutoMapper;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/order/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;

        public ProductController(IProductRepo productRepo)
        {
            _productRepo = productRepo;
        }
        
        [HttpPost("Sync")]
        public async Task<ActionResult> SyncProduct()
        {
            try
            {
                await _productRepo.CreateProduct();
                return Ok("Products Synced");
            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't sync product: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            Console.WriteLine("==> Getting product from Product Service");
            var productItems = await _productRepo.GetAllProduct();
            return Ok(productItems);
        }
    }
}