using Microsoft.AspNetCore.Identity;
using ShopApi.Database.Entities.ProductManagement;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ShopApi.Database.Entities
{
    public class BaseUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public List<Product> FavoritesProducts { get; set; }
        [JsonIgnore]
        public List<Product> ShoppingCartProducts { get; set; }
    }
}
