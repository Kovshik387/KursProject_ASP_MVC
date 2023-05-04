using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataBaseModel.ViewEntity;
using DataBaseModel;
using KursProjectDataBase.Services;

namespace KursProjectDataBase.Controllers
{
    public class PlacementController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KursProjectDataBaseContext _dataBaseModelContext;
        private readonly PlacementService _placementService;

        public PlacementController(ILogger<HomeController> logger, KursProjectDataBaseContext context)
        {
            _logger = logger;
            _dataBaseModelContext = context;
            _placementService = new (context);
            
        }

        [HttpGet]
        [Authorize (Roles ="Renter")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Renter")]
        public IActionResult Create(PlacementView view)
        {
            _placementService.Create(view,this.HttpContext.User.Identity!.Name);
            return View();
        }

    }
}
