using System.ComponentModel.DataAnnotations;

namespace ShopApi.Dto
{
    public class LoginDto
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
