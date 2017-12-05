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
        //connection to firebase database
        private static FirebaseClient firebase = new FirebaseClient("https://chandymishra-74bcd.firebaseio.com/");
        /******************************************
         * returns the controller detail page
        *******************************************/
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var controllers = await firebase.Child("controllers").OnceAsync<ControllersModel>();

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
        /******************************************
         * Takes the dictionary output from the interface page,
         * loops through each assignment and puts it into the database.
        *******************************************/
        [HttpPost]
        public async void AssignTables([FromBody]Dictionary<string, string> Assignments)
        {
            var allControllers = await firebase.Child("controllers").OnceSingleAsync<Dictionary<string, ControllerModel>>();
            foreach (KeyValuePair<string, string> entry in Assignments)
            {
                bool tableAvailable = await firebase.Child("tables").Child($"{entry.Value}").OnceSingleAsync<bool>();
                if (tableAvailable)
                {
                    allControllers[entry.Key].TablesOwned = allControllers[entry.Key].TablesOwned ?? new List<string>();
                    allControllers[entry.Key].TablesOwned.Add(entry.Value);
                    await firebase.Child("tables").Child($"{entry.Value}").PutAsync(false);
                }
                else
                {
                    allControllers[entry.Key].WaitingFor = allControllers[entry.Key].WaitingFor ?? new List<string>();
                    allControllers[entry.Key].WaitingFor.Add(entry.Value);
                    string ProbeController = allControllers.FirstOrDefault(x => x.Value.TablesOwned.Contains(entry.Value)).Key;
                    if(ProbeController != null)
                        allControllers = await Probe(new ProbeModel(ProbeController, entry.Key, entry.Key, allControllers));
                }
            }
            await firebase.Child("controllers").PutAsync(allControllers);
        }
        /******************************************
         * Makes a new controller view component using a controller object
         * from the database.
        *******************************************/
        [HttpGet]
        public async Task<IActionResult> NewController(string ControllerName)
        {
            ControllerModel Controller = await firebase.Child("controllers").Child($"{ControllerName}").OnceSingleAsync<ControllerModel>();
            Controller.ControllerName = ControllerName;
            Controller.WaitingFor = Controller.WaitingFor ?? null;
            Controller.TablesOwned = Controller.TablesOwned ?? null;

            return ViewComponent("Controller", new { Controller.ControllerName, Controller.Deadlock, Controller.WaitingFor, Controller.TablesOwned });
        }
        /******************************************
         * Allows a controller to recieve a probe and detect deadlock.
        *******************************************/
        public async Task<Dictionary<string, ControllerModel>> Probe(ProbeModel model)
        {
            ControllerModel ThisController = model.AllControllers[model.SendToController];
            ThisController.ControllerName = model.SendToController;
            ThisController.WaitingFor = ThisController.WaitingFor ?? new List<string>();

            if(ThisController.ControllerName == model.InitiatedBy)
            {
                model.AllControllers[model.SendToController].Deadlock = true;
            }
            else
            {
                foreach (string Table in ThisController.WaitingFor)
                {
                    string ProbeController = model.AllControllers.FirstOrDefault(x => x.Value.TablesOwned.Contains(Table)).Key;
                    model.AllControllers = await Probe(new ProbeModel(ProbeController, model.InitiatedBy, model.SendToController, model.AllControllers));
                }
            }

            return model.AllControllers;
        }
    }
}

