using G_NET_65_MVC02.BLL.ViewModels.AccountViewModels;
using G_NET_65_MVC02.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace G_NET_65_MVC02_PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager
            ,ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password");
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password,
                model.RememberMe, lockoutOnFailure:true);
            if (result.Succeeded)
            {
                _logger.LogInformation("User {UserId} Signed In", user.Id);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User {UserId} Is Locked Out", user.Id);
                ModelState.AddModelError(string.Empty, "This Account Is Temporarily Locked Try Again Later");
            }
            else if(result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Sign In Not Allowed For This Account");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password");
            }
            return View(model);
        }
    }
}
