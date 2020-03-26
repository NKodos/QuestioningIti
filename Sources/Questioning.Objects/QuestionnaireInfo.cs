using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestioningLibrary
{
    public class QuestionnaireInfo
    {
        public string Id { get; set; }
        public string Direction { get; set; }
        public string DateCreation { get; set; }
        public int CountTasks { get; set; }
        public int CountBusinesses { get; set; }
    }
}
