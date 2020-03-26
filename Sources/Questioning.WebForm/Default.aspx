<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" lang="ru">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Анкетирование</title>
    <link rel="stylesheet" href="css/Style.css" />

    <script type="text/javascript">

        // При загрузки страницы
        window.onload = function () {
            
        }

        // при нажатии на кнопку отправления анкеты
        function btnClick(){

            var labelMess = document.getElementById("errorMess");
            var conteiner = document.getElementById("container_with_questions");
            var elements = conteiner.getElementsByTagName("input");
            var countCheckedRB = 0;
            var masCheckedRB = new Array();
            labelMess.textContent = "";


            // выбираем радиокнопки, у которых свойство checked = true
            for (var i = 0; i < elements.length; i++) {
                
                if (elements[i].type === "radio" && elements[i].checked) {
                    masCheckedRB[masCheckedRB.length] = elements[i];
                    countCheckedRB++;
                }
            }

            // выбираем все вопросы и проверяем, если у вопросов id совпадает с именем отвеченноой radioBtn
            // то на этот вопрос ответили и у этого вопроса убираем класс no_answer
            // иначе добавляем класс no_answer к вопросу
            var elements = conteiner.querySelectorAll(".textQ");

            for (var i = 0; i < elements.length; i++) {

                var flag = false;

                for (var j = 0; j < masCheckedRB.length; j++) {
                    if (elements[i].id === masCheckedRB[j].name) {
                        flag = true;
                        break;
                    }                        
                }
                
                if (!flag)
                    elements[i].classList.add("no_answer");
                else 
                    elements[i].classList.remove("no_answer");
            }

            if (elements.length != countCheckedRB) {
                labelMess.classList.remove("message");
                labelMess.classList.add("errorMess");
                labelMess.textContent = "Ответьте на все вопросы!";
            }
            else {
                var jsonStr = TransformObjectToJson(SaveAnswers(masCheckedRB));
                SendJsonToServer(jsonStr);
            }
            
        }

        // сохранение ответов в список обьектов (список является обьектом/классом),
        // который переводим в строку JSON
        function SaveAnswers(masCheckedRB) {

            var nameCompany = document.querySelector("#DDL_businesses option[selected='selected']").textContent;

            // список ответов
            var ListAnswers = {

                listAnswers: new Array(),

                // метод добавления ответов
                addAnswer: function (answer) {
                    this.listAnswers[this.listAnswers.length] = answer;
                }
            };

            for (var i = 0; i < masCheckedRB.length; i++) {

                var answer = {
                    idVariantsOfAnswers: masCheckedRB[i].parentNode.classList[0],
                    idQuestions: masCheckedRB[i].name,
                    nameBusinesses: nameCompany
                }

                ListAnswers.addAnswer(answer);
            }

            return ListAnswers;
        }

        // преобразование обьекта в JSON строку
        function TransformObjectToJson(object) {

            return JSON.stringify(object);
        }

        // отправка JSON строки на сервер
        function SendJsonToServer(jsonStr) {

            WebService.SendAnswersToServer(jsonStr, OnComplete, OnError);
        }

        // изменение выпадающий список с направлениями
        function DDL_directions_Change(result) {

            var label = document.getElementById("errorMess");
            label.className = "noneMess";
        }

        function OnComplete(result) {

            var label = document.getElementById("errorMess");
            label.classList.remove("errorMess");
            label.classList.add("message");
            label.textContent = result;

            var container = document.getElementById("container_with_questions");
            container.innerHTML = '';
        }

        function OnError(error) {
            alert(error);
        }

    </script>

</head>
<body>
    <header>
        <div class="logo">
            <img src="img/logo.png  "/>
            <h1>ПГУ им. Т.Г.Шевченко<br /> ИНЖЕНЕРНО-ТЕХНИЧЕСКИЙ ИНСТИТУТ</h1>
        </div>
        <div class="buttonExit"><a class="button">Выход</a></div>
        <div class="userName">Имя пользователя</div>        
    </header>
    <!-- Главная форма -->
    <form id="mainForm" class="main_form" runat="server" method="POST" autocomplete="on">
        <!-- информация для пользователя -->
        <div class="info_for_user">
            <h4>Уважаемый работодатель!</h4>
            <p>Приднестровский государственный университет им.Т.Г.Шевченко ведет подготовку специалистов квалификации «бакалавр» по направлению «Технологические машины и оборудование». Ваше предприятие является возможным заказчиком специалистов этого направления.</p>
            <p>Предлагаем Вам принять участие в анкетировании, целью которого является успешная адаптация молодых специалистов к профессиональной деятельности и закрепление их на предприятиях Приднестровкой Молдавской Республики.</p>
            <p>Просим Вас указать на каком уровне должен уметь самостоятельно решать профессиональные задачи молодой специалист в условиях Вашего производства, с учетом приоритетных направлений деятельности  предприятия на ближайший трехлетний период:</p>
            <ul>
                <li>уровень 1 – способен решать известные, не многофакторные задачи, не имеющие далеко идущих последствий, часто встречающиеся, требующие практического знания, известными способами, описанными в стандартах;</li>
                <li>уровень 2 – способен решать известные задачи, не имеющие далеко идущих последствий, часто встречающиеся, но имеющие множество конфликтующих ограничений, с несколькими группами заинтересованных сторон, зачастую способами, выходящими за рамки стандартов;</li>
                <li>уровень 3 – способен решать задачи, принадлежащие известному семейству задач, с множеством конфликтующих ограничений, с несколькими группами заинтересованных сторон, последствия которых могут превышать локальную важность, зачастую способами, выходящими за рамки стандартов.</li>
            </ul>
            <p>Результаты опроса создадут условия для оптимизации  педагогического процесса и помогут в принятии эффективных управленческих решений для повышения качества предоставляемых образовательных услуг</p>
        </div>
        <hr />
        
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/WebService.asmx" />
            </Services>
        </asp:ScriptManager>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <!-- Combobox-ы -->
                <div class="conditions">
                    <asp:Label CssClass="label" runat="server">Выберите отрасль вашего предприятия</asp:Label><br />
                    <asp:DropDownList CssClass="CB" ID="DDL_industry" runat="server" AutoPostBack="true" OnInit="DDL_industry_Init" OnSelectedIndexChanged="DDL_industry_SelectedIndexChanged">
                        <asp:ListItem>Отрасль 1</asp:ListItem>
                        <asp:ListItem>Отрасль 2</asp:ListItem>
                    </asp:DropDownList><br /><br />

                    <asp:Label CssClass="label" runat="server">Выберите предприятие</asp:Label><br />
                    <asp:DropDownList CssClass="CB" ID="DDL_businesses" runat="server" Enabled="false" AutoPostBack="true" OnInit="DDL_businesses_Init1" OnSelectedIndexChanged="DDL_businesses_SelectedIndexChanged" >
                        <asp:ListItem>Предприятие 1</asp:ListItem>
                        <asp:ListItem>Предприятие 2</asp:ListItem>
                    </asp:DropDownList><br /><br />

                    <asp:Label CssClass="label" runat="server">Выберите направление</asp:Label><br />
                    <asp:DropDownList CssClass="CB" ID="DDL_directions" runat="server" onchange="DDL_directions_Change()" Enabled="false" AutoPostBack="true" OnInit="DDL_directions_Init" OnSelectedIndexChanged="DDL_directions_SelectedIndexChanged">
                        <asp:ListItem>Направление 1</asp:ListItem>
                        <asp:ListItem>Направление 2</asp:ListItem>  
                    </asp:DropDownList>
                </div>
                <hr/>
                </ContentTemplate>
        </asp:UpdatePanel>


        <!-- контейнер для вопросов -->
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <Triggers>        
                <asp:AsyncPostBackTrigger ControlID="DDL_directions" EventName="SelectedIndexChanged" />    
            </Triggers>  
            <ContentTemplate>
                <asp:Panel runat="server" ID="container_with_questions" CssClass="container_with_questions">
                    <asp:Panel runat="server" CssClass="qB" Visible="false">
                        <asp:Label CssClass="titleQB" runat="server">Укажите, с каким уровнем сложности производственно-технологической задачи должен справиться молодой специалист на Вашем предприятии</asp:Label>
                        <div class="question" id="question1">
                            <asp:Label CssClass="textQ" runat="server">Контроль соблюдения экологической безопасности проведения работ</asp:Label>
                            <asp:Panel  CssClass="answers" runat="server">
                                <p>
                                    <input id="1_1" type="radio" name="question1"  value="Уровень 1"/>
                                    <label for="1_1">Уровень 1</label>
                                </p>
                                <p>
                                    <input  id="2_1" type="radio" name="question1"  value="Уровень 2"/>
                                    <label for="2_1">Уровень 2</label>
                                </p>
                                <p>
                                    <input  id="3_1" type="radio" name="question1"  value="Уровень 3"/>
                                    <label for="3_1">Уровень 3</label>
                                </p>
                            </asp:Panel>
                        </div>

                        <div class="question" id="question2">
                            <asp:Label CssClass="textQ" runat="server">Планирование работ персонала и фондов оплаты труда и разработка планов работы первичных производственных подразделений</asp:Label>
                            <asp:Panel runat="server" CssClass="answers">
                                <p>
                                    <input id="1_2" type="radio" name="question2" value="Уровень 1"/>
                                    <label for="1_2">Уровень 1</label>
                                </p>
                                <p>
                                    <input id="2_2" type="radio" name="question2" value="Уровень 2"/>
                                    <label for="2_2">Уровень 2</label>
                                </p>
                                <p>
                                    <input id="3_2" type="radio" name="question2" value="Уровень 3"/>
                                    <label for="3_2">Уровень 3</label>
                                </p>
                            </asp:Panel>
                        </div>
                    </asp:Panel>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:Label runat="server" ID="errorMess" CssClass="noneMess" Text="Сообщение об ошибке!"></asp:Label> 
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <Triggers>        
                <asp:AsyncPostBackTrigger ControlID="btn_send" EventName="Click" />    
                <asp:AsyncPostBackTrigger ControlID="DDL_directions" EventName="SelectedIndexChanged" />    
            </Triggers>   
            <ContentTemplate>
                <asp:Button runat="server" ID="btn_send" CssClass="btn_send" Text="ОТПРАВИТЬ" Visible="false" OnClientClick="btnClick()" />
            </ContentTemplate>
         </asp:UpdatePanel>
    </form>
</body>
</html>
