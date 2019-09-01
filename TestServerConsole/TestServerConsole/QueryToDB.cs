using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TestServerConsole
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
                Business b1 = new Business() { Id = Convert.ToInt32(rdr["id"]), Name = rdr["name"].ToString(), Description = rdr["description"].ToString() };
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
                    + "WHERE name = '" + this.nameCompanyForAnswers + "' AND (SELECT count(id) FROM questioning.answers "
                    + "WHERE idBusinesses=businesses.id AND idQuestions=(SELECT id FROM questioning.questions "
                    + "WHERE questions.idTaskType = (SELECT id FROM questioning.tasktype "
                    + "WHERE idDirections = (SELECT id FROM questioning.directions WHERE name = '" + this.nameDirectionForAnswers + "' LIMIT 1)LIMIT 1)LIMIT 1) ) > 0";
                connectionForCompany.Open();
                MySqlCommand cmd = new MySqlCommand(queryForCompany, connectionForCompany);
                rdrForCompany = cmd.ExecuteReader();
            }
            else
            {
                string queryForCompany = "SELECT name FROM questioning.businesses "
                    + "WHERE (SELECT count(id) FROM questioning.answers "
                    + "WHERE idBusinesses=businesses.id AND idQuestions=(SELECT id FROM questioning.questions "
                    + "WHERE questions.idTaskType = (SELECT id FROM questioning.tasktype "
                    + "WHERE idDirections = (SELECT id FROM questioning.directions WHERE name = '" + this.nameDirectionForAnswers + "' LIMIT 1)LIMIT 1)LIMIT 1) ) > 0";
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
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM questioning.questions WHERE idTaskType=" + rdr["id"].ToString(), connection);
                    MySqlDataReader rdr1 = cmd.ExecuteReader();

                    while (rdr1.Read())
                    {
                        Question question = new Question() { Id = Convert.ToInt32(rdr1["id"]), Text = rdr1["text"].ToString() };

                        MySqlDataReader rdr2;
                        MySqlConnection connection1 = new MySqlConnection(connStr);
                        connection1.Open();
                        MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM questioning.answers WHERE idQuestions=" + question.Id.ToString() + " AND idBusinesses = " +
                            "(SELECT id FROM questioning.businesses WHERE name = '" + rdrForCompany.GetString("name") + "')", connection1);
                        rdr2 = cmd1.ExecuteReader();

                        while (rdr2.Read())
                        {
                            question.listAnswers.Add(new Answer() { idBusinesses = rdr2.GetInt32("idBusinesses"), idQuestions = rdr2.GetInt32("idQuestions"), idVariantsOfAnswers = rdr2.GetInt32("idVariantsOfAnswers") });
                        }
                        connection1.Close();
                        rdr2.Close();
                        listQuestions.Add(question);
                    }
                    rdr1.Close();
                    connection.Close();
                    aB.LQB.listQuestionBlocks.Add(new QuestionBlock()
                    {
                        Id = int.Parse(rdr["id"].ToString()),
                        Title = rdr["name"].ToString(),
                        IdDirection = Convert.ToInt32(rdr["idDirections"]),
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
