using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Dtos
{
    public class ProductReadDto
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int Stock { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}