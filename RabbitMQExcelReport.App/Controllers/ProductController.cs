using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RabbitMQExcelReport.App.Models;
using RabbitMQExcelReport.App.Services;
using Shared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RabbitMQExcelReport.App.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        public ProductController(UserManager<IdentityUser> userManager, AppDbContext context, RabbitMqPublisher rabbitMqPublisher)
        {
            _userManager = userManager;
            _context = context;
            _rabbitMqPublisher = rabbitMqPublisher;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateProductExcel()
        {
            var userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var fileName = $"product-excel-{Guid.NewGuid().ToString().Substring(1, 10)}";
            UserFile userFile = new()
            {
                UserId = user.Id,
                FileName = fileName,
                FileStatus = FileStatus.Creating,
            };
            await _context.UserFiles.AddAsync(userFile);
            await _context.SaveChangesAsync();

            _rabbitMqPublisher.Publish(new CreateExcelMessage
            {
                FileId = userFile.Id,
            });

            //RabbitMQ ya mesaj gönder
            TempData["StartCreatingExcel"] = true;
            return RedirectToAction(nameof(Files));
        }
        public async Task<IActionResult> Files()
        {
            var userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            return View(await _context.UserFiles.Where(p => p.UserId == user.Id).ToListAsync());
        }
    }
}
