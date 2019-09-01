using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServerConsole
{
    public class ListBusinesses
    {
        public List<Business> listBusinesses = new List<Business>();
    }

    public class Business
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Industry { get; set; }
        public string Description { get; set; }
    }
}
