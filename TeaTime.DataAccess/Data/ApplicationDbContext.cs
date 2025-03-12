using TeaTime.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TeaTime.DataAccess.Data
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "茶飲", DisplayOrder = 1 },
                new Category { Id = 2, Name = "水果茶", DisplayOrder = 2 },
                new Category { Id = 3, Name = "咖啡", DisplayOrder = 3 }
                );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "台灣水果茶", Size = " 大杯", Price = 60, Description = "天然果飲，迷人多變", CategoryId = 1, ImageUrl = "" },
                new Product { Id = 2, Name = "鐵觀音", Size = "中杯", Price = 35, Description = "品鐵觀音，品味人生", CategoryId = 2, ImageUrl = ""},
                new Product { Id = 3, Name = "美式咖啡", Size = "中杯", Price = 50, Description = "減脂提神，美式人生", CategoryId = 3, ImageUrl = ""}
                );
        }
    }
}
