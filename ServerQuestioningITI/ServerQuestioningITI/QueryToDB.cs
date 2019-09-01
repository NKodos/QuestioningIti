using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using QuestioningLibrary;
using QuestioningLibrary.Questionnaires;
using System;
using System.Collections.Generic;

namespace ServerQuestioningITI
{
    public class QueryToDB
    {
        private string connStr = "server=localhost;CharSet=utf8;user=root;database=questioning;port=3306;";
        public string Type { get; set; }
        public string Table { get; set; }
        public string Query { get; set; }
        public string nameCompanyForAnswers { get; set; } // условие для вывода Ответов
        public string nameDirectionForAnswers { get; set; } // условие для вывода Ответов
        public string password { get; set; } // пароль для авторизации

        public string RunQuery()
        {
            string answer = "";

            MySqlDataReader rdr;
            MySqlConnection connection = new MySqlConnection(connStr);
            connection.Open();
            MySqlCommand cmd = new MySqlCommand(Query, connection);
            if (Type != "SELECT")
            {
                cmd.ExecuteNonQuery();
                answer = "ok";
            }
            else
            {
                rdr = cmd.ExecuteReader();
                switch (Table)
                {
                    case "QuestionBlocks":
                        answer = GetJsonDataForQuestionBlocks(rdr);
                        break;

                    case "Businesses":
                        answer = GetJsonDataForBusinesses(rdr);
                        break;

                    case "Directions":
                        answer = GetJsonDataForDirections(rdr);
                        break;
                        
                    case "Industry":
                        answer = GetJsonDataForIndutry(rdr);
                        break;

                    case "Answers":
                        answer = GetJsonDataForAnswers();
                        break;

                    case "Users":
                        answer = CheckLogAndPass(rdr);
                        break;

                    case "Questionnaire":
                        answer = GetJsonDataForQuestionnaire(rdr);
                        break;

                    case "QuestionnaireInfo":
                        answer = GetJsonDataForQuestionnaireInfo();
                        break;
                }

            }
            connection.Close();
            return answer;
        }

        // получить json-строку для Отраслей Предприятий
        private string GetJsonDataForIndutry(MySqlDataReader rdr)
        {
            ListIndustry list = new ListIndustry();

            while (rdr.Read())
            {
                Industry Ind1 = new Industry() { Id = rdr.GetInt32("id"), Name = rdr["name"].ToString(), Description = rdr["description"].ToString() };
                list.listIndustry.Add(Ind1);
            }
            rdr.Close();

            return JsonConvert.SerializeObject(list);
        }

        // получить json-строку для Анкет
        private string GetJsonDataForQuestionnaire(MySqlDataReader rdr)
        {
            MySqlDataReader rdrQB, rdrQuestion;
            MySqlConnection connectionForQB = new MySqlConnection(connStr);
            MySqlConnection connectionForQuestions = new MySqlConnection(connStr);
            DateTime datetime = new DateTime();

            rdr.Read();

            Questionnaire questionnair = new Questionnaire()
            {
                Id = new Guid(rdr["id"].ToString()),
                DirectionName = rdr["direction"].ToString()
            };

            DateTime.TryParse(rdr["date"].ToString(), out datetime);
            questionnair.DateCreation = datetime;

            rdr.Close();

            connectionForQB.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM questioning.qquestionsblocks WHERE idQuestionnaire='" + questionnair.Id.ToString() + "'", connectionForQB);
            rdrQB = cmd.ExecuteReader();

            while (rdrQB.Read())
            {
                QQuestionBlock QQB = new QQuestionBlock() { Id = new Guid(rdrQB["id"].ToString()), Title = rdrQB["name"].ToString(), ShortName = rdrQB["description"].ToString() };

                connectionForQuestions.Open();
                cmd = new MySqlCommand("SELECT * FROM questioning.qquestions WHERE idQuestionBlock='" + QQB.Id.ToString() + "'", connectionForQuestions);
                rdrQuestion = cmd.ExecuteReader();

                while (rdrQuestion.Read())
                {
                    QQB.ListQQuestions.Add(new QQuestion() { Id = Convert.ToInt32(rdrQuestion["id"]), Text = rdrQuestion["text"].ToString()});
                }

                questionnair.ListQQuestionBlocks.Add(QQB);
                rdrQuestion.Close();
                connectionForQuestions.Close();
            }
            rdrQB.Close();
            connectionForQB.Close();

            return JsonConvert.SerializeObject(questionnair);
        }

        // получить json-строку для Анкет
        private string GetJsonDataForQuestionnaireInfo()
        {
            MySqlDataReader rdr, rdr1;
            List<QuestionnaireInfo> listQuestionnaireInfo = new List<QuestionnaireInfo>();
            DateTime datetime = new DateTime();
            MySqlConnection connection = new MySqlConnection(connStr);
            MySqlConnection connection1 = new MySqlConnection(connStr);

            connection.Open();
            Query = "SELECT * FROM questionnaires";
            MySqlCommand cmd = new MySqlCommand(Query, connection);
            rdr = cmd.ExecuteReader();
            rdr.Close();

            while (rdr.Read())
            {
                connection.Open();
                Query = "SELECT COUNT(id) FROM qquestionsblocks WHERE qquestionsblocks.idQuestionnaire = '" + rdr["id"].ToString() + "'";
                cmd = new MySqlCommand(Query, connection);
                int countTasks = (int)cmd.ExecuteScalar();
                connection.Close();

                connection.Open();
                Query = "SELECT qanswers.id FROM qanswers JOIN qquestions ON qanswers.idQuestion = qquestions.id" +
                    "JOIN qquestionsblocks ON qquestions.idQuestionBlock = qquestionsblocks.id WHERE qquestionsblocks.idQuestionnaire = '" + rdr["id"].ToString() + "' GROUP BY qanswers.idBusinesses";
                cmd = new MySqlCommand(Query, connection);
                int countBusinesses = (int)cmd.ExecuteScalar();
                connection.Close();

                datetime = Convert.ToDateTime(rdr["date"].ToString());

                listQuestionnaireInfo.Add(new QuestionnaireInfo()
                {
                    Id = rdr["id"].ToString(),
                    Direction = rdr["direction"].ToString(),
                    DateCreation = datetime.ToString("dd.MM.yy"),
                    CountTasks = countTasks
                });
            }

            rdr.Close();
            connection.Close();

            return JsonConvert.SerializeObject(listQuestionnaireInfo);
        }

        // получить json-строку для блоков с вопросами
        private string GetJsonDataForQuestionBlocks(MySqlDataReader rdr)
        {
            ListQuestionBlocks list = new ListQuestionBlocks();

            while (rdr.Read())
            {
                List<Question> listQuestions = new List<Question>();
                MySqlDataReader rdr1;
                MySqlConnection connection = new MySqlConnection(connStr);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM questioning.questions WHERE idTaskType=" + rdr["id"].ToString(), connection);
                rdr1 = cmd.ExecuteReader();

                while (rdr1.Read())
                {
                    listQuestions.Add(new Question() { Id = Convert.ToInt32(rdr1["id"]),  Text = rdr1["text"].ToString() });
                }
                rdr1.Close();
                connection.Close();
                list.listQuestionBlocks.Add(new QuestionBlock()
                {
                    Id = int.Parse(rdr["id"].ToString()),
                    Title = rdr["name"].ToString(),
                    ShortName = String.IsNullOrEmpty(rdr["description"].ToString()) ? "" : char.ToUpper(rdr["description"].ToString()[0]) + rdr["description"].ToString().Substring(1),
                    IdDirection = Convert.ToInt32(rdr["idDirections"]),
                    listQuestions = listQuestions
                });
            }
            rdr.Close();
            
            return JsonConvert.SerializeObject(list);
        }


        // получить json-строку для Предприятий
        private string GetJsonDataForBusinesses(MySqlDataReader rdr)
        {
            ListBusinesses list = new ListBusinesses();

            while (rdr.Read())
            {
                Business b1 = new Business() { Id = Convert.ToInt32(rdr["id"]), Name = rdr["name"].ToString(), Description = rdr["description"].ToString(), Email = rdr["email"].ToString() };
                MySqlDataReader rdr1;
                MySqlConnection connection = new MySqlConnection(connStr);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM questioning.industry WHERE id=" + rdr["idIndustry"].ToString(), connection);
                rdr1 = cmd.ExecuteReader();

                while (rdr1.Read())
                {
                    b1.Industry = rdr1["name"].ToString();
                }
                rdr1.Close();
                connection.Close();
                list.listBusinesses.Add(b1);
            }
            rdr.Close();
            
            return JsonConvert.SerializeObject(list);
        }

        // получить json-строку для Направлений
        private string GetJsonDataForDirections(MySqlDataReader rdr)
        {
            ListDirections list = new ListDirections();

            while (rdr.Read())
            {
                Direction d1 = new Direction() { Id = int.Parse(rdr["id"].ToString()), Name = rdr["name"].ToString(), Description = rdr["description"].ToString() };
                list.listDirections.Add(d1);
            }
            rdr.Close();

            return JsonConvert.SerializeObject(list);
        }

        // получить json-строку для Ответов
        private string GetJsonDataForAnswers()
        {
            ListAnswersBlock list = new ListAnswersBlock();
            AnswersBlock aB = new AnswersBlock();
            MySqlDataReader rdrForCompany;
            MySqlConnection connectionForCompany = new MySqlConnection(connStr);

            if (this.nameCompanyForAnswers != null)
            {
                string queryForCompany = "SELECT name FROM questioning.businesses "
                    + "WHERE name = '" + this.nameCompanyForAnswers + "' AND (SELECT count(id) FROM questioning.qanswers "
                    + "WHERE idBusinesses=businesses.id AND idQuestion=(SELECT id FROM questioning.qquestions "
                    + "WHERE idQuestionBlock = (SELECT id FROM questioning.qquestionsblocks "
                    + "WHERE idQuestionnaire = (SELECT id FROM questioning.questionnaires WHERE direction = '" + this.nameDirectionForAnswers + "' LIMIT 1)LIMIT 1)LIMIT 1) ) > 0";
                connectionForCompany.Open();
                MySqlCommand cmd = new MySqlCommand(queryForCompany, connectionForCompany);
                rdrForCompany = cmd.ExecuteReader();
            }
            else
            {
                string queryForCompany = "SELECT name FROM questioning.businesses "
                    + "WHERE (SELECT count(id) FROM questioning.qanswers "
                    + "WHERE idBusinesses=businesses.id AND idQuestion=(SELECT id FROM questioning.qquestions "
                    + "WHERE idQuestionBlock = (SELECT id FROM questioning.qquestionsblocks "
                    + "WHERE idQuestionnaire = (SELECT id FROM questioning.questionnaires WHERE direction = '" + this.nameDirectionForAnswers + "' LIMIT 1)LIMIT 1)LIMIT 1) ) > 0";
                connectionForCompany.Open();
                MySqlCommand cmd = new MySqlCommand(queryForCompany, connectionForCompany);
                rdrForCompany = cmd.ExecuteReader();
            }
            

            while (rdrForCompany.Read())
            {
                MySqlConnection connection0 = new MySqlConnection(connStr);
                connection0.Open();
                MySqlCommand cmd0 = new MySqlCommand(Query, connection0);
                MySqlDataReader rdr = cmd0.ExecuteReader();

                aB = new AnswersBlock() { nameCompany = rdrForCompany.GetString("name") };

                while (rdr.Read())
                {
                    List<Question> listQuestions = new List<Question>();
                    MySqlConnection connection = new MySqlConnection(connStr);
                    connection.Open();
                    string test = rdr["id"].ToString();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM questioning.qquestions WHERE idQuestionBlock='" + rdr["id"].ToString() + "'", connection);
                    MySqlDataReader rdr1 = cmd.ExecuteReader();

                    while (rdr1.Read())
                    {
                        Question question = new Question() { Id = Convert.ToInt32(rdr1["id"]), Text = rdr1["text"].ToString() };

                        MySqlDataReader rdr2;
                        MySqlConnection connection1 = new MySqlConnection(connStr);
                        connection1.Open();
                        MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM questioning.qanswers WHERE idQuestion=" + question.Id.ToString() + " AND idBusinesses = " +
                            "(SELECT id FROM questioning.businesses WHERE name = '" + rdrForCompany.GetString("name") + "')", connection1);
                        rdr2 = cmd1.ExecuteReader();

                        while (rdr2.Read())
                        {
                            question.listAnswers.Add(new Answer() { idBusinesses = rdr2.GetInt32("idBusinesses"), idQuestions = rdr2.GetInt32("idQuestion"), idVariantsOfAnswers = rdr2.GetInt32("idVariantsOfAnswers") });
                        }
                        connection1.Close();
                        rdr2.Close();
                        listQuestions.Add(question);
                    }
                    rdr1.Close();
                    connection.Close();
                    aB.LQB.listQuestionBlocks.Add(new QuestionBlock()
                    {
                        //Id = int.Parse(rdr["id"].ToString()),
                        Title = rdr["name"].ToString(),
                        ShortName = String.IsNullOrEmpty(rdr["description"].ToString()) ? "" : char.ToUpper(rdr["description"].ToString()[0]) + rdr["description"].ToString().Substring(1),
                        listQuestions = listQuestions
                    });
                }
                rdr.Close();
                list.listAB.Add(aB);
            }
            
            rdrForCompany.Close();
            connectionForCompany.Close();

            return JsonConvert.SerializeObject(list);
        }

        private string CheckLogAndPass(MySqlDataReader rdr)
        {
            string answer = "";

            if (rdr.Read())
            {
                if (rdr.GetString("password") == this.password)
                    answer = "ok";
            }            

            rdr.Close();

            return answer;
        }
    }
}
