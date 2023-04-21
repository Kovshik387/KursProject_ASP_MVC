using Microsoft.AspNetCore.Mvc;
using DataBaseModel;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Index()
        {
            return View(await _dataBaseModelContext.Users.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Test()
        {
            return View();
        }
        
    }
}