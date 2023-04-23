using DataBaseModel;
using DataBaseModel.Entity;
using KursProjectDataBase.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KursProjectDataBase.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KursProjectDataBaseContext? _dataBaseModelContext;
        private readonly AccountService _accountService;

        public AccountController(ILogger<HomeController> logger, KursProjectDataBaseContext context)
        {
            _logger = logger;
            _dataBaseModelContext = context;
            _accountService = new AccountService(context);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(TemporalEntity entity)
        {
            _logger.LogInformation(entity.ToString());
            var result = _accountService.Register(entity!);

            if (result.StatusCode == DataBaseModel.Enum.StatusCode.OK)
            {
                return RedirectToAction("/index", "Home");
            }
            return View(result.Description);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Authorization authorization)
        {
            var result = _accountService.Login(authorization);
            if (result.StatusCode == DataBaseModel.Enum.StatusCode.OK) {

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
                    new ClaimsPrincipal(result.Data!));

                return RedirectToAction("Test", "Home");
            }
            return View(authorization);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
