using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.MyTestApp.Services
{
    public interface IFibService
    {
        public void AddIndexToStorage(string number);

        public List<string> GetAvailableIndexes();

        public Dictionary<string, string> GetAvailableIndexesValues();
    }
}
