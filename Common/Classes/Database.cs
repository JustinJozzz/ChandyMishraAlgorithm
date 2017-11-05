using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Classes
{
    public class Database : Node
    {
        public Table[] Tables;
        
        public Database(Locations MyLocation)
        {
            this.MyLocation = MyLocation;
            Tables = new Table[] { new Table("Users"), new Table("Postings"), new Table("Friends") };
        }
        public async Task<string> GetData(string Value)
        {
            if(Tables.Where(x => x.Name == Value).FirstOrDefault().Available)
            {
                Tables.Where(x => x.Name == Value).FirstOrDefault().Available = false;

                return Tables.Where(x => x.Name == Value).FirstOrDefault().Name;
            }
            else
            {
                return "Error";
            }
        }
        public async Task ReturnTable(string Value)
        {
            Tables.Where(x => x.Name == Value).FirstOrDefault().Available = true;
        }
    }
}
