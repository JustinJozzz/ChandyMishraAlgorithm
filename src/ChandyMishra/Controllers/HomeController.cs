using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChandyMishra.Models;

namespace ChandyMishra.Controllers
{
    public class HomeController : Controller
    {
        /******************************************
         * Returns the view of the interface page
        *******************************************/
        public IActionResult Index()
        {
            return View();
        }
    }
}
