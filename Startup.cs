using ECommerce1.Data;
using ECommerce1.Data.Services;
using ECommerce1.Data.Services.Interfaces;
using ECommerce1.Helper;
using ECommerce1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=StoreFront}/{action=Home}/{id?}");
            });

            // Seed Database
            AppDBInitializer.Seed(app);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            // DBContext configuration
            services.AddDbContext<AppDBContext>(context =>
                context.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));

            //services.AddIdentity<User, IdentityRole>()
            //.AddEntityFrameworkStores<AppDBContext>()
            //.AddDefaultTokenProviders();

            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
                 .AddRoleManager<RoleManager<IdentityRole>>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AppDBContext>();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // Email Settings Configuration
            services.AddOptions<EmailSettings>().Bind(Configuration.GetSection("EmailSettings")).ValidateDataAnnotations();
            services.AddOptions<AdyenConfig>().Bind(Configuration.GetSection("AdyenConfig")).ValidateDataAnnotations();

            //// SignalR Configuration
            //services.AddSignalR();

            // Services Configuration
            services.AddScoped<IAdministratorService, AdministratorService>();
            services.AddScoped<IAdyenService, AdyenService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICommonServices, CommonServices>();
            services.AddScoped<ICustomersService, CustomersService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IEmployeesService, EmployeesService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductCategoriesService, ProductCategoriesService>();
            services.AddScoped<IProductsService, ProductsService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IPromotionsService, PromotionsService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IRandomPasswordService, RandomPasswordService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRolesService, RolesService>();

            services.AddControllersWithViews();
        }
    }
}