using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Common.Classes.Node;

namespace Common.Models
{
    public class Probe
    {
        public Locations Controller;
        public string TableName;
        public bool DependentOn;
        public Probe(Locations Controller, string TableName, bool DependentOn)
        {
            this.Controller = Controller;
            this.TableName = TableName;
            this.DependentOn = DependentOn;
        }
    }
}
