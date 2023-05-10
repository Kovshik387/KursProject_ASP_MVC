using DataBaseModel.Response;
using Microsoft.AspNetCore.Mvc;

namespace KursProjectDataBase.Controllers
{
    public class Error : Controller
    {

        [HttpGet]
        public IActionResult Index() 
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult Index(string contoller, string action)
        {
            return RedirectToAction(action,contoller);
        }
    }
}
