using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestServerConsole
{
    public class ListIndustry
    {
        public List<Industry> listIndustry = new List<Industry>();
    }

    public class Industry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
