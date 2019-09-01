using QuestioningLibrary;
using QuestioningLibrary.Questionnaires;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    QueryToDB q1 = new QueryToDB();
    string answer = "";
    List<RadioButton> listAnswers = new List<RadioButton>();
    Dictionary<Label, bool> dicQuestions = new Dictionary<Label, bool>(); 

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    // загрузка информацие конбобокса со структурными Подразделениями
    private void Load_Info_In_DDL_industry()
    {
        q1.Type = "SELECT";
        q1.Table = "Industry";
        q1.Query = "SELECT * FROM questioning.industry";

        answer = QueryToDB.SendQuery(q1);
        ListIndustry LInd = (ListIndustry)QueryToDB.ProcessResponse(answer, q1);

        DDL_industry.Items.Clear();

        DDL_industry.Items.Add("");
        foreach (Industry Ind in LInd.listIndustry)
            DDL_industry.Items.Add(Ind.Name);
    }

    // загрузка информацие конбобокса с Предприятиями
    private void Load_Info_In_DDL_businesses()
    {
        string nameIndustry = DDL_industry.SelectedValue;
        q1.Type = "SELECT";
        q1.Table = "Businesses";
        if (nameIndustry != "")
            q1.Query = "SELECT * FROM questioning.businesses WHERE idIndustry = (SELECT id FROM questioning.industry WHERE name = '" + nameIndustry + "')";
        else
            q1.Query = "SELECT * FROM questioning.businesses";

        answer = QueryToDB.SendQuery(q1);
        ListBusinesses LB = (ListBusinesses)QueryToDB.ProcessResponse(answer, q1);

        DDL_businesses.Items.Clear();

        DDL_businesses.Items.Add("");
        foreach (Business b in LB.listBusinesses)
            DDL_businesses.Items.Add(b.Name);
    }

    // загрузка информацие конбобокса с Направлениями
    private void Load_Info_In_DDL_directions()
    {
        q1.Type = "SELECT";
        q1.Table = "Directions";
        q1.Query = "SELECT * FROM questioning.Directions WHERE isCreatedQuestioning = 1";

        answer = QueryToDB.SendQuery(q1);
        ListDirections LD = (ListDirections)QueryToDB.ProcessResponse(answer, q1);

        DDL_directions.Items.Clear();

        DDL_directions.Items.Add("");
        foreach (Direction d in LD.listDirections)
            DDL_directions.Items.Add(d.Name);
    }

    protected void DDL_industry_Init(object sender, EventArgs e)
    {
        Load_Info_In_DDL_industry();
    }

    protected void DDL_businesses_Init1(object sender, EventArgs e)
    {
        Load_Info_In_DDL_businesses();
    }

    protected void DDL_directions_Init(object sender, EventArgs e)
    {
        Load_Info_In_DDL_directions();
    }

    // выбор отрасли предприятия
    protected void DDL_industry_SelectedIndexChanged(object sender, EventArgs e)
    {
        DDL_businesses.Enabled = true;
        Load_Info_In_DDL_businesses();
    }

    // выбор предприятия
    protected void DDL_businesses_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDL_businesses.SelectedValue == "")
        {
            DDL_directions.Enabled = false;
            DDL_directions.SelectedValue = ""; 
        }
        else
            DDL_directions.Enabled = true;


    }

    // выбор направления - Прогрузка анкеты
    protected void DDL_directions_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDL_directions.SelectedValue == "")
        {
            btn_send.Visible = false;
        }
        else
            btn_send.Visible = true;
        errorMess.CssClass += " noneMess";
        Load_QB();
    }

    private void Load_QB()
    {
        if (DDL_directions.SelectedValue != "")
        {
            q1.Type = "SELECT";
            q1.Table = "Questionnaire";
            q1.Query = "SELECT * FROM questioning.questionnaires WHERE direction = '" + DDL_directions.SelectedValue + "'";

            answer = QueryToDB.SendQuery(q1);

            Questionnaire questionnaire = (Questionnaire)QueryToDB.ProcessResponse(answer, q1);
            container_with_questions.Controls.Clear();
            dicQuestions.Clear();

            foreach (QQuestionBlock questionBlock in questionnaire.ListQQuestionBlocks)
            {
                Panel p1 = new Panel() { CssClass = "qB" };
                p1.Controls.Add(new Label() { CssClass = "titleQB", Text = questionBlock.Title});
                foreach (QQuestion q in questionBlock.ListQQuestions)
                {
                    Panel question = new Panel() { CssClass = "question", ID = "question_" + q.Id };
                    Label textQuestion = new Label() { ID = q.Id.ToString(), CssClass = "textQ", Text = q.Text };
                    question.Controls.Add(textQuestion); dicQuestions.Add(textQuestion, false);

                    Panel rad1 = new Panel() { CssClass = "answer" };
                    Panel rad2 = new Panel() { CssClass = "answer" };
                    Panel rad3 = new Panel() { CssClass = "answer" };

                    RadioButton rb = new RadioButton() { GroupName = q.Id.ToString(), Text = "Уровень 1", CssClass = "1" };
                    listAnswers.Add(rb);
                    rad1.Controls.Add(rb); 

                    rb = new RadioButton() { GroupName = q.Id.ToString(), Text = "Уровень 2", CssClass = "2" };
                    listAnswers.Add(rb);
                    rad2.Controls.Add(rb); 

                    rb = new RadioButton() { GroupName = q.Id.ToString(), Text = "Уровень 3", CssClass = "3" };
                    listAnswers.Add(rb);
                    rad3.Controls.Add(rb); 

                    question.Controls.Add(rad1); question.Controls.Add(rad2); question.Controls.Add(rad3);
                    p1.Controls.Add(question);
                    container_with_questions.Controls.Add(p1);
                }
            }
            btn_send.Visible = true;

        }
        else
            container_with_questions.Controls.Clear();
    }
}