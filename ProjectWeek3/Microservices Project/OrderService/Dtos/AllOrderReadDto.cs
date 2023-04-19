using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Dtos
{
    public class AllOrderReadDto
    {
        public int OrderId { get; set; }
        public string Username { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; }
    }
}