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
                    allControllers[entry.Key].TablesOwned = allControllers[entry.Key].TablesOwned ?? new List<string>();
                    allControllers[entry.Key].TablesOwned.Add(entry.Value);
                    await firebase.Child("tables").Child($"{entry.Value}").PutAsync(false);
                }
                else
                {
                    foreach(var controller in allControllers.Keys.ToArray())
                    {
                        allControllers[entry.Key].Dependent = allControllers[entry.Key].Dependent ?? new List<string>();
                        allControllers[controller] = await Probe(
                                new ProbeModel(allControllers[controller], entry.Value, entry.Key, allControllers[entry.Key].Dependent.Contains(controller))
                            );
                    }
                }
            }
            await firebase.Child("controllers").PutAsync(allControllers);
        }
        [HttpGet]
        public IActionResult NewController(string ControllerName)
        {
            return ViewComponent("Controller", ControllerName);
        }
        public async Task<ControllerModel> Probe(ProbeModel model)
        {
            var controller = model.Controller;
            controller.TablesOwned = controller.TablesOwned ?? new List<string>();

            if (controller.TablesOwned.Contains(model.Table))
            {
                if (model.DependentOn || model.SentBy == controller.ControllerName)
                    controller.Deadlock = true;
                controller.Dependent = controller.Dependent ?? new List<string>();
                controller.Dependent.Add(model.SentBy);
            }

            return controller;
        }
    }
}
