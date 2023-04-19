using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WalletService.Dtos;
using System.ComponentModel.DataAnnotations;

namespace WalletService.Dtos
{
    public class WalletReadDto
    {
        [Key]
        // public int Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public int Cash { get; set; }
    }
}