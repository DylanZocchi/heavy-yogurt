using System;
using FinalApp.Models;
using FinalApp.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FinalApp.Areas.Identity.IdentityHostingStartup))]
namespace FinalApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {

            builder.ConfigureServices((context, services) => {
                var sqlOptions = context.Configuration.Get<SqlServerOptions>();
                var UserInfo = context.Configuration.GetConnectionString("FinalApp");
                var UserFormat = String.Format(UserInfo, sqlOptions.UserId, sqlOptions.Password);
                services.AddDbContext<MyIdentityContext>(options =>
                    options.UseSqlServer(UserFormat));
                services.AddIdentity<IdentityUser, IdentityRole>(config =>
                    {
                        config.SignIn.RequireConfirmedEmail = true;
                    })
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<MyIdentityContext>();
            });
        }
    }
}