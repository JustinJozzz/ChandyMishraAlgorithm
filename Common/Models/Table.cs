using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Table
    {
        public string Name;
        public bool Available = true;
        public Table(string Name)
        {
            this.Name = Name;
        }
    }
}
