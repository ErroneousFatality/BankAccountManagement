using Infrastructure.DataAccess.EntityFramework;
using Infrastructure.DataAccess.EntityFramework.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Applications.Website
{
    public class Startup
    {
        // Properties
        public IConfiguration Configuration { get; }

        // Constructors
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EfContext>(
                (options) =>
                {
                    string sqlServerConnectionString = Configuration["DataAccess:SQLServerConnectionString"];
                    options.UseSqlServer(sqlServerConnectionString);
                }
            );
            services.AddDatabaseDeveloperPageExceptionFilter();
            services
                .AddDefaultIdentity<ApplicationUser>(
                    (options) =>
                    {
                        // Username settings
                        options.User.AllowedUserNameCharacters = "0123456789";

                        // Password settings
                        options.Password.RequireDigit = true;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequiredLength = 6;
                        options.Password.RequiredUniqueChars = 0;
                    }
                )
                .AddEntityFrameworkStores<EfContext>();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
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
                endpoints.MapRazorPages();
            });
        }
    }
}
