using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class ProductReadDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}