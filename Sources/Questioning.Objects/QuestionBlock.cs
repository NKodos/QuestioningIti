using System.Collections.Generic;

namespace QuestioningLibrary
{
    public class QuestionBlock
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public int IdDirection { get; set; }

        public List<Question> ListQuestions = new List<Question>();
    }
}
