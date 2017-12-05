using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ChandyMishra.Models
{
    public class ControllerModel
    {
        public string ControllerName { get; set; }
        public List<string> TablesOwned { get; set; }
        public bool Deadlock { get; set; }
        public List<string> WaitingFor{ get; set; }

        public string GetTitle()
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            return textInfo.ToTitleCase(ControllerName);
        }
    }
}
