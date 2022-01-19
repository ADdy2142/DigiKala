using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Data.Services;
using ProjectDigiKala.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using ProjectDigiKala.Models.Orders;

namespace ProjectDigiKala
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            string conStr = Configuration["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(conStr));

            services.AddIdentity<Operator, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IKeyPointRepository, KeyPointRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagValueRepository, TagValueRepository>();
            services.AddScoped<ISpecificationGroupRepository, SpecificationGroupRepository>();
            services.AddScoped<ISpecificationRepository, SpecificationRepository>();
            services.AddScoped<ISpecificationValueRepository, SpecificationValueRepository>();
            services.AddScoped<IProductItemRepository, ProductItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Admin/Account/SignIn";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            // حتما باید قبل از route پروژه تعریف بشه
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });


                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });
            });

            ApplicationDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();
        }
    }
}
