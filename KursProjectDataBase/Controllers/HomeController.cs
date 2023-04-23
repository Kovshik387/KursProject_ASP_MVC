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
        public  IActionResult Test()
        {
            string temp = HttpContext.User.Identity.Name;
            _logger.LogWarning(temp);
            return View( _dataBaseModelContext.Users.FirstOrDefault(item => Convert.ToInt32(temp) == item.IdU));
        }
        
    }
}