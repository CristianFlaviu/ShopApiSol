using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShopApi.Database.Data;
using ShopApi.Database.Entities;
using ShopApi.Database.Entities.ProductManagement;
using ShopApi.Repository;
using System;
using System.Collections;
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
        public static string[] ProductNames = { "Lapte integral Zuzu ",
                                                "Lapte de consum Zuzu",
                                                "Lapte de consum Auchan ",
                                                "Lapte batut Auchan",
                                                "Lapte integral Zuzu",
                                                "Lapte de consum Zuzu 1.8L, 3.5% grasime",
                                                "Lapte de consum Auchan 1 l",
                                                "Lapte batut Auchan 2% grasime, 330 g" };
        public static string[] ShortProductNames = { "Lapte integral",
                                                "Lapte de consum ",
                                                "Lapte de consum ",
                                                "Lapte batut ",
                                                "Lapte integral ",
                                                "Lapte de consum ",
                                                "Lapte de consum",
                                                "Lapte batut Auchan" };

        public static string[] PathsToImage =
        {

            "https://i.ibb.co/Sd5XFfc/i-edit.png",
            "https://i.ibb.co/y8f7NG3/iaurt-edit.png",
            "https://i.ibb.co/KFwQtZv/Almette-Smantana.png",
            "https://i.ibb.co/zSYLCX6/iaurt-grecesc-10-grasime-olympus-150g-8827871559710.png",
            "https://i.ibb.co/kBqkyns/iaurt-natural-bakoma.png",
            "https://i.ibb.co/L9vtRKR/iaurt-usurel-de-baut-330g.png",
            "https://i.ibb.co/kqRZHqQ/lapte-batut-01.png",
            "https://i.ibb.co/xX2xgfV/napolact-branza-burduf-ii-400g-32642.png",
            "https://i.ibb.co/Q6KmMNT/salam-sergiana-brasov-crud-uscat-vid-cantitate-variabila-8903702839326.png"

        };

        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            var dataContext = services.GetService<DataContext>();

            var logger = services.GetRequiredService<ILogger<Startup>>();

            /* ###############################   BRANDS  ############################### */
            if (dataContext.Users.FirstOrDefault(x => x.Email == "flaviu_remus@yahoo.com") == null)
            {
                await AddDefaultUser(userManager, roleManager, logger);
                var zuzuBrand = new Brand
                {
                    Email = "Zuzu@yahoo.com",
                    Address = "Sectorul 5",
                    City = "Bucuresti",
                    Country = "Romania",
                    Name = "Zuzu",
                    Phone = "0744584911"
                };

                var fulgaBrand = new Brand
                {
                    Email = "fulga@yahoo.com",
                    Address = "Sectorul 5",
                    City = "Bucuresti",
                    Country = "Romania",
                    Name = "Fulga",
                    Phone = "0744584911"
                };

                var covalactBrand = new Brand
                {
                    Email = "covalact @yahoo.com",
                    Address = "Sectorul 5",
                    City = "Bucuresti",
                    Country = "Romania",
                    Name = "Covalact ",
                    Phone = "0744584911"
                };

                await dataContext.Brands.AddAsync(zuzuBrand);
                await dataContext.Brands.AddAsync(fulgaBrand);
                await dataContext.Brands.AddAsync(covalactBrand);

                /* ###############################   CATEGORIES   ############################### */

                var food = new Category
                {
                    ParentCategory = null,
                    Name = "Food"
                };
                await dataContext.Category.AddAsync(food);

                var dairyProducts = new Category
                {
                    ParentCategory = food,
                    Name = "Dairy products"
                };
                await dataContext.Category.AddAsync(dairyProducts);

                var yogurt = new Category
                {
                    ParentCategory = dairyProducts,
                    Name = "Yogurt"
                };
                await dataContext.Category.AddAsync(yogurt);

                var butter = new Category
                {
                    ParentCategory = dairyProducts,
                    Name = "Butter"
                };
                await dataContext.Category.AddAsync(butter);

                var milk = new Category
                {
                    ParentCategory = dairyProducts,
                    Name = "Milk"
                };
                await dataContext.Category.AddAsync(milk);


                /* ###############################   PRODUCTS  ############################### */


                /* ###### ZUZU 3.5 L ###### */
                var attributesMilk35Zuzu = new ArrayList();

                attributesMilk35Zuzu.Add(new MyField { Name = "kJ per 100g or 100ml ", Value = "257", InfoCategory = "Nutritional Information" });
                attributesMilk35Zuzu.Add(new MyField { Name = "kcal per 100g or 100ml ", Value = "4.7g", InfoCategory = "Nutritional Information" });

                attributesMilk35Zuzu.Add(new MyField { Name = "Fats ", Value = "3.5g", InfoCategory = "Nutritional Information" });
                attributesMilk35Zuzu.Add(new MyField { Name = "Fatty acids ", Value = "2.4g", InfoCategory = "Nutritional Information" });

                attributesMilk35Zuzu.Add(new MyField { Name = "Carbohydrates ", Value = "4.5g", InfoCategory = "Nutritional Information" });
                attributesMilk35Zuzu.Add(new MyField { Name = "Sugars", Value = "4.5g", InfoCategory = "Nutritional Information" });
                attributesMilk35Zuzu.Add(new MyField { Name = "Salt", Value = "0.1g", InfoCategory = "Nutritional Information" });
                attributesMilk35Zuzu.Add(new MyField { Name = "Proteins", Value = "3g", InfoCategory = "Nutritional Information" });
                attributesMilk35Zuzu.Add(new MyField { Name = "Fat percentage", Value = "3.5%", InfoCategory = "Nutritional Information" });

                attributesMilk35Zuzu.Add(new MyField { Name = "Storage Conditions", Value = "To be kept at 2-4C degree", InfoCategory = "Use" });
                attributesMilk35Zuzu.Add(new MyField { Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use" });


                attributesMilk35Zuzu.Add(new MyField { Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens" });


                var zuzuMilk35 = new Product
                {
                    Title = "Milk Zuzu 1.8L, 3.5% fat",
                    ShortTitle = "Milk",
                    Barcode = "123",
                    Brand = zuzuBrand,
                    Category = milk,
                    PathToImage = "https://i.ibb.co/4RdsJ2q/zuzu-lapte3-5.png",
                    Score = 4.53,
                    BasePrice = 10,
                    Discount = 12,
                    UnitsAvailable = 4,
                    Availability = DateTime.Today.AddDays(5),
                    Attributes = JsonConvert.SerializeObject(attributesMilk35Zuzu)
                };
                await dataContext.Products.AddAsync(zuzuMilk35);


                /* ###### ZUZU 1 L ###### */
                var attributesMilk1LZuzu = new ArrayList();
                attributesMilk1LZuzu.Add(new MyField { Name = "kJ per 100g or 100ml ", Value = "257", InfoCategory = "Nutritional Information" });
                attributesMilk1LZuzu.Add(new MyField { Name = "kcal per 100g or 100ml ", Value = "4.7g", InfoCategory = "Nutritional Information" });

                attributesMilk1LZuzu.Add(new MyField { Name = "Fats ", Value = "3.5g", InfoCategory = "Nutritional Information" });
                attributesMilk1LZuzu.Add(new MyField { Name = "Fatty acids ", Value = "2.4g", InfoCategory = "Nutritional Information" });

                attributesMilk1LZuzu.Add(new MyField { Name = "Carbohydrates ", Value = "4.5g", InfoCategory = "Nutritional Information" });
                attributesMilk1LZuzu.Add(new MyField { Name = "Sugars", Value = "4.5g", InfoCategory = "Nutritional Information" });
                attributesMilk1LZuzu.Add(new MyField { Name = "Salt", Value = "0.1g", InfoCategory = "Nutritional Information" });
                attributesMilk1LZuzu.Add(new MyField { Name = "Proteins", Value = "3g", InfoCategory = "Nutritional Information" });
                attributesMilk1LZuzu.Add(new MyField { Name = "Fat percentage", Value = "3.5%", InfoCategory = "Nutritional Information" });

                attributesMilk1LZuzu.Add(new MyField { Name = "Storage Conditions", Value = "To be kept at 2-4C degree", InfoCategory = "Use" });
                attributesMilk1LZuzu.Add(new MyField { Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use" });


                attributesMilk1LZuzu.Add(new MyField { Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens" });

                var zuzuMilk1L = new Product
                {
                    Title = "Milk Zuzu 1L, 3.5% fat",
                    ShortTitle = "Milk",
                    Barcode = "1234",
                    Brand = zuzuBrand,
                    Category = milk,
                    PathToImage = "https://i.ibb.co/3sTwBhb/zuzu-lapte.png",
                    Score = 4.68,
                    BasePrice = 5,
                    Discount = 5,
                    UnitsAvailable = 30,
                    Availability = DateTime.Today.AddDays(5),
                    Attributes = JsonConvert.SerializeObject(attributesMilk1LZuzu)
                };
                await dataContext.Products.AddAsync(zuzuMilk1L);



                /* ###### Fulga 1 L ###### */
                var attributesMilkFulga = new ArrayList();
                attributesMilkFulga.Add(new MyField { Name = "kJ per 100g or 100ml ", Value = "181", InfoCategory = "Nutritional Information" });
                attributesMilkFulga.Add(new MyField { Name = "kcal per 100g or 100ml ", Value = "44.3g", InfoCategory = "Nutritional Information" });

                attributesMilkFulga.Add(new MyField { Name = "Fats ", Value = "1.5g", InfoCategory = "Nutritional Information" });
                attributesMilkFulga.Add(new MyField { Name = "Fatty acids ", Value = "1g", InfoCategory = "Nutritional Information" });

                attributesMilkFulga.Add(new MyField { Name = "Carbohydrates ", Value = "4.5g", InfoCategory = "Nutritional Information" });
                attributesMilkFulga.Add(new MyField { Name = "Sugars", Value = "4.5g", InfoCategory = "Nutritional Information" });
                attributesMilkFulga.Add(new MyField { Name = "Salt", Value = "0.1g", InfoCategory = "Nutritional Information" });
                attributesMilkFulga.Add(new MyField { Name = "Proteins", Value = "3.1g", InfoCategory = "Nutritional Information" });
                attributesMilkFulga.Add(new MyField { Name = "Fat percentage", Value = "3.5%", InfoCategory = "Nutritional Information" });

                attributesMilkFulga.Add(new MyField { Name = "Storage Conditions", Value = "To be kept at 2-4C degree", InfoCategory = "Use" });
                attributesMilkFulga.Add(new MyField { Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use" });

                attributesMilkFulga.Add(new MyField { Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens" });

                var fulgaMilk1L = new Product
                {
                    Title = "Milk Fulga 1L, 1.5% fat",
                    ShortTitle = "Milk",
                    Barcode = "12345",
                    Brand = fulgaBrand,
                    Category = milk,
                    PathToImage = "https://i.ibb.co/DzrBHPn/lapte-de-consum-fulga-uht-1-l-8877631897630.png",
                    Score = 3.57,
                    BasePrice = 5,
                    Discount = 8,
                    UnitsAvailable = 13,
                    Availability = DateTime.Today.AddDays(4),
                    Attributes = JsonConvert.SerializeObject(attributesMilkFulga)
                };
                await dataContext.Products.AddAsync(fulgaMilk1L);


                /* ###### Covalact 1 L ###### */
                var attributesMilkCovalact = new ArrayList();
                attributesMilkCovalact.Add(new MyField { Name = "kJ per 100g or 100ml ", Value = "257", InfoCategory = "Nutritional Information" });
                attributesMilkCovalact.Add(new MyField { Name = "kcal per 100g or 100ml ", Value = "62g", InfoCategory = "Nutritional Information" });

                attributesMilkCovalact.Add(new MyField { Name = "Fats ", Value = "3.5g", InfoCategory = "Nutritional Information" });
                attributesMilkCovalact.Add(new MyField { Name = "Fatty acids ", Value = "2.4g", InfoCategory = "Nutritional Information" });

                attributesMilkCovalact.Add(new MyField { Name = "Carbohydrates ", Value = "4.5g", InfoCategory = "Nutritional Information" });
                attributesMilkCovalact.Add(new MyField { Name = "Sugars", Value = "4.5g", InfoCategory = "Nutritional Information" });
                attributesMilkCovalact.Add(new MyField { Name = "Proteins", Value = "3g", InfoCategory = "Nutritional Information" });

                attributesMilkCovalact.Add(new MyField { Name = "Fat percentage", Value = "3.5%", InfoCategory = "Nutritional Information" });

                attributesMilkCovalact.Add(new MyField { Name = "Storage Conditions", Value = "To be kept at 2-6C degree", InfoCategory = "Use" });
                attributesMilkCovalact.Add(new MyField { Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use" });

                attributesMilkFulga.Add(new MyField { Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens" });

                var covalactMilk1L = new Product
                {
                    Title = "Milk Covalact 1L, 1.5% fat",
                    ShortTitle = "Milk",
                    Barcode = "12355",
                    Brand = covalactBrand,
                    Category = milk,
                    PathToImage = "https://i.ibb.co/JqVKVWw/lapte-consum-covalact-1-l-cutie-8868827234334.png",
                    Score = 4.81,
                    BasePrice = 5,
                    Discount = 3,
                    UnitsAvailable = 8,
                    Availability = DateTime.Today.AddDays(4),
                    Attributes = JsonConvert.SerializeObject(attributesMilkFulga)
                };
                await dataContext.Products.AddAsync(covalactMilk1L);

                await dataContext.SaveChangesAsync();
            }
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

                await userManager.CreateAsync(defaultBaseUser, password);

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


        public class MyField
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public string InfoCategory { get; set; }
        }
    }
}
