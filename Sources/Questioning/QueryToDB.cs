using Newtonsoft.Json;
using QuestioningLibrary;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Questioning
{
    public class QueryToDB
    {
        public string Type { get; set; }
        public string Table { get; set; }
        public string Query { get; set; }
        public string nameCompanyForAnswers { get; set; } // условие для вывода Ответов
        public string nameDirectionForAnswers { get; set; } // условие для вывода Ответов
        public string password { get; set; } // пароль

        // передача запроса серверу... Возвращает ответ (строка json или ok, если запрос не должен возвращать обьектов, то есть не select)
        public static string SendQuery(QueryToDB q1)
        {
            try
            {
                string jsonData = "";
                string answer = "";

                jsonData = JsonConvert.SerializeObject(q1);

                TcpClient client = new TcpClient();
                client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12000));

                StreamWriter sw = new StreamWriter(client.GetStream());
                sw.AutoFlush = true;
                sw.WriteLine(jsonData);

                StreamReader sr = new StreamReader(client.GetStream());
                answer = sr.ReadLine();

                client.Close();

                return answer;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // обработка ответа от сервера
        public static object ProcessResponse(string answer, QueryToDB q1)
        {
            object ob = null;

            if (answer != "ok")
            {
                switch (q1.Table)
                {
                    case "QuestionBlocks":
                        ob = JsonConvert.DeserializeObject<ListQuestionBlocks>(answer);
                        break;

                    case "Businesses":
                        ob = JsonConvert.DeserializeObject<ListBusinesses>(answer);
                        break;

                    case "Directions":
                        ob = JsonConvert.DeserializeObject<ListDirections>(answer);
                        break;

                    case "Industry":
                        ob = JsonConvert.DeserializeObject<ListIndustry>(answer);
                        break;

                    case "Answers":
                        ob = JsonConvert.DeserializeObject<ListAnswersBlock>(answer);
                        break;
                }
            }

            return ob;
        }
    }
}
