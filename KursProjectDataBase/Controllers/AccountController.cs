using DataBaseModel;
using DataBaseModel.Entity;
using DataBaseModel.ViewEntity;
using KursProjectDataBase.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KursProjectDataBase.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KursProjectDataBaseContext _dataBaseModelContext;
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
        public IActionResult Register(UserView entity)
        {
            _logger.LogInformation(entity.ToString());
            var result = _accountService.Register(entity!);

            if (result.StatusCode == DataBaseModel.Enum.StatusCode.OK)
            {
                return RedirectToAction("Info", "Account");
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

                return RedirectToAction("Index", "Home");
            }
            return View(authorization);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Info", "Account");
        }

        [HttpGet]
        [Authorize (Roles = "Tenant, Renter")]
        public IActionResult Info()
        {
            int temp = int.Parse(HttpContext.User.Identity!.Name!);
            _logger.LogWarning(temp.ToString());

            var user = _dataBaseModelContext.Users.Include(u => u.Tenants).Include(u0=> u0.Renters).FirstOrDefault(u => u.IdU == temp);
            var auth = _dataBaseModelContext.Authorizations.Where(a => a.IdU == temp).ToList();
            var contract = _dataBaseModelContext.Contracts.
                Include(s => s.IdSNavigation).
                Include(p=>p.IdPNavigation).
                ThenInclude(r => r.IdRNavigation.IdUNavigation).
                Include(t => t.IdSNavigation.IdTNavigation.IdUNavigation).
                Where(e => e.IdSNavigation.IdRNavigation.IdU == temp || e.IdSNavigation.IdTNavigation.IdU == temp).ToList();

            Tuple<User, List<Authorization>,List<Contract>> tuple = new (user!, auth, contract);

            return View(tuple);
        }
    }
}
