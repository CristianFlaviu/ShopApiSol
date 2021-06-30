using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Dto
{
    public class ConfirmationEmail
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
