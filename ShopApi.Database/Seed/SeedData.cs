using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShopApi.Database.Data;
using ShopApi.Database.Entities;
using ShopApi.Database.Entities.ProductManagement;

namespace ShopApi.Database.Seed
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            var dataContext = services.GetService<DataContext>();

            var logger = services.GetRequiredService<ILogger<DataContext>>();

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

                var olympusBrand = new Brand
                {
                    Email = "olympusBrand @yahoo.com",
                    Address = "Sectorul 5",
                    City = "Bucuresti",
                    Country = "Romania",
                    Name = "Olympus",
                    Phone = "0744584911"
                };
                var napolactBrand = new Brand
                {
                    Email = "Napolact@yahoo.com",
                    Address = "Sectorul 5",
                    City = "Bucuresti",
                    Country = "Romania",
                    Name = "Napolact",
                    Phone = "0744584911"
                };

                var mullerBrand = new Brand
                {
                    Email = "Muller@yahoo.com",
                    Address = "Sectorul 5",
                    City = "Bucuresti",
                    Country = "Romania",
                    Name = "Muller",
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
                await dataContext.Categories.AddAsync(food);

                var dairyProducts = new Category
                {
                    ParentCategory = food,
                    Name = "Dairy products"
                };
                await dataContext.Categories.AddAsync(dairyProducts);

                var yogurt = new Category
                {
                    ParentCategory = dairyProducts,
                    Name = "Yogurt"
                };
                await dataContext.Categories.AddAsync(yogurt);

                var butter = new Category
                {
                    ParentCategory = dairyProducts,
                    Name = "Butter"
                };
                await dataContext.Categories.AddAsync(butter);

                var milk = new Category
                {
                    ParentCategory = dairyProducts,
                    Name = "Milk"
                };
                await dataContext.Categories.AddAsync(milk);


                /* ###############################   PRODUCTS  ############################### */


                /* ###### ZUZU 3.5 L ###### */
                var attributesMilk35Zuzu = new ArrayList();

                attributesMilk35Zuzu.Add(new MyField
                    {Name = "kJ per 100g or 100ml ", Value = "257", InfoCategory = "Nutritional Information"});
                attributesMilk35Zuzu.Add(new MyField
                    {Name = "kcal per 100g or 100ml ", Value = "4.7g", InfoCategory = "Nutritional Information"});

                attributesMilk35Zuzu.Add(new MyField
                    {Name = "Fats ", Value = "3.5g", InfoCategory = "Nutritional Information"});
                attributesMilk35Zuzu.Add(new MyField
                    {Name = "Fatty acids ", Value = "2.4g", InfoCategory = "Nutritional Information"});

                attributesMilk35Zuzu.Add(new MyField
                    {Name = "Carbohydrates ", Value = "4.5g", InfoCategory = "Nutritional Information"});
                attributesMilk35Zuzu.Add(new MyField
                    {Name = "Sugars", Value = "4.5g", InfoCategory = "Nutritional Information"});
                attributesMilk35Zuzu.Add(new MyField
                    {Name = "Salt", Value = "0.1g", InfoCategory = "Nutritional Information"});
                attributesMilk35Zuzu.Add(new MyField
                    {Name = "Proteins", Value = "3g", InfoCategory = "Nutritional Information"});
                attributesMilk35Zuzu.Add(new MyField
                    {Name = "Fat percentage", Value = "3.5%", InfoCategory = "Nutritional Information"});

                attributesMilk35Zuzu.Add(new MyField
                    {Name = "Storage Conditions", Value = "To be kept at 2-4C degree", InfoCategory = "Use"});
                attributesMilk35Zuzu.Add(new MyField
                    {Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use"});


                attributesMilk35Zuzu.Add(new MyField
                    {Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens"});


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
                    Attributes = JsonConvert.SerializeObject(attributesMilk35Zuzu)
                };
                await dataContext.Products.AddAsync(zuzuMilk35);


                /* ###### ZUZU 1 L ###### */
                var attributesMilk1LZuzu = new ArrayList();
                attributesMilk1LZuzu.Add(new MyField
                    {Name = "kJ per 100g or 100ml ", Value = "257", InfoCategory = "Nutritional Information"});
                attributesMilk1LZuzu.Add(new MyField
                    {Name = "kcal per 100g or 100ml ", Value = "4.7g", InfoCategory = "Nutritional Information"});

                attributesMilk1LZuzu.Add(new MyField
                    {Name = "Fats ", Value = "3.5g", InfoCategory = "Nutritional Information"});
                attributesMilk1LZuzu.Add(new MyField
                    {Name = "Fatty acids ", Value = "2.4g", InfoCategory = "Nutritional Information"});

                attributesMilk1LZuzu.Add(new MyField
                    {Name = "Carbohydrates ", Value = "4.5g", InfoCategory = "Nutritional Information"});
                attributesMilk1LZuzu.Add(new MyField
                    {Name = "Sugars", Value = "4.5g", InfoCategory = "Nutritional Information"});
                attributesMilk1LZuzu.Add(new MyField
                    {Name = "Salt", Value = "0.1g", InfoCategory = "Nutritional Information"});
                attributesMilk1LZuzu.Add(new MyField
                    {Name = "Proteins", Value = "3g", InfoCategory = "Nutritional Information"});
                attributesMilk1LZuzu.Add(new MyField
                    {Name = "Fat percentage", Value = "3.5%", InfoCategory = "Nutritional Information"});

                attributesMilk1LZuzu.Add(new MyField
                    {Name = "Storage Conditions", Value = "To be kept at 2-4C degree", InfoCategory = "Use"});
                attributesMilk1LZuzu.Add(new MyField
                    {Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use"});


                attributesMilk1LZuzu.Add(new MyField
                    {Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens"});

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
                    Attributes = JsonConvert.SerializeObject(attributesMilk1LZuzu)
                };
                await dataContext.Products.AddAsync(zuzuMilk1L);



                /* ###### Fulga 1 L ###### */
                var attributesMilkFulga = new ArrayList();
                attributesMilkFulga.Add(new MyField
                    {Name = "kJ per 100g or 100ml ", Value = "181", InfoCategory = "Nutritional Information"});
                attributesMilkFulga.Add(new MyField
                    {Name = "kcal per 100g or 100ml ", Value = "44.3g", InfoCategory = "Nutritional Information"});

                attributesMilkFulga.Add(new MyField
                    {Name = "Fats ", Value = "1.5g", InfoCategory = "Nutritional Information"});
                attributesMilkFulga.Add(new MyField
                    {Name = "Fatty acids ", Value = "1g", InfoCategory = "Nutritional Information"});

                attributesMilkFulga.Add(new MyField
                    {Name = "Carbohydrates ", Value = "4.5g", InfoCategory = "Nutritional Information"});
                attributesMilkFulga.Add(new MyField
                    {Name = "Sugars", Value = "4.5g", InfoCategory = "Nutritional Information"});
                attributesMilkFulga.Add(new MyField
                    {Name = "Salt", Value = "0.1g", InfoCategory = "Nutritional Information"});
                attributesMilkFulga.Add(new MyField
                    {Name = "Proteins", Value = "3.1g", InfoCategory = "Nutritional Information"});
                attributesMilkFulga.Add(new MyField
                    {Name = "Fat percentage", Value = "3.5%", InfoCategory = "Nutritional Information"});

                attributesMilkFulga.Add(new MyField
                    {Name = "Storage Conditions", Value = "To be kept at 2-4C degree", InfoCategory = "Use"});
                attributesMilkFulga.Add(new MyField
                    {Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use"});

                attributesMilkFulga.Add(new MyField
                    {Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens"});

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
                    Attributes = JsonConvert.SerializeObject(attributesMilkFulga)
                };
                await dataContext.Products.AddAsync(fulgaMilk1L);


                /* ###### Covalact 1 L ###### */
                var attributesMilkCovalact = new ArrayList();
                attributesMilkCovalact.Add(new MyField
                    {Name = "kJ per 100g or 100ml ", Value = "257", InfoCategory = "Nutritional Information"});
                attributesMilkCovalact.Add(new MyField
                    {Name = "kcal per 100g or 100ml ", Value = "62g", InfoCategory = "Nutritional Information"});

                attributesMilkCovalact.Add(new MyField
                    {Name = "Fats ", Value = "3.5g", InfoCategory = "Nutritional Information"});
                attributesMilkCovalact.Add(new MyField
                    {Name = "Fatty acids ", Value = "2.4g", InfoCategory = "Nutritional Information"});

                attributesMilkCovalact.Add(new MyField
                    {Name = "Carbohydrates ", Value = "4.5g", InfoCategory = "Nutritional Information"});
                attributesMilkCovalact.Add(new MyField
                    {Name = "Sugars", Value = "4.5g", InfoCategory = "Nutritional Information"});
                attributesMilkCovalact.Add(new MyField
                    {Name = "Proteins", Value = "3g", InfoCategory = "Nutritional Information"});

                attributesMilkCovalact.Add(new MyField
                    {Name = "Fat percentage", Value = "3.5%", InfoCategory = "Nutritional Information"});

                attributesMilkCovalact.Add(new MyField
                    {Name = "Storage Conditions", Value = "To be kept at 2-6C degree", InfoCategory = "Use"});
                attributesMilkCovalact.Add(new MyField
                    {Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use"});

                attributesMilkFulga.Add(new MyField
                    {Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens"});

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
                    Attributes = JsonConvert.SerializeObject(attributesMilkFulga)
                };
                await dataContext.Products.AddAsync(covalactMilk1L);
          

            /* ###### olympus yogurt ###### */
                var attributesYogurtGreek = new ArrayList();
                attributesYogurtGreek.Add(new MyField { Name = "kJ per 100g or 100ml ", Value = "275", InfoCategory = "Nutritional Information" });
                attributesYogurtGreek.Add(new MyField { Name = "kcal per 100g or 100ml ", Value = "65", InfoCategory = "Nutritional Information" });

                attributesYogurtGreek.Add(new MyField { Name = "Fats ", Value = "2g", InfoCategory = "Nutritional Information" });
                attributesYogurtGreek.Add(new MyField { Name = "Fatty acids ", Value = "1.2g", InfoCategory = "Nutritional Information" });

                attributesYogurtGreek.Add(new MyField { Name = "Carbohydrates ", Value = "3.8g", InfoCategory = "Nutritional Information" });
                attributesYogurtGreek.Add(new MyField { Name = "Sugars", Value = "3.8g", InfoCategory = "Nutritional Information" });
                attributesYogurtGreek.Add(new MyField { Name = "Salt", Value = "0.2g", InfoCategory = "Nutritional Information" });
                attributesYogurtGreek.Add(new MyField { Name = "Proteins", Value = "8g", InfoCategory = "Nutritional Information" });


                attributesYogurtGreek.Add(new MyField { Name = "Storage Conditions", Value = "To be kept at 2-6C degree", InfoCategory = "Use" });
                attributesYogurtGreek.Add(new MyField { Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use" });

                attributesYogurtGreek.Add(new MyField { Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens" });

                attributesYogurtGreek.Add(new MyField { Name = "Weight", Value = "150g", InfoCategory = "Dimension" });

                attributesYogurtGreek.Add(new MyField { Name = "Country", Value = "Romania", InfoCategory = "Origin Country" });

                var olympysYogortGreek = new Product
                {
                    Title = "Greek yogurt 150g",
                    ShortTitle = "Milk",
                    Barcode = "123451",
                    Brand = olympusBrand,
                    Category = yogurt,
                    PathToImage = "https://i.ibb.co/zSZJJTb/iaurt-grecesc.png",
                    Score = 4.78,
                    BasePrice = 2,
                    Discount = 5,
                    UnitsAvailable = 23,
                    Attributes = JsonConvert.SerializeObject(attributesYogurtGreek)
                };
                await dataContext.Products.AddAsync(olympysYogortGreek);


                /* ######  Napolact Yogurt 900g ###### */
                var attributesNapolactYogurt900 = new ArrayList();
                attributesNapolactYogurt900.Add(new MyField { Name = "kJ per 100g or 100ml ", Value = "250", InfoCategory = "Nutritional Information" });
                attributesNapolactYogurt900.Add(new MyField { Name = "kcal per 100g or 100ml ", Value = "60", InfoCategory = "Nutritional Information" });

                attributesNapolactYogurt900.Add(new MyField { Name = "Fats ", Value = "3.5g", InfoCategory = "Nutritional Information" });
                attributesNapolactYogurt900.Add(new MyField { Name = "Fatty acids ", Value = "2.1g", InfoCategory = "Nutritional Information" });

                attributesNapolactYogurt900.Add(new MyField { Name = "Carbohydrates ", Value = "3.7g", InfoCategory = "Nutritional Information" });
                attributesNapolactYogurt900.Add(new MyField { Name = "Sugars", Value = "3.7g", InfoCategory = "Nutritional Information" });
                attributesNapolactYogurt900.Add(new MyField { Name = "Salt", Value = "0.06g", InfoCategory = "Nutritional Information" });
                attributesNapolactYogurt900.Add(new MyField { Name = "Proteins", Value = "3.4g", InfoCategory = "Nutritional Information" });


                attributesNapolactYogurt900.Add(new MyField { Name = "Storage Conditions", Value = "To be kept at 2-6C degree", InfoCategory = "Use" });
                attributesNapolactYogurt900.Add(new MyField { Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use" });

                attributesNapolactYogurt900.Add(new MyField { Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens" });

                attributesNapolactYogurt900.Add(new MyField { Name = "Weight", Value = "900g", InfoCategory = "Dimension" });

                attributesNapolactYogurt900.Add(new MyField { Name = "Country", Value = "Romania", InfoCategory = "Origin Country" });

                var NapolactYogurt = new Product
                {
                    Title = "Napolact yogurt 900g",
                    ShortTitle = "Milk",
                    Barcode = "123452",
                    Brand = napolactBrand,
                    Category = yogurt,
                    PathToImage = "https://i.ibb.co/chFND6W/iaurt-napolact-numa-bun-900-g-8950854713374.png",
                    Score = 4.78,
                    BasePrice = 7,
                    Discount = 4,
                    UnitsAvailable = 23,
                    Attributes = JsonConvert.SerializeObject(attributesNapolactYogurt900)
                };
                await dataContext.Products.AddAsync(NapolactYogurt);


                /* ######  Napolact Yogurt 140g  ###### */
                var attributesNapolactYogurt140g = new ArrayList();
                attributesNapolactYogurt140g.Add(new MyField { Name = "kJ per 100g or 100ml ", Value = "261", InfoCategory = "Nutritional Information" });
                attributesNapolactYogurt140g.Add(new MyField { Name = "kcal per 100g or 100ml ", Value = "63", InfoCategory = "Nutritional Information" });

                attributesNapolactYogurt140g.Add(new MyField { Name = "Fats ", Value = "3.8g", InfoCategory = "Nutritional Information" });
                attributesNapolactYogurt140g.Add(new MyField { Name = "Fatty acids ", Value = "2.3g", InfoCategory = "Nutritional Information" });

                attributesNapolactYogurt140g.Add(new MyField { Name = "Carbohydrates ", Value = "3.7g", InfoCategory = "Nutritional Information" });
                attributesNapolactYogurt140g.Add(new MyField { Name = "Sugars", Value = "3.7g", InfoCategory = "Nutritional Information" });
                attributesNapolactYogurt140g.Add(new MyField { Name = "Salt", Value = "0.06g", InfoCategory = "Nutritional Information" });
                attributesNapolactYogurt140g.Add(new MyField { Name = "Proteins", Value = "3.4g", InfoCategory = "Nutritional Information" });

                attributesNapolactYogurt140g.Add(new MyField { Name = "Storage Conditions", Value = "To be kept at 2-6C degree", InfoCategory = "Use" });
                attributesNapolactYogurt140g.Add(new MyField { Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use" });

                attributesNapolactYogurt140g.Add(new MyField { Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens" });

                attributesNapolactYogurt140g.Add(new MyField { Name = "Weight", Value = "900g", InfoCategory = "Dimension" });

                attributesNapolactYogurt140g.Add(new MyField { Name = "Country", Value = "Romania", InfoCategory = "Origin Country" });

                var NapolactYogurt140 = new Product
                {
                    Title = "Napolact yogurt 140g",
                    ShortTitle = "Milk",
                    Barcode = "123453",
                    Brand = covalactBrand,
                    Category = yogurt,
                    PathToImage = "https://i.ibb.co/gTf0fY8/iaurt-napolact-bio-pahar-140-g-8950857367582.png",
                    Score = 4.78,
                    BasePrice = 2.25,
                    Discount = 4,
                    UnitsAvailable = 17,
                    Attributes = JsonConvert.SerializeObject(attributesNapolactYogurt140g)
                };
                await dataContext.Products.AddAsync(NapolactYogurt140);



                /* ######  Covalact Yogurt 900g  ###### */
                var attributesCovalactYogurt900g = new ArrayList();
                attributesCovalactYogurt900g.Add(new MyField { Name = "kJ per 100g or 100ml ", Value = "260", InfoCategory = "Nutritional Information" });
                attributesCovalactYogurt900g.Add(new MyField { Name = "kcal per 100g or 100ml ", Value = "62", InfoCategory = "Nutritional Information" });

                attributesCovalactYogurt900g.Add(new MyField { Name = "Fats ", Value = "2.8g", InfoCategory = "Nutritional Information" });
                attributesCovalactYogurt900g.Add(new MyField { Name = "Fatty acids ", Value = "1.7g", InfoCategory = "Nutritional Information" });

                attributesCovalactYogurt900g.Add(new MyField { Name = "Carbohydrates ", Value = "3.8g", InfoCategory = "Nutritional Information" });
                attributesCovalactYogurt900g.Add(new MyField { Name = "Sugars", Value = "3.8g", InfoCategory = "Nutritional Information" });
                attributesCovalactYogurt900g.Add(new MyField { Name = "Salt", Value = "0.1g", InfoCategory = "Nutritional Information" });
                attributesCovalactYogurt900g.Add(new MyField { Name = "Proteins", Value = "3.2g", InfoCategory = "Nutritional Information" });

                attributesCovalactYogurt900g.Add(new MyField { Name = "Storage Conditions", Value = "To be kept at 2-6C degree", InfoCategory = "Use" });
                attributesCovalactYogurt900g.Add(new MyField { Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use" });

                attributesCovalactYogurt900g.Add(new MyField { Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens" });

                attributesCovalactYogurt900g.Add(new MyField { Name = "Weight", Value = "900g", InfoCategory = "Dimension" });

                attributesCovalactYogurt900g.Add(new MyField { Name = "Country", Value = "Romania", InfoCategory = "Origin Country" });

                var covalactYogurt900 = new Product
                {
                    Title = "Covalact yogurt 900g",
                    ShortTitle = "Milk",
                    Barcode = "123454",
                    Brand = napolactBrand,
                    Category = yogurt,
                    PathToImage = "https://i.ibb.co/ctB08Cw/iaurt-ecologic-covalact-38-grasime-140g-8876676218910.png",
                    Score = 2,
                    BasePrice = 8,
                    Discount = 3,
                    UnitsAvailable = 4,
                    Attributes = JsonConvert.SerializeObject(attributesCovalactYogurt900g)
                };
                await dataContext.Products.AddAsync(covalactYogurt900);

                /* ######  Covalact Yogurt 140g  ###### */
                var attributesCovalactYogurt140g = new ArrayList();
                attributesCovalactYogurt140g.Add(new MyField { Name = "kJ per 100g or 100ml ", Value = "233", InfoCategory = "Nutritional Information" });
                attributesCovalactYogurt140g.Add(new MyField { Name = "kcal per 100g or 100ml ", Value = "53", InfoCategory = "Nutritional Information" });

                attributesCovalactYogurt140g.Add(new MyField { Name = "Fats ", Value = "2.8g", InfoCategory = "Nutritional Information" });
                attributesCovalactYogurt140g.Add(new MyField { Name = "Fatty acids ", Value = "1.7g", InfoCategory = "Nutritional Information" });

                attributesCovalactYogurt140g.Add(new MyField { Name = "Carbohydrates ", Value = "3.8g", InfoCategory = "Nutritional Information" });
                attributesCovalactYogurt140g.Add(new MyField { Name = "Sugars", Value = "3.8g", InfoCategory = "Nutritional Information" });
                attributesCovalactYogurt140g.Add(new MyField { Name = "Salt", Value = "0.1g", InfoCategory = "Nutritional Information" });
                attributesCovalactYogurt140g.Add(new MyField { Name = "Proteins", Value = "3.2g", InfoCategory = "Nutritional Information" });

                attributesCovalactYogurt140g.Add(new MyField { Name = "Storage Conditions", Value = "To be kept at 2-6C degree", InfoCategory = "Use" });
                attributesCovalactYogurt140g.Add(new MyField { Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use" });

                attributesCovalactYogurt140g.Add(new MyField { Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens" });

                attributesCovalactYogurt140g.Add(new MyField { Name = "Weight", Value = "140g", InfoCategory = "Dimension" });

                attributesCovalactYogurt140g.Add(new MyField { Name = "Country", Value = "Romania", InfoCategory = "Origin Country" });

                var covalactYogurt140 = new Product
                {
                    Title = "Covalact yogurt 140g",
                    ShortTitle = "Milk",
                    Barcode = "123455",
                    Brand = napolactBrand,
                    Category = yogurt,
                    PathToImage = "https://i.ibb.co/kxGVQpx/iaurt-covalact-900-g-8886763946014.png",
                    Score = 4.78,
                    BasePrice = 7,
                    Discount = 3,
                    UnitsAvailable = 17,
                    Attributes = JsonConvert.SerializeObject(attributesNapolactYogurt140g)
                };
                await dataContext.Products.AddAsync(covalactYogurt140);



                /* ######  Muller Yogurt  ###### */
                var attributesMullerYogurt= new ArrayList();
                attributesMullerYogurt.Add(new MyField { Name = "kJ per 100g or 100ml ", Value = "427", InfoCategory = "Nutritional Information" });
                attributesMullerYogurt.Add(new MyField { Name = "kcal per 100g or 100ml ", Value = "101", InfoCategory = "Nutritional Information" });

                attributesMullerYogurt.Add(new MyField { Name = "Fats ", Value = "3.2g", InfoCategory = "Nutritional Information" });
                attributesMullerYogurt.Add(new MyField { Name = "Fatty acids ", Value = "2.2g", InfoCategory = "Nutritional Information" });

                attributesMullerYogurt.Add(new MyField { Name = "Carbohydrates ", Value = "14g", InfoCategory = "Nutritional Information" });
                attributesMullerYogurt.Add(new MyField { Name = "Sugars", Value = "12.9g", InfoCategory = "Nutritional Information" });
                attributesMullerYogurt.Add(new MyField { Name = "Proteins", Value = "3.3g", InfoCategory = "Nutritional Information" });

                attributesMullerYogurt.Add(new MyField { Name = "Storage Conditions", Value = "To be kept at 2-6C degree", InfoCategory = "Use" });
                attributesMullerYogurt.Add(new MyField { Name = "Method of preparation", Value = "Pasteurized", InfoCategory = "Use" });

                attributesMullerYogurt.Add(new MyField { Name = "Allergen", Value = "Lactose", InfoCategory = "Food allergens" });

                attributesMullerYogurt.Add(new MyField { Name = "Weight", Value = "140g", InfoCategory = "Dimension" });

                attributesMullerYogurt.Add(new MyField { Name = "Country", Value = "Germany", InfoCategory = "Origin Country" });

                var mullerYogurt = new Product
                {
                    Title = "Muller  yogurt 500g",
                    ShortTitle = "Milk",
                    Barcode = "123456",
                    Brand = mullerBrand,
                    Category = yogurt,
                    PathToImage = "https://i.ibb.co/G3pzXHh/iaurt-cu-cirese-muller-pezzi-500g-8850666913822.png",
                    Score = 4.78,
                    BasePrice = 7,
                    Discount = 3,
                    UnitsAvailable = 17,
                    Attributes = JsonConvert.SerializeObject(attributesMullerYogurt)
                };
                await dataContext.Products.AddAsync(mullerYogurt);
                await dataContext.SaveChangesAsync();

            }

        }

        private static async Task AddDefaultUser(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, ILogger<DataContext> logger)
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
