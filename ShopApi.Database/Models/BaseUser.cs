using System;
using Microsoft.AspNetCore.Identity;

namespace ShopApi.Database.Models
{
    public class BaseUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
