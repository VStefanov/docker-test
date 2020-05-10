using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.MyTestApp.Models
{
    public class IndexViewModel
    {
        public int Index { get; set; }

        public List<string> AvailableIndexes { get; set; }
        
        public Dictionary<string, string> AvailableIndexesValues { get; set; }
    }
}
