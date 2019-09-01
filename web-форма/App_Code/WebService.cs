using Newtonsoft.Json;
using System.Web.Services;
using QuestioningLibrary;

/// <summary>
/// Сводное описание для WebService
/// </summary>
[WebService(Namespace = "http://tempuri1.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{
    QueryToDB q1 = new QueryToDB();
    string answerFromServer = "";

    public WebService()
    {

        //Раскомментируйте следующую строку в случае использования сконструированных компонентов 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string SendAnswersToServer(string jsonStr)
    {
        ListAnswers LA = JsonConvert.DeserializeObject<ListAnswers>(jsonStr);

        foreach (Answer answer in LA.listAnswers)
        {
            q1.Type = "INSERT";
            q1.Table = "answers";
            q1.Query = "INSERT INTO questioning.qanswers (idBusinesses, idQuestion, idVariantsOfAnswers) VALUES ( (SELECT id FROM questioning.businesses WHERE name = '" + answer.NameBusinesses + "'), '" + answer.idQuestions + "', '" + answer.idVariantsOfAnswers + "')";

            QueryToDB.SendQuery(q1);
        }       

        return "Анкета успешно отправлена. Спасибо за то, что уделили вермя на заполнение анкеты.";
    }

}
