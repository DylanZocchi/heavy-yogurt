using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalApp.Data;
using FinalApp.Models;
using FinalApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinalApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddAuthentication().AddMicrosoftAccount(opts =>
            {
                var settings = Configuration.GetSection("Microsoft").Get<BaseProject.MicrosoftSettings>();
                opts.ClientId = settings.Key;
                opts.ClientSecret = settings.Secret;
                opts.CallbackPath = "/signin-microsoft";
            });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<AuthMessageSenderOptions>(Configuration);
            var sqlOptions = Configuration.Get<SqlServerOptions>();            
            var UserInfo = Configuration.GetConnectionString("FinalApp");           
            var UserFormat = String.Format(UserInfo, sqlOptions.UserId, sqlOptions.Password);           
            services.AddDbContext<ApplicationContext>(opts => opts.UseSqlServer(UserFormat));
            services.AddSingleton<IEmailSender, EmailSender>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider services)
        {
            EnsureDatabaseUpdated(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Product}/{action=Index}/{id?}");
            });
            CreateUserRoles(services).Wait();
        }

        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityResult roleResult;
            //Add Admin role if it doesn't exist
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck)
            {
                //Create Admin role and seed to the database
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin"));
            }
            //Assign Admin role to one User 
            IdentityUser user = await UserManager.FindByEmailAsync("pbslavin@gmail.com");
            if (user != null)
                await UserManager.AddToRoleAsync(user, "Admin");

            IdentityUser user1 = await UserManager.FindByEmailAsync("dxocchi@gmail.com");
            if (user1 != null)
                await UserManager.AddToRoleAsync(user1, "Admin");
        }

        private void EnsureDatabaseUpdated(IApplicationBuilder app)
        {
            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationContext>())
                {
                    context.Database.Migrate();
                }

                using (var context = serviceScope.ServiceProvider.GetService<MyIdentityContext>())
                {
                    context.Database.Migrate();
                }

            }

        }
    }
}
