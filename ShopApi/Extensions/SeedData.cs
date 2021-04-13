using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShopApi.Database.Entities;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShopApi.Extensions
{
    public static class SeedData
    {
        public static string DefaultBarcode = "076575693118";
        public static string[] ProductNames = { "Lapte integral Zuzu 3.5% grasime, 1L",
                                                "Lapte de consum Zuzu 1.8L, 3.5% grasime",
                                                "Lapte de consum Auchan 1 l",
                                                "Lapte batut Auchan 2% grasime, 330 g",
                                                "Lapte integral Zuzu 3.5% grasime, 1L",
                                                "Lapte de consum Zuzu 1.8L, 3.5% grasime",
                                                "Lapte de consum Auchan 1 l",
                                                "Lapte batut Auchan 2% grasime, 330 g" };

        public static string[] PathsToImage =
        {
            "zuzu_kefir.png",
            "zuzu_lapte.png",
            "zuzu_lapte3.5.png",
            "zuzu.jpg",
            "zuzu_kefir.png",
            "zuzu_lapte.png",
            "zuzu_lapte3.5.png",
            "zuzu.jpg",
            "zuzu_kefir.png",
            "zuzu_lapte.png",
            "zuzu_lapte3.5.png",
            "zuzu.jpg",

        };

        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            var brandRepo = services.GetService<BrandRepo>();
            var categoryRepo = services.GetService<CategoryRepo>();
            var productRepo = services.GetService<ProductRepo>();

            var logger = services.GetRequiredService<ILogger<Startup>>();

            await AddDefaultUser(userManager, roleManager, logger);

            await AddDefaultCategory(categoryRepo, logger);

            await AddDefaultBrand(brandRepo, logger);

            await AddDefaultProduct(productRepo, brandRepo, categoryRepo, logger);


        }

        private static async Task AddDefaultUser(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, ILogger<Startup> logger)
        {
            var checkDefaultUser = await userManager.Users
                .Where(x => x.Email == "flaviu_remus@yahoo.com")
                .SingleOrDefaultAsync();

            if (checkDefaultUser == null)
            {
                const string password = "Gps123456*";
                const string defaultRole = "defaultRole";
                var defaultBaseUser = new BaseUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "flaviu_remus@yahoo.com",
                    FirstName = "flaviu",
                    LastName = "Remus",
                    Email = "flaviu_remus@yahoo.com",
                    PhoneNumber = "0744594966",
                };

                var resultAddUSer = await userManager.CreateAsync(defaultBaseUser, password);

                if ((!await roleManager.RoleExistsAsync(defaultRole)))
                {
                    await roleManager.CreateAsync(new IdentityRole(defaultRole));

                }
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.GivenName, defaultBaseUser.FirstName),
                    new Claim(JwtRegisteredClaimNames.FamilyName, defaultBaseUser.LastName),
                    new Claim(JwtRegisteredClaimNames.NameId, defaultBaseUser.Id),
                    new Claim(JwtRegisteredClaimNames.Email, defaultBaseUser.Email),
                    new Claim(JwtRegisteredClaimNames.Typ, defaultRole),
                };

                await userManager.AddClaimsAsync(defaultBaseUser, claims);

                logger.LogInformation("Default User Added");

            }
        }

        public static async Task AddDefaultBrand(BrandRepo brandRepo, ILogger<Startup> logger)
        {
            if ((await brandRepo.GetByName("ProduseLucacesti")) == null)
            {
                var defaultBrand = new Brand
                {
                    Email = "ProduseLucacestiMM@yahoo.com",
                    Address = "Strada Somesului Nr5",
                    City = "Lucacesti",
                    Country = "Romania",
                    Name = "ProduseLucacesti",
                    Phone = "0744584966"
                };
                await brandRepo.SaveAsync(defaultBrand);

                logger.LogInformation("Default Brand Added");
            }
        }

        public static async Task<Category> AddDefaultCategory(CategoryRepo categoryRepo, ILogger<Startup> logger)
        {
            if ((await categoryRepo.GetByName("defaultCategory")) == null)
            {
                var defaultCategory = new Category
                {
                    ParentCategory = null,
                    Name = "defaultCategory"
                };
                await categoryRepo.SaveAsync(defaultCategory);

                logger.LogInformation("Default Category Added");
            }

            return await categoryRepo.GetByName("defaultCategory");
        }

        public static async Task AddDefaultProduct(ProductRepo productRepo, BrandRepo brandRepo, CategoryRepo categoryRepo, ILogger<Startup> logger)
        {
            var count = 0;
            var attributes = new List<MyField>();
            var category = await categoryRepo.GetByName("defaultCategory");

            attributes.Add(new MyField { Name = "Grasimi", Value = "186", InfoCategory = "NutritionInfo", IsImportant = true });
            attributes.Add(new MyField { Name = "Glucide ", Value = "4.7", InfoCategory = "NutritionInfo", IsImportant = true });
            attributes.Add(new MyField { Name = "Alergeni descriere", Value = "Lapte", InfoCategory = "IngredientsAndAllergens", IsImportant = false });
            attributes.Add(new MyField { Name = "Greutate", Value = "1kg", InfoCategory = "Size", IsImportant = true });
            attributes.Add(new MyField { Name = "Ingrediente", Value = "Lapte", InfoCategory = "IngredientsAndAllergens", IsImportant = false });

            var brand = await brandRepo.GetByName("ProduseLucacesti");

            foreach (var productName in SeedData.ProductNames)
            {
                var newProd = new Product
                {
                    Title = productName,

                    Barcode = SeedData.DefaultBarcode + count++,
                    Brand = brand,
                    Category = category,
                    PathToImage = SeedData.PathsToImage[count],
                    Score = new Random().Next(1, 5),
                    UnitsAvailable = new Random().Next(1, 20),
                    Availability = DateTime.Today,
                    OldPrice = new Random().Next(10, 15),
                    NewPrice = new Random().Next(5, 10),
                    Discount = new Random().Next(10, 25),
                    Attributes = JsonConvert.SerializeObject(attributes)


                };
                await productRepo.SaveAsync(newProd);
            }
            logger.LogInformation("Default Products Added");
        }




        public class MyField
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string InfoCategory { get; set; }
            public bool IsImportant { get; set; } = true;
        }
    }
}
