using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataBaseModel.ViewEntity;
using DataBaseModel;
using KursProjectDataBase.Services;
using DataBaseModel.Entity;
using KursProjectDataBase.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Mvc.RazorPages;
using KursProjectDataBase.Models;

namespace KursProjectDataBase.Controllers
{
    public class PlacementController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KursProjectDataBaseContext _dataBaseModelContext;
        private readonly PlacementService _placementService;
        private readonly IConfiguration _configuration;

        public PlacementController(ILogger<HomeController> logger, KursProjectDataBaseContext context, IConfiguration configuration)
        {
            _logger = logger;
            _dataBaseModelContext = context;
            _placementService = new (context);
            _configuration = configuration;
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
            _placementService.Create(view,this.HttpContext.User.Identity!.Name!);
            return RedirectToAction("MyPlacements","Placement");
        }

        [HttpGet]
        [Authorize(Roles = "Renter")]
        public IActionResult MyPlacements() => View(_placementService.ShowPlacement(this.HttpContext.User.Identity!.Name!));

        [HttpPost]
        [Authorize(Roles ="Renter")]
        public IActionResult MyPlacements(Placement placement)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Renter")]
        public IActionResult Edit(string id) => View(_placementService.GetPlacementView(id));
        

        [HttpPost]
        [Authorize(Roles = "Renter")]
        public IActionResult Edit(PlacementView view)
        {
            _placementService.Update(view);

            return RedirectToAction("MyPlacements","Placement");
        }

        [Authorize(Roles = "Renter")]
        public IActionResult Delete(int id)
        {
            _placementService.Delete(id);
            return RedirectToAction("MyPlacements", "Placement");
        }

        [HttpGet]
        [Authorize(Roles = "Tenant")]
        public IActionResult Placements(int page = 1) 
        {
            int pageSize = 10;

            var source = _placementService.TenantPlacements();
            var count = source.Count();
            var items = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageViewModel pageViewModel = new(count,page,pageSize);
            IndexViewModel viewModel = new()
            {
                PageViewModel = pageViewModel,
                Contracts = items,
            };

            return View(viewModel); 
        }


        [HttpGet]
        [Authorize(Roles = "Tenant")]
        public IActionResult Accommodation(string id)
        {
            return View(_placementService.GetPlacementView(id));
        }

        [HttpPost]
        [Authorize(Roles = "Tenant")]
        public IActionResult Accommodation(PlacementView view)
        {
            _placementService.SetDeal(view, int.Parse(this.HttpContext.User.Identity!.Name!));
            return RedirectToAction("Info", "Account");
        }

    }
}
