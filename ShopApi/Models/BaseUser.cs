using System;
using Microsoft.AspNetCore.Identity;

namespace ShopApi.Models
{
    public class BaseUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
