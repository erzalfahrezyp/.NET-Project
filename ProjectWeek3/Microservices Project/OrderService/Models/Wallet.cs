using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Models
{
    public class Wallet
    {
        [Key]
        public string Username { get; set; }
        public string Fullname { get; set; }
        public int Cash { get; set; }
        //public ICollection<Order> Orders { get; set; }
    }
}