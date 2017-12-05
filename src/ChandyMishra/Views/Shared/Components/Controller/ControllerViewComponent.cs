using ChandyMishra.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChandyMishra.Controllers
{
    public class ControllerViewComponent : ViewComponent
    {
        /******************************************
         * Invoke Method send the model to the view to be built
        *******************************************/
        public IViewComponentResult Invoke(string controllername, bool deadlock, List<string> waitingfor, List<string> tablesowned)
        {
            ControllerModel model = new ControllerModel() {
                ControllerName = controllername,
                Deadlock = deadlock,
                WaitingFor = waitingfor,
                TablesOwned = tablesowned
            };
            return View(model);
        }
    }
}
