﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopApi.Models;

namespace ShopApi.Data
{
    public class DataContext : IdentityDbContext<IdentityUser>
    {
        public override DbSet<IdentityUser> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BasicUser>();
        }
    }
}
