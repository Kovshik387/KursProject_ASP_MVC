using Microsoft.AspNetCore.Mvc;
using DataBaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace KursProjectDataBase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KursProjectDataBaseContext _dataBaseModelContext;

        public HomeController(ILogger<HomeController> logger, KursProjectDataBaseContext context)
        {
            _logger = logger;
            _dataBaseModelContext = context; 
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _dataBaseModelContext.Users.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize (Roles ="Пользователь")]
        public async Task<IActionResult> Test()
        {
            var authorization = await _dataBaseModelContext.Authorizations.FirstOrDefaultAsync(item => item.Loginuser == HttpContext.User.Identity!.Name);

            

            return View(await _dataBaseModelContext.Users.FirstOrDefaultAsync(item => authorization!.IdU == item.IdU));
        }
        
    }
}