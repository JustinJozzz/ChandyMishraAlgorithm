using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Classes
{
    public class Node
    {
        public enum Locations
        {
            database = 51158,
            controller1 = 51160,
            controller2 = 51043,
            controller3 = 50830
        }
        public List<string> Data = new List<string>();
        public Locations MyLocation;
    }
}
