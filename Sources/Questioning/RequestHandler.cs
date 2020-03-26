using QuestioningLibrary;

namespace Questioning
{
    public class RequestHandler
    {
        private QueryToDB _query = new QueryToDB();
        private string _answer = "";

        /// <summary>
        /// Сохранение обьекта Анкета в БД
        /// </summary>
        public void SaveQuestionnaire(string id, string direction, string dateCreation)
        {
            _query.Type = "INSERT";
            _query.Table = "tasktype";
            _query.Query = "INSERT INTO questioning.questionnaires (id, direction, date) VALUES ('" + id + "', '" + direction + "', '" + dateCreation + "')";
            QueryToDB.SendQuery(_query);

            _query.Type = "UPDATE";
            _query.Table = "directitons";
            _query.Query = "UPDATE questioning.directions SET isCreatedQuestioning = 1 WHERE name = '" + direction + "'";
            QueryToDB.SendQuery(_query);
        }

        /// <summary>
        /// Сохранение обьекста Блок вопросов в БД
        /// </summary>
        public void SaveQuestionBlock(string idQB, string idQuestionnnaire, string description, string direction)
        {
            _query.Type = "SELECT";
            _query.Table = "QuestionBlocks";
            _query.Query = "SELECT * FROM questioning.tasktype WHERE description  = '" + description + "' AND idDirections = (SELECT id FROM questioning.directions WHERE name = '" + direction + "')";
            _answer = QueryToDB.SendQuery(_query);
            ListQuestionBlocks LQB = (ListQuestionBlocks)QueryToDB.ProcessResponse(_answer, _query);

            _query.Type = "INSERT";
            _query.Table = "qquestionsblocks";
            _query.Query = "INSERT INTO questioning.qquestionsblocks (id, name, idQuestionnaire, description) VALUES ('" + idQB + "', '" + LQB.listQuestionBlocks[0].Title + "', '" + idQuestionnnaire + "', '" + description + "')";
            QueryToDB.SendQuery(_query);

            SaveQuestions(idQB, LQB.listQuestionBlocks[0]);
        }
        
        /// <summary>
        /// Сохранение вопросов
        /// </summary>
        public void SaveQuestions(string idQB, QuestionBlock QB)
        {
            foreach (Question question in QB.listQuestions)
            {
                _query.Type = "INSERT";
                _query.Table = "qquestions";
                _query.Query = "INSERT INTO questioning.qquestions (idQuestionBlock, text) VALUES ('" + idQB + "', '" + question.Text + "')";
                QueryToDB.SendQuery(_query);
            }
        }
    }
}
