using Newtonsoft.Json;
using QuestioningLibrary;
using QuestioningLibrary.Questionnaires;
using System.IO;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Сводное описание для QueryToDB
/// </summary>
public class QueryToDB
{
    public string Type { get; set; }
    public string Table { get; set; }
    public string Query { get; set; }


    // передача запроса серверу... Возвращает ответ (строка json или ok, если запрос не должен возвращать обьектов, то есть не select)
    public static string SendQuery(QueryToDB q1)
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

    // обработка ответа от сервера, если запрос SELECT
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

                case "Questionnaire":
                    ob = JsonConvert.DeserializeObject<Questionnaire>(answer);
                    break;
            }
        }

        return ob;
    }
}