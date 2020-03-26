using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestioningLibrary.Questionnaires
{
    public class QQuestionBlock
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }

        public List<QQuestion> ListQQuestions = new List<QQuestion>();
    }
}
