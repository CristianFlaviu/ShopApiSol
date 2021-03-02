using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Dto
{
    public class ConfirmationEmailDto
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
