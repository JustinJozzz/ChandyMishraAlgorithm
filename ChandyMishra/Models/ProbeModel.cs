using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChandyMishra.Models
{
    public class ProbeModel
    {
        public string ControllerNum;
        public string Table;
        public string SentBy;
        public bool DependentOn;
        public ProbeModel(string ControllerNum, string Table, string SentBy, bool DependentOn)
        {
            this.ControllerNum = ControllerNum;
            this.Table = Table;
            this.SentBy = SentBy;
            this.DependentOn = DependentOn;
        }
    }
}
