using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using DataAccess.Repositories;
using Presentation.Controllers;
using Domain.Interfaces;
using Presentation;

namespace MattiasTonnaEPSolution
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<PollDbContext>(options =>
                options.UseNpgsql(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();


            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<PollDbContext>();


            int dbSetting = 1;
            try
            {
                dbSetting = builder.Configuration.GetValue<int>("DbSetting");
            }
            catch
            {
                dbSetting = 0;
            }

            switch (dbSetting)
            {
                case 0:
                    builder.Services.AddScoped<IPollRepository, PollRepository>();
                    break;

                case 1:
                    builder.Services.AddScoped<IPollRepository, PollFileRepository>();
                    break;

                default:
                    builder.Services.AddScoped<IPollRepository, PollRepository>();
                    break;
            }

            builder.Services.AddScoped<VoteRepository>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Logging.AddConsole();
            builder.Services.AddScoped<HasNotVotedFilter>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.MapRazorPages();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
