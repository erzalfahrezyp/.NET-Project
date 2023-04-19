using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WalletService.Dtos
{
    public class TopupWalletDto
    {
        [Required]
        public int Cash { get; set; }
    }
}