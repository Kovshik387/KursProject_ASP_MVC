using DataBaseModel;
using DataBaseModel.Entity;
using KursProjectDataBase.Models;
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
        public IActionResult UsersList(string role = "Tenant", int count = 0)
        {
            ViewBag.Role = role;
            Tuple<List<Tenant>, List<Renter>> users;
            if (count > 1) users = _adminService.UsersGetCount(count);
            else users = _adminService.UsersGet();
            return View(users);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UserInfo(string id, string type)
        {
            return View(_adminService.UserSolution(id,type));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UserDelete(string id, string type)
        {
            _adminService.DeleteUser(id, type);
            return RedirectToAction("UsersList", "Admin");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult UserInfo(UserModelView user)
        {
            _adminService.UpdateUser(user);
            return RedirectToAction("UsersList", "Admin");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult ReportListAsync(string first, string last)
        {
            var result = _adminService.GetReport();
            ReportViewModel reportViewModel = new ReportViewModel()
            {
                Contracts = result,
                CountContracts = result.Count(),
                SumContracts = result.Sum(s => s.Paymentsize),
            };

            return View(reportViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ReportListAsync(ReportViewModel view)
        {
            if (view.Email != null)
            {
                EmailService emailService = new EmailService();
                await emailService.SentReportAsync(_adminService.GetReport().ToList(), view.Email);
            }

            if (view.FirstDate == null || view.LastDate == null) return RedirectToAction("ReportList");

            IQueryable<Contract> result = _adminService.GetReportTime(DateOnly.Parse(view.FirstDate), DateOnly.Parse(view.LastDate));
            ReportViewModel reportViewModel = new ReportViewModel()
            {
                Contracts = result,
                CountContracts = result.Count(),
                SumContracts = result.Sum(s => s.Paymentsize),
            };

            return View(reportViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Report()
        {
            var result = _adminService.GetReportThisMonth(date: DateOnly.FromDateTime(DateTime.Now));
            ReportViewModel reportViewModel = new ReportViewModel()
            {
                Contracts = result,
                CountContracts = result.Count(),
                SumContracts = result.Sum(s => s.Paymentsize),
            };

            return View(reportViewModel);
        }
    }
}
