using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChandyMishra.Models
{
    public class ProbeModel
    {
        public ControllerModel Controller { get; set; }
        public string Table { get; set; }
        public string SentBy { get; set; }
        public bool DependentOn { get; set; }
        public ProbeModel(ControllerModel Controller, string Table, string SentBy, bool DependentOn)
        {
            this.Controller = Controller;
            this.Table = Table;
            this.SentBy = SentBy;
            this.DependentOn = DependentOn;
        }
    }
}
