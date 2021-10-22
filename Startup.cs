using ECommerce1.Data;
using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECommerce1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=StoreFront}/{action=Index}/{id?}");
            });

            // Seed Database
            AppDBInitializer.Seed(app);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // DBContext configuration
            services.AddDbContext<AppDBContext>(context =>
                context.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            // Services Configuration
            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IPromotionsService, PromotionsService>();

            services.AddControllersWithViews();
        }
    }
}