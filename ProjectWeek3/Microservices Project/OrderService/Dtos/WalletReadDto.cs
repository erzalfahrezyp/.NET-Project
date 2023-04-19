using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Dtos
{
    public class WalletReadDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Fullname { get; set; }
        [Required]
        public int Cash { get; set; }
    }
}