using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChandyMishra.Models
{
    public class ProbeModel
    {
        public string ControllerName;
        public string Table;
        public string SentBy;
        public bool DependentOn;
        public ProbeModel(string ControllerName, string Table, string SentBy, bool DependentOn)
        {
            this.ControllerName = ControllerName;
            this.Table = Table;
            this.SentBy = SentBy;
            this.DependentOn = DependentOn;
        }
    }
}
