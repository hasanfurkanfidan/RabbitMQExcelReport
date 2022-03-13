using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace RabbitMQExcelReport.App.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hasUser = await _userManager.FindByEmailAsync(email);
            if (hasUser == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı!");
                return View();
            }
            var identityResult = await _signInManager.PasswordSignInAsync(hasUser, password, true, false);
            if (!identityResult.Succeeded)
            {
                ModelState.AddModelError("", "Şifre yanlış!");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
