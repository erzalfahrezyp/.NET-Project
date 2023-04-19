using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Price { get; set; }
        public Product Product { get; set; }

        [ForeignKey("Wallet")]
        public string Username { get; set; }
        public Wallet Wallet { get; set; }
    }
}