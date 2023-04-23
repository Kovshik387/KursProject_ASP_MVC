using DataBaseModel;
using DataBaseModel.Entity;
using KursProjectDataBase.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Register(TemporalEntity entity)
        {
            _logger.LogInformation(entity.ToString());
            _ = _accountService.Register(entity!);

            RedirectToAction("/index","Home");

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

            if (_accountService.Login(authorization)) {
                RedirectToAction("/Test", "Home");
                _logger.LogInformation("Успешно");
            }
            return View(authorization);
        }
    }
}
