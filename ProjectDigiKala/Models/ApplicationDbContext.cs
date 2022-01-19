using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectDigiKala.Models.Carts;
using ProjectDigiKala.Models.Orders;
using ProjectDigiKala.Models.Products;
using ProjectDigiKala.Models.Profile;
using ProjectDigiKala.Models.Tags;

namespace ProjectDigiKala.Models
{
    public class ApplicationDbContext : IdentityDbContext<Operator>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<KeyPoint> keyPoints { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagValue> TagValues { get; set; }
        public DbSet<SpecificationGroup> SpecificationGroups { get; set; }
        public DbSet<Specification> Specifications { get; set; }
        public DbSet<SpecificationValue> SpecificationValues { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<ProductItemTagValue> ProductItemTagValues { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductItemTagValue>().HasKey(p => new { p.ProductItemId, p.TagValueId });

            //modelBuilder.Entity<Specification>()
            //    .HasOne(s => s.SpecificationValue)
            //    .WithOne(s => s.Specification)
            //    .HasForeignKey<SpecificationValue>(s => s.Id);
        }

        public async static Task CreateAdminAccount(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            UserManager<Operator> userManager = serviceProvider.GetRequiredService<UserManager<Operator>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string username = configuration["Data:Admin:Name"];
            string email = configuration["Data:Admin:Email"];
            string password = configuration["Data:Admin:Password"];
            string role = configuration["Data:Admin:Role"];
            if (await userManager.FindByNameAsync(username) == null)
            {
                if (await roleManager.FindByNameAsync(role) == null)
                    await roleManager.CreateAsync(new IdentityRole(role));

                Operator user = new Operator()
                {
                    UserName = username,
                    Email = email
                };

                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
