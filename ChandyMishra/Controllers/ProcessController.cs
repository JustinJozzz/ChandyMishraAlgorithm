using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChandyMishra.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Net;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChandyMishra.Controllers
{
    public class ProcessController : Controller
    {
        private static FirebaseClient firebase = new FirebaseClient("https://chandymishra-74bcd.firebaseio.com/");
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var controllers= await firebase.Child("controllers").OnceAsync<ControllersModel>();

            ControllersModel model = new ControllersModel()
            {
                Controllers = new List<string>()
            };
            foreach (var controller in controllers)
            {
                model.Controllers.Add(controller.Key);
            }

            return View("ProcessView", model);
        }
        [HttpPost]
        public async void AssignTables([FromBody]Dictionary<string, string> Assignments)
        {
            var allControllers = await firebase.Child("controllers").OnceSingleAsync<Dictionary<string, ControllerModel>>();
            foreach (KeyValuePair<string, string> entry in Assignments)
            {
                bool tableAvailable = await firebase.Child("tables").Child($"{entry.Value}").OnceSingleAsync<bool>();
                if(tableAvailable)
                {
                    var tablesOwned = allControllers[entry.Key].TablesOwned ?? new List<string>();
                    tablesOwned.Add(entry.Value);
                    await firebase.Child("controllers").Child($"{entry.Key}").Child("tablesOwned").PutAsync(tablesOwned.ToArray());
                    await firebase.Child("tables").Child($"{entry.Value}").PutAsync(false);
                }
                else
                {
                    var dependent = allControllers[entry.Key].Dependent ?? new List<string>();

                    foreach(var controller in allControllers)
                    {
                        Probe(new ProbeModel(controller.Key, entry.Value, entry.Key, dependent.Contains(controller.Key)));
                    }
                }
            }
        }
        [HttpGet]
        public IActionResult NewController(string ControllerName)
        {
            return ViewComponent("Controller", ControllerName);
        }
        public async void Probe(ProbeModel model)
        {
            var controller = await firebase.Child("controllers").Child($"{model.ControllerName}").OnceSingleAsync<ControllerModel>();
            var tablesOwned = controller.TablesOwned ?? new List<string>();

            if (controller.TablesOwned.Contains(model.Table))
            {
                if (model.DependentOn || model.SentBy == model.ControllerName)
                    await firebase.Child("controllers").Child($"{model.ControllerName}").Child("deadlock").PutAsync(true);
                var dependent = controller.Dependent ?? new List<string>();
                dependent.Add(model.SentBy);
                await firebase.Child("controllers").Child($"{model.ControllerName}").Child("dependent").PutAsync(dependent.ToArray());
            }
        }
    }
}
