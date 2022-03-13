using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQExcelReport.App.Models;
using System.Linq;

namespace RabbitMQExcelReport.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                appDbContext.Database.Migrate();
                if (!appDbContext.Users.Any())
                {
                    userManager.CreateAsync(new IdentityUser() { UserName = "hasanfurkanfidan", Email = "furkanfidan.job@gmail.com" }, "Password12*").Wait();
                    userManager.CreateAsync(new IdentityUser() { UserName = "deneme", Email = "deneme@gmail.com" }, "Password12*").Wait();
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
