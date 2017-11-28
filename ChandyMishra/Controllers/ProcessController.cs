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
        [HttpGet]
        [Route("process/{controllerNum?}")]
        public IActionResult Index(string controllerNum)
        {
            ProcessModel model = new ProcessModel(controllerNum);
            return View("ProcessView", model);
        }
        [HttpPost]
        public async void GetTables([FromBody]TableModel model)
        {
            var firebase = new FirebaseClient("https://chandymishra-74bcd.firebaseio.com/");
            var tables = await firebase.Child("tables").OnceAsync<Boolean>();
            var dependent = await firebase.Child($"controller{model.controllerNum}").Child("dependent").OnceAsync<Boolean>();

            bool available = tables.Where(x => x.Key == model.table).FirstOrDefault().Object;
            if(available)
            {
                await firebase.Child("tables").Child(model.table).PutAsync(false);
                await firebase.Child($"controller{model.controllerNum}").Child("data").Child(model.table).PutAsync(true);
            }
            else
            {
                for (int i = 1; i <= 3; i++)
                {
                    Probe(new ProbeModel(i.ToString(), model.table, model.controllerNum, dependent.Where(x => x.Key == $"controller{i}").FirstOrDefault().Object));
                }
            }
        }
        public async void Probe(ProbeModel model)
        {
            var firebase = new FirebaseClient("https://chandymishra-74bcd.firebaseio.com/");
            var tables = await firebase.Child($"controller{model.ControllerNum}").Child("data").OnceAsync<Boolean>();
            var dependent = await firebase.Child($"controller{model.ControllerNum}").Child("data").OnceAsync<Boolean>();

            if (tables.Where(x => x.Key == model.Table).FirstOrDefault().Object)
            {
                await firebase.Child($"controller{model.ControllerNum}").Child("dependent").Child($"controller{model.SentBy}").PutAsync(true);
                if (model.DependentOn)
                    await firebase.Child($"controller{model.ControllerNum}").Child("deadlock").PutAsync(true);
            }
        }
    }
}
