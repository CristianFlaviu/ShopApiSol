using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Dto
{
    public class LoginDto
    {
       
        public string Username { get; set; }
        public string Password { get; set; }
        //public string Grant_type { get; set; }
        //public string Scope { get; set; }
        //public string Client_id { get; set; }

    }
}
