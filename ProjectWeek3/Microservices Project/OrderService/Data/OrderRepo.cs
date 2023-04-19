using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data
{
    public class OrderRepo : IOrderRepo
    {
        private readonly AppDbContext _context;
        public OrderRepo(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Order> CreateOrder(Order order)
        {
            var product = await _context.Products.FindAsync(order.ProductId);
            var wallet = await _context.Wallets.FindAsync(order.Username);

            
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            
            if (product.Stock < order.Quantity)
            {
                throw new Exception("Product is out of stock");
            }
            
            var price = product.Price * order.Quantity;
            order.Price = price;

            if (wallet.Cash < order.Price)
            {
                throw new Exception("Not enough cash");
            }

            product.Stock -= order.Quantity;
            wallet.Cash -= order.Price;
            order.OrderDate = DateTime.UtcNow;

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrderAll()
        {
            return await _context.Orders.Include(o => o.Product).Include(o => o.Wallet).ToListAsync();
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders.Include(o => o.Product).Include(o => o.Wallet).FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}