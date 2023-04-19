using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using OrderService.Models;

namespace OrderService.Dtos
{
    public class OrderReadDto
    {
        public int OrderId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
        public int TotalPay { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Wallet")]
        public string Username { get; set; }
    }
}