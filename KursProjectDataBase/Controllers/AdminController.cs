using DataBaseModel;
using KursProjectDataBase.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace KursProjectDataBase.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KursProjectDataBaseContext _dataBaseModelContext;
        private readonly AdminService _adminService;

        public AdminController(ILogger<HomeController> logger, KursProjectDataBaseContext context)
        {
            _logger = logger;
            _dataBaseModelContext = context;
            _adminService = new AdminService(context);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UsersList(string role = "Tenant")
        {
            ViewBag.Role = role;
            var users = _adminService.UsersGet();
            return View(_adminService.UsersGet());
        }
    }
}
