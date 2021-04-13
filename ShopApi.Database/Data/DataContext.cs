using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopApi.Database.Entities;
using ShopApi.Database.Entities.ProductManagement;


namespace ShopApi.Database.Data
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public override DbSet<IdentityUser> Users { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<ProductsUsersShoppingCart> ProductsUsersShopping { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<BaseUser>().HasMany<ProductsUsersShoppingCart>(u => u.ProductsUsersShopping)
            //    .WithOne(p => p.User);






        }
    }
}
