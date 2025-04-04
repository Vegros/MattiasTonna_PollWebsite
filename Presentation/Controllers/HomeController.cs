using System.Diagnostics;
using MattiasTonnaEPSolution.Models;
using Microsoft.AspNetCore.Mvc;

namespace MattiasTonnaEPSolution.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       
    }
}
