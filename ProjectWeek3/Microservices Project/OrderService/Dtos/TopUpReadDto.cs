using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Dtos
{
    public class TopUpReadDto
    {
        
        [Required]
        public string Username { get; set; }
        [Required]
        public int Cash { get; set; }
    }
}