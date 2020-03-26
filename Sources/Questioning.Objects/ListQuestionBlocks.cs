using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestioningLibrary
{
    public class ListQuestionBlocks
    {
        public List<QuestionBlock> listQuestionBlocks = new List<QuestionBlock>();
    }

    public class QuestionBlock
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public int IdDirection { get; set; }
        public List<Question> listQuestions = new List<Question>();
    }

    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<Answer> listAnswers = new List<Answer>();
    }
}
