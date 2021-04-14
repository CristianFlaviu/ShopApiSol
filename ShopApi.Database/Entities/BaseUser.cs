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
        public List<ProductsUsersShoppingCart> ProductsUsersShopping { get; set; }
        [JsonIgnore]
        public List<ProductsUserFavorite> ProductUserFavorites { get; set; }


    }
}
