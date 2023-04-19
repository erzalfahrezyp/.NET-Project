using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.Data;
using ProductService.Dtos;
using ProductService.Models;
using ProductService.SyncDataService.Http;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _repo;
        private readonly IMapper _mapper;
        private readonly IOrderDataClient _orderDataClient;

        public ProductController(IProductRepo repo, IMapper mapper, IOrderDataClient orderDataClient)
        {
            _repo = repo;
            _mapper = mapper;
            _orderDataClient = orderDataClient;
        }

        [HttpPost("Sync")]
        public async Task<ActionResult> SyncProduct()
        {
            try
            {
                await _repo.CreateProduct();
                return Ok("Products Synced");
            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't sync product: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetProduct()
        {
            Console.WriteLine("==> Getting Product...");
            var productItem = await _repo.GetAllProduct();
           
            return Ok(productItem);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var product = await _repo.GetById(id);
            var productRead = _mapper.Map<ProductReadDto>(product);

            return Ok(productRead);
        }

        [HttpGet("Name/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var productName = await _repo.GetByName(name);
            var productNameRead = _mapper.Map<ProductReadDto>(productName);

            return Ok(productNameRead);
        }

        [HttpPut("Edit/{id}")]
        public async Task<ActionResult> UpdateProduct(int id, UpdateProductDto updateProductDto)
        {
            try
            {
                var product = _mapper.Map<Product>(updateProductDto);
                product.ProductId = id;
                await _repo.Update(id, product);
                _repo.SaveChanges();

                var productReadDto = _mapper.Map<ProductReadDto>(product);
                return Ok(productReadDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            try
            {
                await _repo.DeleteProduct(id);
                _repo.SaveChanges();

                return Ok("Produk berhasil dihapus");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ProductReadDto>> CreateProduct(CreateProductDto createProductDto)
        {
            var productModel = _mapper.Map<Product>(createProductDto);
            await _repo.CreateProduct(productModel);
            _repo.SaveChanges();
            
            var productReadDto = _mapper.Map<ProductReadDto>(productModel);

            try
            {
                await _orderDataClient.SendProductToOrder(productReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"==> Could not send synchronous data: {ex.Message}");
            }
            return Ok(productReadDto);
        }
    }
}