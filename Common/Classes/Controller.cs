using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Classes
{
    public class Controller : Node
    {
        public Dictionary<Locations, bool> Dependent = new Dictionary<Locations, bool>()
        {
            { (Locations)Enum.Parse(typeof(Locations), "controller1"), false },
            { (Locations)Enum.Parse(typeof(Locations), "controller2"), false },
            { (Locations)Enum.Parse(typeof(Locations), "controller3"), false }
        };
        public bool Deadlock = false;
        public Controller(Locations MyLocation)
        {
            this.MyLocation = MyLocation;
        }
        public async Task GetData(HttpClient client, Locations Destination, string TableName)
        {
            var Response = await client.GetAsync($"http://localhost:{((int)Destination).ToString()}/data?value={TableName}");
            string ResponseString = await Response.Content.ReadAsStringAsync();

            if(!ResponseString.Contains("Error"))
            {
                Data.Add(ResponseString);
            }
            else
            {
                foreach(var DependentController in Dependent.Select(x => x.Key))
                {
                    SendProbe(client, DependentController, TableName, Dependent[DependentController]);
                }
            }
        }
        public async Task ReturnData(HttpClient client, Locations Destination, string TableName)
        {
            Data.Remove(TableName);

            var content = new StringContent(TableName, Encoding.UTF8, "application/json");

            await client.PostAsync($"http://localhost:{((int)Destination).ToString()}/return", content);

            foreach(var DependentController in Dependent.Where(x => x.Value).Select(x => x.Key))
            {
                Dependent[DependentController] = false;
            }
        }
        public async Task ReceiveProbe(Probe ProbeModel)
        {
            if(Data.Contains(ProbeModel.TableName))
                Dependent[ProbeModel.Controller] = true;
            if(ProbeModel.DependentOn)
                Deadlock = true;
        }
        public async Task SendProbe(HttpClient client, Locations Controller, string TableName, bool DependentOn)
        {
            Probe ProbeModel = new Probe(MyLocation, TableName, DependentOn);
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(ProbeModel), Encoding.UTF8, "application/json");

            await client.PostAsync($"http://localhost:{((int)Controller).ToString()}/probe", content);
        }
    }
}
