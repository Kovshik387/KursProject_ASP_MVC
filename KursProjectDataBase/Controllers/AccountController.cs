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
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure.Internal;
using System.Buffers;
using System.Security.Claims;

namespace KursProjectDataBase.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KursProjectDataBaseContext _dataBaseModelContext;
        private readonly AccountService _accountService;
        private readonly PlacementService _placementService;

        public AccountController(ILogger<HomeController> logger, KursProjectDataBaseContext context)
        {
            _logger = logger;
            _dataBaseModelContext = context;
            _accountService = new AccountService(context);
            _placementService = new PlacementService(context);
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
            else
            {
                ModelState.AddModelError("", "Логин уже используется");
            }

            return View(entity);
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
            else
            {
                ModelState.AddModelError("","Пользователь не найден");
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
                Include(p=>p.IdPNavigation).
                Include(s => s.IdSNavigation).
                ThenInclude(r => r.IdRNavigation.IdUNavigation).
                Include(t => t.IdSNavigation.IdTNavigation.IdUNavigation).
                Where(e => e.IdSNavigation.IdRNavigation.IdU == temp || e.IdSNavigation.IdTNavigation.IdU == temp).ToList();

            Tuple<User, List<Authorization>,List<Contract>> tuple = new (user!, auth, contract);

            return View(tuple);
        }

        [HttpPost]
        [Authorize(Roles = "Tenant, Renter")]
        public IActionResult Info(UserView user) => RedirectToAction("Change", "Account");

        [HttpGet]
        [Authorize(Roles = "Tenant, Renter")]
        public IActionResult Change()
        {
            int temp = int.Parse(HttpContext.User.Identity!.Name!);
            var temporalUser = _dataBaseModelContext.Users.Include(u => u.Tenants).Include(u0 => u0.Renters).FirstOrDefault(u => u.IdU == temp);
            var auth = _dataBaseModelContext.Authorizations.Where(a => a.IdU == temp).First();
            var user = new UserView
            {
                Loginuser = auth.Loginuser,
                Passworduser = auth.Passworduser,

                Name = temporalUser!.Name,
                Surname = temporalUser.Surname,
                Sex = temporalUser.Sex,
                Contact = temporalUser.Contact
            };

            if (HttpContext.User.FindFirst("role")!.Value == "Tenant")
            {
                user.Rating = _dataBaseModelContext.Tenants.Where(t => t.IdU == temp).First().Rating;
            }
            else
            {
                user.License = _dataBaseModelContext.Renters.Where(r => r.IdU == temp).First().License;
            }

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Tenant, Renter")]
        public IActionResult Change(UserView user)
        {
            _accountService.Update(user,this.HttpContext.User.Identity!.Name!);

            return RedirectToAction("Info", "Account");
        }

        [HttpGet]
        [Authorize(Roles = "Tenant, Renter")]
        public IActionResult PlacementInfo(string id)
        {
            return View(_placementService.GetPlacementView(id));
        }

        [HttpPost]
        [Authorize(Roles = "Tenant, Renter")]
        public IActionResult PlacementInfo()
        {
            return RedirectToAction("Info", "Account");
        }


    }
}
