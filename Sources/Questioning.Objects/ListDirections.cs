using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestioningLibrary
{
    public class ListDirections
    {
        public List<Direction> listDirections = new List<Direction>();
    }

    public class Direction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
