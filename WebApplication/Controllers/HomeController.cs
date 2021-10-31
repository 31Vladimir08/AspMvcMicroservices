using WebApplication.Filters;

namespace WebApplication.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Diagnostics;

    using WebApplication.Models;

    [ServiceFilter(typeof(LogingCallsActionFilter))]
    public class HomeController : Controller
    {
        [ServiceFilter(typeof(LogingCallsActionFilter))]
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
