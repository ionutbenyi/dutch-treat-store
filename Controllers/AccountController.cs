using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signManager;

        public AccountController(ILogger<AccountController> logger, SignInManager<StoreUser> signManager)
        {
            _logger = logger;
            _signManager = signManager;
        }

        public IActionResult Login()
        {
            // User is in the Controller (base class prop)
            // if they are already logged in
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index","App"); // figure url for Index action on App controller
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signManager.PasswordSignInAsync(model.Username, model.Password,
                    model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["returnUrl"].First());
                    }
                    else
                    {
                        return RedirectToAction("Shop", "App");
                    }
                }
            }
            ModelState.AddModelError("", "Failed to login");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout(LoginViewModel model)
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }
    }
}
