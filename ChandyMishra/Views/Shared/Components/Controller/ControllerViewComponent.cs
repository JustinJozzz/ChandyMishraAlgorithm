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
    public class ControllerViewComponent: ViewComponent
    {
        private static FirebaseClient firebase = new FirebaseClient("https://chandymishra-74bcd.firebaseio.com/");
        public async Task<IViewComponentResult> InvokeAsync(string ControllerName)
        {
            var model = await firebase.Child("controllers").Child(ControllerName).OnceSingleAsync<ControllerModel>();
            model.ControllerName = ControllerName;

            return View(model);
        }
    }
}
