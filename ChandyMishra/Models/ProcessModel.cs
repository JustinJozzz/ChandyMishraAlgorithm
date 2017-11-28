using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChandyMishra.Models
{
    public class ProcessModel
    {
        public int ControllerNum;
        public ProcessModel(string ControllerNum)
        {
            this.ControllerNum = Int32.Parse(ControllerNum);
        }
    }
}
