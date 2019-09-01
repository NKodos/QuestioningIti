using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestioningLibrary.Questionnaires
{
    public class Questionnaire
    {
        public Guid Id { get; set; }
        public string DirectionName { get; set; }
        public DateTime DateCreation { get; set; }

        public List<QQuestionBlock> ListQQuestionBlocks = new List<QQuestionBlock>();
    }
}
