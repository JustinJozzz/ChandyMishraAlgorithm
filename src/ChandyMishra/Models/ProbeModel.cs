using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChandyMishra.Models
{
    public class ProbeModel
    {
        public string SendToController { get; set; }
        public string InitiatedBy { get; set; }
        public string ForwardedBy { get; set; }
        public Dictionary<string, ControllerModel> AllControllers { get; set; }
        public ProbeModel(string SendToController, string InitiatedBy, string ForwardedBy, Dictionary<string, ControllerModel> AllControllers)
        {
            this.SendToController = SendToController;
            this.InitiatedBy = InitiatedBy;
            this.ForwardedBy = ForwardedBy;
            this.AllControllers = AllControllers;
        }
    }
}
