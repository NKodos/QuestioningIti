using QuestioningLibrary;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace questioningITI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml 
    /// </summary>
    public partial class MainWindow
    {
        private QueryToDB _query = new QueryToDB();
        private string _answer = "";
        private RequestHandler _requestHandler = new RequestHandler();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Нажатие на вкладку Верхнего уровня
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            tabControl_1.SelectedIndex = index;
            GridCursor.Margin = new Thickness(10 + 150 * index, 45, 0, 0);

            switch (index)
            {
                case 0:
                    //LoadQuestionnairesView();
                    break;

                case 1:
                    _query.Type = "SELECT";
                    _query.Table = "QuestionBlocks";
                    _query.Query = "SELECT * FROM questioning.tasktype";
                    LoadQuestionBlockesView();
                    break;

                case 2:
                    _query.Type = "SELECT";
                    _query.Table = "Businesses";
                    _query.Query = "SELECT * FROM questioning.businesses";
                    LoadCompanyView();
                    break;

                case 3:
                    _query.Type = "SELECT";
                    _query.Table = "Directions";
                    _query.Query = "SELECT * FROM questioning.directions";
                    LoadDirectionsView();
                    break;
            }

            LoadInfoInComboboxes(); // загрузка combobox-ов
        }

        // Нажатие на вкладку Нижнего уровня > Анкеты
        private void Button_Click_2_Questioning(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            tabControl_2.SelectedIndex = index;
            GridCursor_2.Margin = new Thickness(10 + 150 * index, 0, 0, 0);

            switch (index)
            {
                // Вопросы
                case 0:
                    _query.Type = "SELECT";
                    _query.Table = "QuestionBlocks";
                    _query.Query = "SELECT * FROM questioning.tasktype";
                    LoadQuestionBlockesView();
                    break;

                // Редактор анкет
                case 1:
                    LoadQuestionnairesEditor();
                    break;

                // Ответы
                case 2:
                    LoadAnswers();
                    break;
            }
            cbDirInQuestionBlockesView.SelectedValue = null;
        }

        // Нажатие на вкладку Нижнего уровня > Предприятия
        private void Button_Click_2_Businesses(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            tabControl_Businesses.SelectedIndex = index;
            GridCursorBusinesses.Margin = new Thickness(10 + 150 * index, 0, 0, 0);            
        }

        // Нажатие на вкладку Нижнего уровня > Направления
        private void Button_Click_2_Directions(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            tabControl_Directions.SelectedIndex = index;
            GridCursorDirections.Margin = new Thickness(10 + 150 * index, 0, 0, 0);
            _query.Type = "SELECT";
            _query.Table = "Directions";
            _query.Query = "SELECT * FROM questioning.directions";
            LoadDirectionsView();
        }

        // Нажатие на кнопку Редактирать в Анкета > Просмотр
        private void Button_Click_Edit_Task(object sender, RoutedEventArgs e)
        {
            int i = 0;
            tabControl_2.SelectedIndex = 4;

            string idQB = ((Button)e.Source).Uid; // сохраняем id Блока Вопросов
            btnSaveQuestioning.Uid = idQB;

            _query.Type = "SELECT";
            _query.Table = "QuestionBlocks";
            _query.Query = "SELECT * FROM questioning.tasktype WHERE id = '" + idQB + "'";
            _answer = QueryToDB.SendQuery(_query);
            ListQuestionBlocks LQB = (ListQuestionBlocks)QueryToDB.ProcessResponse(_answer, _query);
            tbNameEditQB.Text = LQB.listQuestionBlocks[0].Title;
            tbShortNameEditQB.Text = LQB.listQuestionBlocks[0].ShortName;

            _query.Type = "SELECT";
            _query.Table = "Directions";
            _query.Query = "SELECT * FROM questioning.directions WHERE id = '" + LQB.listQuestionBlocks[0].IdDirection + "'";
            _answer = QueryToDB.SendQuery(_query);
            ListDirections LD = (ListDirections)QueryToDB.ProcessResponse(_answer, _query);
            cbDirInQBEdit.SelectedValue = LD.listDirections[0].Name;
            QuestionBlockesEditContainer.Children.Clear();
            NewQuestionsEditContainer.Children.Clear();
            TextBox tboxNewQuestion = new TextBox() { FontSize = 16, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(50, 0, 85, 10), BorderThickness = new Thickness(0, 0, 0, 2), BorderBrush = Brushes.LightGray };
            tboxNewQuestion.LostFocus += new RoutedEventHandler(TB_NewQuestion_LostFocus);
            NewQuestionsEditContainer.Children.Add(tboxNewQuestion);

            foreach (Question q in LQB.listQuestionBlocks[0].listQuestions)
            {
                i++;
                Grid el1 = new Grid() { Margin = new Thickness(0, 0, 0, 20) };
                el1.Children.Add(new TextBlock() { Text = i + ". ", FontSize = 16, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(30, 0, 30, 0) });
                TextBox el2 = new TextBox() { Margin = new Thickness(50, 0, 85, 0), FontSize = 16, VerticalAlignment = VerticalAlignment.Center, Text = q.Text, Uid = q.Id.ToString()};
                el2.LostFocus += new RoutedEventHandler(TextBox_LostFocus);
                el1.Children.Add(el2);

                Button el3 = new Button()
                {
                    Content = "X",
                    FontSize = 20,
                    Foreground = (Brush)(new BrushConverter().ConvertFrom("#FFA2A2A2")),
                    Width = 30,
                    Padding = new Thickness(0),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5, 0, 40, 0),
                    Background = null,
                    BorderBrush = null,
                    Uid = q.Id.ToString()
                };
                el3.Click += new RoutedEventHandler(Button_Click_Del_Question);
                el1.Children.Add(el3);
                QuestionBlockesEditContainer.Children.Add(el1);
            }
        }

        // Нажатие на кнопку Редактирать в Предприятие > Просмотр
        private void Button_Click_Edit_Businesses(object sender, RoutedEventArgs e)
        {
            tabControl_Businesses.SelectedIndex = 2;
            string idCompany = ((Button)e.Source).Uid;
            btnSaveCompany.Uid = idCompany;

            _query.Type = "SELECT";
            _query.Table = "Businesses";
            _query.Query = "SELECT * FROM questioning.businesses WHERE id = '" + idCompany + "'";
            _answer = QueryToDB.SendQuery(_query);
            var listBusinesses = (ListBusinesses)QueryToDB.ProcessResponse(_answer, _query);
            tbNameEditCompany.Text = listBusinesses.listBusinesses[0].Name;
            cbIndustryInCompanyEdit.SelectedValue = listBusinesses.listBusinesses[0].Industry;
            tbDescriptionEditCompany.Text = listBusinesses.listBusinesses[0].Description;

        }

        // Нажатие на кнопку Редактирать в Направления > Просмотр
        private void Button_Click_Edit_Directions(object sender, RoutedEventArgs e)
        {
            tabControl_Directions.SelectedIndex = 2;
            string idDirection = ((Button)e.Source).Uid;
            btnSaveDirection.Uid = idDirection;

            _query.Type = "SELECT";
            _query.Table = "Directions";
            _query.Query = "SELECT * FROM questioning.Directions WHERE id = '" + idDirection + "'";
            _answer = QueryToDB.SendQuery(_query);
            ListDirections LB = (ListDirections)QueryToDB.ProcessResponse(_answer, _query);
            tbNameEditDirection.Text = LB.listDirections[0].Name;
            tbDescriptionEditDirection.Text = LB.listDirections[0].Description;
        }

        // Перенести курсор в начало, когда снимается фокус с TextBox
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBx = (TextBox)e.Source;
            txtBx.Select(0, 0);
        }

        // Событие - удаление старого TextBox'а, при снятии фокута
        private void TB_RemoveTB_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBx = (TextBox)e.Source;

            if (txtBx.Text.Replace(" ", "") == "")
            {
                ((StackPanel)txtBx.Parent).Children.Remove(txtBx);
            }
        }

        // Событие - cоздание нового TextBox, при написании нового вопроса
        private void TB_NewQuestion_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBx = (TextBox)e.Source;
            txtBx.Select(0, 0);
            if (txtBx.Text.Replace(" ", "") != "")
            {                
                TextBox tboxNewQuestion = new TextBox() { FontSize = 16, VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(50, 0, 85, 10), BorderThickness = new Thickness(0, 0, 0, 2), BorderBrush = Brushes.LightGray };
                tboxNewQuestion.LostFocus += new RoutedEventHandler(TB_NewQuestion_LostFocus);
                ((StackPanel)txtBx.Parent).Children.Add(tboxNewQuestion);
                txtBx.LostFocus -= TB_NewQuestion_LostFocus;
                txtBx.LostFocus += new RoutedEventHandler(TextBox_LostFocus);
                txtBx.LostFocus += new RoutedEventHandler(TB_RemoveTB_LostFocus);
            }
        }
        
        // Нажатие на кнопку Отмена в Анкета > Редактирование
        private void btnCancelEditQuestioning_Click(object sender, RoutedEventArgs e)
        {
            tabControl_2.SelectedIndex = 0;
            GridCursor_2.Margin = new Thickness(10 + 150 * 0, 0, 0, 0);
        }

        // Нажатие на кнопку Отмена в Предприятие > Редактирование
        private void btnCancelEditBusinesses_Click(object sender, RoutedEventArgs e)
        {
            tabControl_Businesses.SelectedIndex = 0;
            GridCursorBusinesses.Margin = new Thickness(10 + 150 * 0, 0, 0, 0);
        }        

        // Нажатие на кнопку Отмена в Предприятие > Редактирование
        private void btnCancelEditDirections_Click(object sender, RoutedEventArgs e)
        {
            tabControl_Directions.SelectedIndex = 0;
            GridCursorDirections.Margin = new Thickness(10 + 150 * 0, 0, 0, 0);
        }
        
        // Загрузка информацией Анкета > Просмотр
        private void LoadQuestionBlockesView()
        {
            
            _answer = QueryToDB.SendQuery(_query); // отправка запроса и получение ответа от сервера

            ListQuestionBlocks LQB = (ListQuestionBlocks)QueryToDB.ProcessResponse(_answer, _query); // обработка ответа от сервера (получаем екземпляр класса)

            QuestionBlockesViewContainer.Children.Clear();
            // заполнение информацией
            foreach (QuestionBlock questionBlock in LQB.listQuestionBlocks)
            {
                int i = 0;
                StackPanel el1 = new StackPanel() { Margin = new Thickness(20, 15, 0, 0), Background = Brushes.White };

                el1.Children.Add( new Label() {
                    FontWeight = FontWeights.Bold,
                    Foreground = (Brush)(new BrushConverter().ConvertFrom("#303030")),
                    Content = "Заголовок задачи:" });

                el1.Children.Add(new TextBlock() {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 0, 10, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    FontSize = 16,
                    Text = questionBlock.Title});

                el1.Children.Add(new Label()
                {
                    FontWeight = FontWeights.Bold,
                    Foreground = (Brush)(new BrushConverter().ConvertFrom("#303030")),
                    Content = "Краткое название:"
                });

                el1.Children.Add(new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 0, 10, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    FontSize = 16,
                    Text = String.IsNullOrEmpty(questionBlock.ShortName) ? "-" : questionBlock.ShortName
                });

                el1.Children.Add(new Label()
                {
                    FontWeight = FontWeights.Bold,
                    Foreground = (Brush)(new BrushConverter().ConvertFrom("#303030")),
                    Content = "Вопросы:"
                });

                foreach (Question q in questionBlock.listQuestions)
                {
                    i++;
                    StackPanel el2 = new StackPanel();
                    el2.Children.Add(new TextBlock() {
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(30, 10, 30, 0),
                        FontSize = 16,
                        Text = i.ToString() + ". " +q.Text
                    });

                    RadioButton rb1 = new RadioButton() {
                        IsEnabled = false,
                        BorderThickness = new Thickness(4),
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(20, 0, -10, 0),
                        Content = "Уровень 1"};
                    RadioButton rb2 = new RadioButton()
                    {
                        IsEnabled = false,
                        BorderThickness = new Thickness(4),
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(20, 0, -10, 0),
                        Content = "Уровень 2"
                    };
                    RadioButton rb3 = new RadioButton()
                    {
                        IsEnabled = false,
                        BorderThickness = new Thickness(4),
                        VerticalContentAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(20, 0, -10, 0),
                        Content = "Уровень 3"
                    };
                    WrapPanel el3 = new WrapPanel() { Margin = new Thickness(30, 10, 0, 10) };
                    el3.Children.Add(rb1); el3.Children.Add(rb2); el3.Children.Add(rb3);
                    el2.Children.Add(el3);
                    el1.Children.Add(el2);
                }

                WrapPanel el4 = new WrapPanel();

                Button b1 = new Button() {
                    Content = "Редактировать",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(20, 5, -10, 10),
                    Padding = new Thickness(10, 5, 10, 5),
                    Background = (Brush)(new BrushConverter().ConvertFrom("#0067a2")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(0),
                    Uid = questionBlock.Id.ToString()
                };
                b1.Click += new RoutedEventHandler(Button_Click_Edit_Task);

                Button b2 = new Button()
                {
                    Content = "Удалить",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(20, 5, -10, 10),
                    Padding = new Thickness(10, 5, 10, 5),
                    Background = (Brush)(new BrushConverter().ConvertFrom("#0067a2")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(0),
                    Uid = questionBlock.Id.ToString()
                };
                b2.Click += new RoutedEventHandler(Button_Click_Del_Task);
                el4.Children.Add(b1); el4.Children.Add(b2);
                el1.Children.Add(el4);

                QuestionBlockesViewContainer.Children.Add(el1);
            }   
        }

        // Загрузка Анкеты > Редактор анкет
        private void LoadQuestionnairesEditor()
        {
            // Комбобокс
            _query.Type = "SELECT";
            _query.Table = "Directions";
            _query.Query = "SELECT * FROM questioning.Directions WHERE isCreatedQuestioning = 0";

            _answer = QueryToDB.SendQuery(_query);
            ListDirections LD = (ListDirections)QueryToDB.ProcessResponse(_answer, _query);

            cbDirInEditor.Items.Clear();

            foreach (Direction d in LD.listDirections)
            {
                cbDirInEditor.Items.Add(d.Name);
            }

            // Листбокс

            _query.Type = "SELECT";
            _query.Table = "Businesses";
            _query.Query = "SELECT * FROM questioning.businesses";

            _answer = QueryToDB.SendQuery(_query);
            ListBusinesses LB = (ListBusinesses)QueryToDB.ProcessResponse(_answer, _query);

            listBoxBusinessLeft.Items.Clear();
            listBoxBusinessRight.Items.Clear();

            foreach (Business b in LB.listBusinesses)
            {
                if (!String.IsNullOrEmpty(b.Email))
                {
                    ListBoxItem lBI = new ListBoxItem() { Content = b.Name };
                    lBI.MouseDoubleClick += new MouseButtonEventHandler(listBoxItemBusinessLeft_MouseDoubleClick);
                    listBoxBusinessLeft.Items.Add(lBI);
                }
            }
        }

        // Загрузка информацией Анкеты > Ответы
        private void LoadAnswers()
        {
            AnswersBlockViewContainer.Children.Clear();

            string conditionForQuery = "";

            if (cbDirInAnswers.SelectedValue != null)
            {
                if (cbCompanyInAnswers.SelectedValue == null || cbCompanyInAnswers.SelectedValue.ToString() == "Любое")
                    _query.nameCompanyForAnswers = null;
                else
                {
                    _query.nameCompanyForAnswers = cbCompanyInAnswers.SelectedValue.ToString();
                    conditionForQuery = "AND idBusinesses = (SELECT id FROM questioning.businesses WHERE name='" + _query.nameCompanyForAnswers + "')";
                }
                _query.nameDirectionForAnswers = cbDirInAnswers.SelectedValue.ToString();

                _query.Type = "SELECT";
                _query.Table = "Answers";
                _query.Query = "SELECT * FROM questioning.qquestionsblocks WHERE idQuestionnaire = ( SELECT id FROM questioning.questionnaires WHERE direction = '" + cbDirInAnswers.SelectedValue + "')";

                _answer = QueryToDB.SendQuery(_query);

                ListAnswersBlock LAB = (ListAnswersBlock)QueryToDB.ProcessResponse(_answer, _query);

                // ходим по блокам всех ответов
                foreach (AnswersBlock aB in LAB.listAB)
                {
                    StackPanel el1 = new StackPanel() { Margin = new Thickness(20, 15, 0, 0), Background = Brushes.White, Uid = aB.nameCompany};

                    el1.Children.Add(new TextBlock()
                    {
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(10, 0, 10, 0),
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        FontSize = 18,
                        Text = aB.nameCompany
                    });

                    Button b1 = new Button()
                    {
                        Content = "Очистить",
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Margin = new Thickness(20, 5, -10, 10),
                        Padding = new Thickness(10, 5, 10, 5),
                        Background = (Brush)(new BrushConverter().ConvertFrom("#0067a2")),
                        Foreground = Brushes.White,
                        FontWeight = FontWeights.Light,
                        BorderThickness = new Thickness(0)
                    };

                    // ходим по блокам вопросов
                    foreach (QuestionBlock QB in aB.LQB.listQuestionBlocks)
                    {
                        StackPanel el2 = new StackPanel() { Margin = new Thickness(20, 15, 0, 0), Background = Brushes.White };

                        el2.Children.Add(new Label()
                        {
                            FontWeight = FontWeights.Bold,
                            Foreground = (Brush)(new BrushConverter().ConvertFrom("#303030")),
                            Content = "Заголовок задачи:"
                        });

                        el2.Children.Add(new TextBlock()
                        {
                            TextWrapping = TextWrapping.Wrap,
                            Margin = new Thickness(10, 0, 10, 0),
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            FontSize = 16,
                            Text = QB.Title
                        });

                        el2.Children.Add(new Label()
                        {
                            FontWeight = FontWeights.Bold,
                            Foreground = (Brush)(new BrushConverter().ConvertFrom("#303030")),
                            Content = "Вопросы:"
                        });

                        

                        int i = 1;
                        // ходим по вопросам
                        foreach (Question question in QB.listQuestions)
                        {
                            StackPanel el3 = new StackPanel();
                            el3.Children.Add(new TextBlock()
                            {
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(30, 10, 30, 0),
                                FontSize = 16,
                                Text = i.ToString() + ". " + question.Text
                            });

                            i++;

                            el3.Children.Add(new TextBlock()
                            {
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(30, 10, 30, 0),
                                FontSize = 16,
                                FontStyle = FontStyles.Italic,
                                Text = "Ответы:"
                            });

                            TextBox tbAnswers = new TextBox()
                            {
                                TextWrapping = TextWrapping.Wrap,
                                Margin = new Thickness(60, 0, 60, 0),
                                FontSize = 16,
                                FontStyle = FontStyles.Italic,
                                Text = ""
                            };

                            el3.Children.Add(tbAnswers);

                            foreach(Answer answer in question.listAnswers)
                            {
                                tbAnswers.Text += answer.idVariantsOfAnswers + " ";
                            }

                            b1.Uid += question.Id + ";";

                            el2.Children.Add(el3);
                        }
                        el1.Children.Add(el2);
                    }

                    b1.Click += new RoutedEventHandler(Button_Click_Del_Answers);
                    el1.Children.Add(b1);
                    AnswersBlockViewContainer.Children.Add(el1);
                }
            }
        }

        // Загрузка информацией Предприятия > Просмотр
        private void LoadCompanyView()
        {
            int i = 0;
            _answer = QueryToDB.SendQuery(_query); // отправка запроса и получение ответа от сервера

            ListBusinesses LB = (ListBusinesses)QueryToDB.ProcessResponse(_answer, _query); // обработка ответа от сервера (получаем екземпляр класса)
            
            CompanyViewContainer.Children.Clear();

            foreach (Business b in LB.listBusinesses)
            {
                i++;
                StackPanel el1 = new StackPanel() { Margin = new Thickness(50, 15, 50, 15), Background = Brushes.White };

                el1.Children.Add(new TextBlock()
                {
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 5),
                    Text = "Название:"
                });

                el1.Children.Add(new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 0, 0, 10),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    FontSize = 16,
                    Text = i + ". " + b.Name
                });

                el1.Children.Add(new TextBlock()
                {
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 5),
                    Text = "Отрасль предприятия:"
                });

                el1.Children.Add(new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 0, 10, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    FontSize = 16,
                    Text = b.Industry
                });

                el1.Children.Add(new TextBlock()
                {
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 5),
                    Text = "Email:"
                });

                el1.Children.Add(new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 0, 10, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    FontSize = 16,
                    Text = String.IsNullOrEmpty(b.Email) ? "-" : b.Email
                });

                el1.Children.Add(new TextBlock()
                {
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(0, 10, 0, 5),
                    Text = "Описание:"
                });

                el1.Children.Add(new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 0, 10, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    FontSize = 14,
                    Text = String.IsNullOrEmpty(b.Description) ? "-" : b.Description
                });

                WrapPanel el2 = new WrapPanel() { Margin = new Thickness(0, 10, 0, 0) };

                Button b1 = new Button()
                {
                    Content = "Редактировать",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(20, 5, -10, 10),
                    Padding = new Thickness(10, 5, 10, 5),
                    Background = (Brush)(new BrushConverter().ConvertFrom("#0067a2")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(0),
                    Uid = b.Id.ToString()
                };
                b1.Click += new RoutedEventHandler(Button_Click_Edit_Businesses);

                Button b2 = new Button()
                {
                    Content = "Удалить",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(20, 5, -10, 10),
                    Padding = new Thickness(10, 5, 10, 5),
                    Background = (Brush)(new BrushConverter().ConvertFrom("#0067a2")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(0),
                    Uid = b.Id.ToString()
                };
                b2.Click += new RoutedEventHandler(Button_Click_Del_Company);
                el2.Children.Add(b1); el2.Children.Add(b2);
                el1.Children.Add(el2);
                CompanyViewContainer.Children.Add(el1);
            }
        }

        // Загрузка информацией Направления > Просмотр
        private void LoadDirectionsView()
        {
            int i = 0;
            _answer = QueryToDB.SendQuery(_query); // отправка запроса и получение ответа от сервера

            ListDirections LD = (ListDirections)QueryToDB.ProcessResponse(_answer, _query); // обработка ответа от сервера (получаем екземпляр класса)

            DirectionsViewContainer.Children.Clear();

            foreach (Direction d in LD.listDirections)
            {
                i++;
                StackPanel el1 = new StackPanel() { Margin = new Thickness(50, 15, 50, 15), Background = Brushes.White };

                el1.Children.Add(new TextBlock()
                {
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(0, 0, 0, 5),
                    Text = "Название:"
                });

                el1.Children.Add(new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 0, 0, 10),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    FontSize = 16,
                    Text = i + ". " + d.Name
                });

                el1.Children.Add(new TextBlock()
                {
                    FontStyle = FontStyles.Italic,
                    Margin = new Thickness(0, 0, 0, 5),
                    Text = "Описание:"
                });

                el1.Children.Add(new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(10, 0, 10, 0),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    FontSize = 16,
                    Text = d.Description
                });

                WrapPanel el2 = new WrapPanel() { Margin = new Thickness(0, 10, 0, 0) };

                Button b1 = new Button()
                {
                    Content = "Редактировать",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(20, 5, -10, 10),
                    Padding = new Thickness(10, 5, 10, 5),
                    Background = (Brush)(new BrushConverter().ConvertFrom("#0067a2")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(0),
                    Uid = d.Id.ToString()
                };
                b1.Click += new RoutedEventHandler(Button_Click_Edit_Directions);

                Button b2 = new Button()
                {
                    Content = "Удалить",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(20, 5, -10, 10),
                    Padding = new Thickness(10, 5, 10, 5),
                    Background = (Brush)(new BrushConverter().ConvertFrom("#0067a2")),
                    Foreground = Brushes.White,
                    FontWeight = FontWeights.Light,
                    BorderThickness = new Thickness(0),
                    Uid = d.Id.ToString()
                };
                b2.Click += new RoutedEventHandler(Button_Click_Del_Directions);
                el2.Children.Add(b1); el2.Children.Add(b2);
                el1.Children.Add(el2);
                DirectionsViewContainer.Children.Add(el1);
            }
        }

        // Нажатие на кнопку Удалить в Анкета > Просмотр
        private void Button_Click_Del_Task(object sender, RoutedEventArgs e)
        {
            string idQB = ((Button)e.Source).Uid;

            MessageBoxResult result = MessageBox.Show("Удалить?", "Удаление Блока Вопросов", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                _query.Type = "DELETE";
                _query.Table = "tasktype";
                _query.Query = "DELETE FROM questioning.tasktype WHERE id='" + idQB + "'";
                QueryToDB.SendQuery(_query);

                _query.Type = "DELETE";
                _query.Table = "Questions";
                _query.Query = "DELETE FROM questioning.questions WHERE idTaskType='" + idQB + "'";
                QueryToDB.SendQuery(_query);

                _query.Type = "SELECT";
                _query.Table = "QuestionBlocks";
                _query.Query = "SELECT * FROM questioning.tasktype";
                LoadQuestionBlockesView();
            }
        }

        // Нажатие на кнопку Очистить в Анкета > Ответы
        private void Button_Click_Del_Answers(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Удалить ответы?", "Удаление ответов", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string idQuestions = ((Button)e.Source).Uid;
                string nameCompany = ((StackPanel)(((Button)e.Source).Parent)).Uid;
                string[] idMass = idQuestions.Split(';');

                for (int i = 0; i < idMass.Length - 1; i++)
                {
                    _query.Type = "DELETE";
                    _query.Table = "qanswers";
                    _query.Query = "DELETE FROM questioning.qanswers WHERE idQuestion=" + idMass[i] +
                        " AND idBusinesses = (SELECT id FROM questioning.businesses WHERE name='" + nameCompany + "')";
                    QueryToDB.SendQuery(_query);
                }

                LoadAnswers();
            }
        }

        // Нажатие на кнопку Удаления вопроса в Анкета > Редактирование
        private void Button_Click_Del_Question(object sender, RoutedEventArgs e)
        {
            string idQ = ((Button)e.Source).Uid;

            MessageBoxResult result = MessageBox.Show("Удалить вопрос?", "Удаление Вопроса", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _query.Type = "DELETE";
                _query.Table = "Questions";
                _query.Query = "DELETE FROM questioning.questions WHERE id='" + idQ + "'";
                QueryToDB.SendQuery(_query);

                ((Grid)(((Button)e.Source).Parent)).IsEnabled = false;
            }
        }

        // Нажатие на кнопку Удалить в Предприятия > Редактирование
        private void Button_Click_Del_Company(object sender, RoutedEventArgs e)
        {
            string idCompany = ((Button)e.Source).Uid;

            MessageBoxResult result = MessageBox.Show("Удалить Предприятие?", "Удаление Предприятия", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _query.Type = "DELETE";
                _query.Table = "Businesses";
                _query.Query = "DELETE FROM questioning.Businesses WHERE id='" + idCompany + "'";
                QueryToDB.SendQuery(_query);

                _query.Type = "SELECT";
                _query.Table = "Businesses";
                _query.Query = "SELECT * FROM questioning.businesses";
                LoadCompanyView();
            }
        }

        // Нажатие на кнопку Удалить в Направления > Просмотр
        private void Button_Click_Del_Directions(object sender, RoutedEventArgs e)
        {
            string idDir = ((Button)e.Source).Uid;

            MessageBoxResult result = MessageBox.Show("После удаления направления, созданные ранее блоки вопросов и анкеты будут автоматически удалены. Удалить направление?", "Удаление Направления", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                _query.Type = "SELECT"; _query.Table = "Directions"; _query.Query = "SELECT * FROM questioning.directions WHERE id = '" + idDir + "'";
                _answer = QueryToDB.SendQuery(_query);
                ListDirections LD = (ListDirections)QueryToDB.ProcessResponse(_answer, _query);
                string nameDirectionForDelete = LD.listDirections[0].Name;

                _query.Type = "DELETE"; _query.Table = "qanswers"; _query.Query = "DELETE qanswers.* FROM questioning.qanswers JOIN qquestions ON qanswers.idQuestion = qquestions.id " +
                "JOIN qquestionsblocks ON qquestions.idQuestionBlock = qquestionsblocks.id " +
                "JOIN questionnaires ON qquestionsblocks.idQuestionnaire = questionnaires.id " +
                "WHERE questionnaires.direction = '" + nameDirectionForDelete + "'";
                QueryToDB.SendQuery(_query);

                _query.Type = "DELETE"; _query.Table = "questionnaires";  _query.Query = "DELETE FROM questioning.questionnaires WHERE direction = '" + nameDirectionForDelete + "'";
                QueryToDB.SendQuery(_query);

                _query.Type = "DELETE"; _query.Table = "questions"; _query.Query = "DELETE questions.* FROM questions " +
                "JOIN tasktype ON questions.idTaskType = tasktype.id " +
                "JOIN directions ON tasktype.idDirections = directions.id " +
                "WHERE directions.name = '" + nameDirectionForDelete + "'";
                QueryToDB.SendQuery(_query);

                _query.Type = "DELETE"; _query.Table = "tasktype"; _query.Query = "DELETE tasktype.* FROM tasktype WHERE tasktype.idDirections = (SELECT id FROM directions WHERE name = '" + nameDirectionForDelete + "')";
                QueryToDB.SendQuery(_query);

                _query.Type = "DELETE"; _query.Table = "Directions"; _query.Query = "DELETE FROM questioning.Directions WHERE id='" + idDir + "'";
                QueryToDB.SendQuery(_query);

                _query.Type = "SELECT"; _query.Table = "Directions"; _query.Query = "SELECT * FROM questioning.directions";
                LoadDirectionsView();
            }
        }

        // прогрузка всех comboBox-ов
        private void LoadInfoInComboboxes()
        {
            try
            {

                _query.Type = "SELECT";
                _query.Table = "Directions";
                _query.Query = "SELECT * FROM questioning.Directions";

                _answer = QueryToDB.SendQuery(_query);
                ListDirections LD = (ListDirections)QueryToDB.ProcessResponse(_answer, _query);

                _query.Type = "SELECT";
                _query.Table = "Industry";
                _query.Query = "SELECT * FROM questioning.industry";

                _answer = QueryToDB.SendQuery(_query);
                ListIndustry LInd = (ListIndustry)QueryToDB.ProcessResponse(_answer, _query);

                _query.Type = "SELECT";
                _query.Table = "Businesses";
                _query.Query = "SELECT * FROM questioning.businesses";

                _answer = QueryToDB.SendQuery(_query);
                ListBusinesses LB = (ListBusinesses)QueryToDB.ProcessResponse(_answer, _query);

                cbDirInAnswers.Items.Clear();
                cbDirInQuestionBlockesCreate.Items.Clear();
                cbDirInQuestionBlockesView.Items.Clear();
                cbIndustryInCompanyAdd.Items.Clear();
                cbIndustryInCompanyEdit.Items.Clear();
                cbIndustryInCompanyView.Items.Clear();
                cbCompanyInAnswers.Items.Clear();
                cbDirInQBEdit.Items.Clear();
                cbDirInDataProcessing.Items.Clear();


                cbDirInQuestionBlockesView.Items.Add("Любое");
                cbIndustryInCompanyView.Items.Add("Любая");
                cbCompanyInAnswers.Items.Add("Любое");

                foreach (Direction d in LD.listDirections)
                {
                    cbDirInAnswers.Items.Add(d.Name);
                    cbDirInQuestionBlockesCreate.Items.Add(d.Name);
                    cbDirInQuestionBlockesView.Items.Add(d.Name);
                    cbDirInQBEdit.Items.Add(d.Name);
                    cbDirInDataProcessing.Items.Add(d.Name);
                }

                foreach (Industry Ind in LInd.listIndustry)
                {
                    cbIndustryInCompanyAdd.Items.Add(Ind.Name);
                    cbIndustryInCompanyEdit.Items.Add(Ind.Name);
                    cbIndustryInCompanyView.Items.Add(Ind.Name);
                }

                foreach (Business b in LB.listBusinesses)
                {
                    cbCompanyInAnswers.Items.Add(b.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // при выборе направлении Анкеты > Просмотр
        private void cbDirInQuestionBlockesView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string condition = "";
            if (cbDirInQuestionBlockesView.SelectedItem != null && cbDirInQuestionBlockesView.SelectedValue.ToString() != "Любое")
                condition = " WHERE idDirections = (SELECT id FROM questioning.directions WHERE name = '" + cbDirInQuestionBlockesView.SelectedValue.ToString() + "')";

            _query.Type = "SELECT";
            _query.Table = "QuestionBlocks";
            _query.Query = "SELECT * FROM questioning.tasktype" + condition;
            LoadQuestionBlockesView();

        }

        // Нажатие на кнопку Найти в Предприятия > Просмотр
        private void btnSearchInCompanyView_Click(object sender, RoutedEventArgs e)
        {
            string condition = "";
            condition = " WHERE name LIKE '%"+ tbSearchInCompanyView.Text + "%'";
            if (cbIndustryInCompanyView.SelectedItem != null && cbIndustryInCompanyView.SelectedValue.ToString() != "Любая")
                condition += " AND idIndustry = (SELECT id FROM questioning.industry WHERE name = '" + cbIndustryInCompanyView.SelectedValue.ToString() + "')";
            _query.Type = "SELECT";
            _query.Table = "Businesses";
            _query.Query = "SELECT * FROM questioning.businesses" + condition;
            LoadCompanyView();
            
        }

        // при выборе Структурного подразделения в Предприятия > Просмотр
        private void cbIndustryInCompanyView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string condition = "";
            if (cbIndustryInCompanyView.SelectedItem != null && cbIndustryInCompanyView.SelectedValue.ToString() != "Любая")
            {
                condition = " WHERE idIndustry = (SELECT id FROM questioning.industry WHERE name = '" + cbIndustryInCompanyView.SelectedValue.ToString() + "')";

                if (tbSearchInCompanyView.Text != "")
                    condition += " AND name LIKE '%" + tbSearchInCompanyView.Text + "%'";
            }
            _query.Type = "SELECT";
            _query.Table = "Businesses";
            _query.Query = "SELECT * FROM questioning.businesses" + condition;
            LoadCompanyView();
        }

        // Нажатие на кнопку Найти в Направления > Просмотр
        private void btnSearchInDirectionView_Click(object sender, RoutedEventArgs e)
        {
            string condition = "";
            condition = " WHERE name LIKE '%" + tbSearchInDirectionView.Text + "%'";
            _query.Type = "SELECT";
            _query.Table = "Directions";
            _query.Query = "SELECT * FROM questioning.directions" + condition;
            LoadDirectionsView();
        }

        // нажатие на кнопу Создания Анкет
        private void btnCreateQuestioning_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbNameQB.Text.Replace(" ", "") == "" || cbDirInQuestionBlockesCreate.SelectedValue == null || 
                    tbFirstQuestionCreate.Text.Replace(" ", "") == "" || tbShortNameQB.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Не все поля заполнены");
                }
                else
                {
                    string nameQB = tbNameQB.Text;
                    string shortNameQB = tbShortNameQB.Text;
                    int idDir;
                    int idQB;

                    _query.Type = "SELECT";
                    _query.Table = "Directions";
                    _query.Query = "SELECT * FROM questioning.directions WHERE name = '" + cbDirInQuestionBlockesCreate.SelectedValue.ToString() + "'";

                    _answer = QueryToDB.SendQuery(_query);
                    ListDirections LD = (ListDirections)QueryToDB.ProcessResponse(_answer, _query);
                    idDir = LD.listDirections[0].Id;

                    _query.Type = "INSERT";
                    _query.Table = "tasktype";
                    _query.Query = "INSERT INTO questioning.tasktype (name, description, idDirections) VALUES ('" + nameQB + "', '" + shortNameQB + "', '" + idDir + "')";
                    QueryToDB.SendQuery(_query);

                    _query.Type = "SELECT";
                    _query.Table = "QuestionBlocks";
                    _query.Query = "SELECT * FROM questioning.tasktype WHERE name = '" + nameQB + "' AND description  = '" + shortNameQB + "' AND idDirections = '" + idDir + "'";
                    _answer = QueryToDB.SendQuery(_query);
                    ListQuestionBlocks LQB = (ListQuestionBlocks)QueryToDB.ProcessResponse(_answer, _query);
                    idQB = LQB.listQuestionBlocks[0].Id;

                    foreach (TextBox tb in containerWithNewQuestions.Children)
                    {
                        if (tb.Text.Replace(" ", "") != "")
                        {
                            _query.Type = "INSERT";
                            _query.Table = "questions";
                            _query.Query = "INSERT INTO questioning.questions (text, idTaskType) VALUES ('" + tb.Text + "', '" + idQB + "')";
                            QueryToDB.SendQuery(_query);
                        }
                    }
                    MessageBox.Show("Блок вопросов создан успешно");

                    int index = 0;
                    tabControl_2.SelectedIndex = index;
                    GridCursor_2.Margin = new Thickness(10 + 150 * index, 0, 0, 0);
                    ClearCreatingQuestionBlockes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // нажатие на кнопу Добавления Предприятий
        private void btnAddCompany_Click(object sender, RoutedEventArgs e)
        {
            if (tbNameCompany.Text.Replace(" ", "") == "" || cbIndustryInCompanyAdd.SelectedValue == null)
            {
                MessageBox.Show("Не все поля заполнены");
            } else {
                int IdInd;
                string nameCompany = tbNameCompany.Text;
                string descriptionCompany = tbDescriptionCompany.Text;
                string email = tbEmailCompany.Text;

                _query.Type = "SELECT";
                _query.Table = "Industry";
                _query.Query = "SELECT * FROM questioning.industry WHERE name = '" + cbIndustryInCompanyAdd.SelectedValue.ToString() + "'";

                _answer = QueryToDB.SendQuery(_query);
                ListIndustry LInd = (ListIndustry)QueryToDB.ProcessResponse(_answer, _query);
                IdInd = LInd.listIndustry[0].Id;

                _query.Type = "INSERT";
                _query.Table = "Businesses";
                _query.Query = "INSERT INTO questioning.businesses (name, description, idIndustry, email) VALUES ('" + nameCompany + "', '" + descriptionCompany + "', '" + IdInd + "', '" + email + "')";
                QueryToDB.SendQuery(_query);

                tbNameCompany.Text = "";
                tbDescriptionCompany.Text = "";
                tbEmailCompany.Text = "";

                MessageBox.Show("Предприятие добавлено успешно");
            }
        }

        // нажатие на кнопу Добавления Направления
        private void btnAddDirection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbNameDirection.Text.Replace(" ", "") == "")
                {
                    MessageBox.Show("Не все поля заполнены");
                }
                else
                {
                    string nameDescription = tbNameDirection.Text;
                    string descriptionDescription = tbDescriptionDirection.Text;

                    _query.Type = "INSERT";
                    _query.Table = "Directions";
                    _query.Query = "INSERT INTO questioning.directions (name, description) VALUES ('" + nameDescription + "', '" + descriptionDescription + "')";
                    QueryToDB.SendQuery(_query);

                    MessageBox.Show("Направление добавлено успешно");
                    ClearCreatingDirection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Кнопка Сохранить в Анкета > Редактировать
        private void btnSaveQuestioning_Click(object sender, RoutedEventArgs e)
        {
            if (tbNameEditQB.Text.Replace(" ", "") == "" || cbDirInQBEdit.SelectedValue == null)
            {
                MessageBox.Show("Не все поля заполнены");
            } else {
                string idQB = btnSaveQuestioning.Uid;
                string nameQB = tbNameEditQB.Text;
                string shortName = tbShortNameEditQB.Text;
                int idDirections;

                _query.Type = "SELECT";
                _query.Table = "Directions";
                _query.Query = "SELECT * FROM questioning.directions WHERE name = '" + cbDirInQBEdit.SelectedValue.ToString() + "'";
                _answer = QueryToDB.SendQuery(_query);
                ListDirections LD = (ListDirections)QueryToDB.ProcessResponse(_answer, _query);
                idDirections = LD.listDirections[0].Id;

                _query.Type = "UPDATE";
                _query.Table = "tasktype";
                _query.Query = "UPDATE questioning.tasktype SET name='" + nameQB + "', idDirections='" + idDirections + "', description = '" + shortName + "' WHERE id='" + idQB + "'";
                QueryToDB.SendQuery(_query);

                foreach (Grid grid in QuestionBlockesEditContainer.Children)
                {
                    TextBox tb = (TextBox)grid.Children[1];
                    if (tb.Text.Replace(" ", "") != "")
                    {
                        _query.Type = "UPDATE";
                        _query.Table = "questions";
                        _query.Query = "UPDATE questioning.questions SET text='" + tb.Text + "', idTaskType='" + idQB + "' WHERE id='" + tb.Uid + "'";
                        QueryToDB.SendQuery(_query);
                    }
                }

                foreach (TextBox tb in NewQuestionsEditContainer.Children)
                {
                    if (tb.Text.Replace(" ", "") != "")
                    {
                        _query.Type = "INSERT";
                        _query.Table = "questions";
                        _query.Query = "INSERT INTO questioning.questions (text, idTaskType) VALUES ('" + tb.Text + "', '" + idQB + "')";
                        QueryToDB.SendQuery(_query);
                    }
                }
                MessageBox.Show("Блок вопросов успешно отредактирован");
            }
        }

        // нажатие на кнопку Сохранить в Предприятия > Редактировать
        private void btnSaveCompany_Click(object sender, RoutedEventArgs e)
        {
            if (tbNameEditCompany.Text.Replace(" ", "") == "" || cbIndustryInCompanyEdit.SelectedValue == null)
            {
                MessageBox.Show("Не все поля заполнены");
            }else
            {
                string idCompany = btnSaveCompany.Uid;
                string nameCompany = tbNameEditCompany.Text;
                string description = tbDescriptionEditCompany.Text;
                string email = tbEmailEditCompany.Text;
                int idInd;

                _query.Type = "SELECT";
                _query.Table = "Industry";
                _query.Query = "SELECT * FROM questioning.Industry WHERE name = '" + cbIndustryInCompanyEdit.SelectedValue.ToString() + "'";
                _answer = QueryToDB.SendQuery(_query);
                ListIndustry LD = (ListIndustry)QueryToDB.ProcessResponse(_answer, _query);
                idInd = LD.listIndustry[0].Id;

                _query.Type = "UPDATE";
                _query.Table = "Businesses";
                _query.Query = "UPDATE questioning.businesses SET name='" + nameCompany + "', description = '" + description + "', idIndustry='" + idInd + "', email = '" + email + "' WHERE id='" + idCompany + "'";
                QueryToDB.SendQuery(_query);
                MessageBox.Show("Информация о предприятии успешно отредактировано");
                LoadCompanyView();
            }
        }

        // нажатие на кнопку Сохранить в Предприятия > Редактировать
        private void btnSaveDirection_Click(object sender, RoutedEventArgs e)
        {
            if (tbNameEditDirection.Text.Replace(" ", "") == "")
            {
                MessageBox.Show("Не все поля заполнены");
            }else
            {
                string idDir = btnSaveDirection.Uid;
                string nameDir = tbNameEditDirection.Text;
                string description = tbDescriptionEditDirection.Text;

                _query.Type = "UPDATE";
                _query.Table = "Directions";
                _query.Query = "UPDATE questioning.directions SET name='" + nameDir + "', description = '" + description + "' WHERE id='" + idDir + "'";
                QueryToDB.SendQuery(_query);

                MessageBox.Show("Информация о направлении успешно отредактировано");
            }
        }

        // нажатие на кнопку Очистить в Анкеты > Создание
        private void btnClearQuestioning_Click(object sender, RoutedEventArgs e)
        {
            ClearCreatingQuestionBlockes();
        }
        
        private void ClearCreatingQuestionBlockes()
        {
            tbNameQB.Clear();
            tbShortNameQB.Clear();
            cbDirInQuestionBlockesCreate.SelectedValue = null;

            TextBox tboxNewQuestion = new TextBox() { FontSize = 16, VerticalAlignment = VerticalAlignment.Center, Margin = new Thickness(50, 0, 85, 10), BorderThickness = new Thickness(0, 0, 0, 2), BorderBrush = Brushes.LightGray };
            tboxNewQuestion.LostFocus += new RoutedEventHandler(TB_NewQuestion_LostFocus);
            containerWithNewQuestions.Children.Clear();
            containerWithNewQuestions.Children.Add(tboxNewQuestion);
        }

        // нажатие на кнопку Очистить в Предприятия > Добавление
        private void btnClearAddCompany_Click(object sender, RoutedEventArgs e)
        {
            tbNameCompany.Clear();
            cbIndustryInCompanyAdd.SelectedValue = null;
            tbDescriptionCompany.Clear();
        }

        // Кнопка очистить в Создании Направления
        private void btnClearAddDirection_Click(object sender, RoutedEventArgs e)
        {
            ClearCreatingDirection();
        }

        // Очистка полей в Создании Направления
        private void ClearCreatingDirection()
        {
            tbNameDirection.Clear();
            tbDescriptionDirection.Clear();
        }

        private void cbDirInAnswers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAnswers();
            cbCompanyInAnswers.IsEnabled = true;
        }

        private void cbCompanyInAnswers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadAnswers();
        }

        private void MoveListItem(ListBoxItem listBoxItem, ListBox inListBox)
        {
            ((ListBox)(listBoxItem.Parent)).Items.Remove(listBoxItem);
            inListBox.Items.Add(listBoxItem);
        }

        // нажатие на элемент listbox-а (1)
        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lBI = (ListBoxItem)sender;
            MoveListItem(lBI, listBox1);
            lBI.MouseDoubleClick -= new MouseButtonEventHandler(ListBoxItem_MouseDoubleClick);
            lBI.MouseDoubleClick += new MouseButtonEventHandler(ListBoxItem_MouseDoubleClick_1);

        }

        // нажатие на элемент listbox-а (2)
        private void ListBoxItem_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lBI = (ListBoxItem)sender;
            MoveListItem(lBI, listBox);
            lBI.MouseDoubleClick -= new MouseButtonEventHandler(ListBoxItem_MouseDoubleClick_1);
            lBI.MouseDoubleClick += new MouseButtonEventHandler(ListBoxItem_MouseDoubleClick);
            
        }

        // нажатие на кнопку переноса всех предприятий из левого listbox в правый
        private void btnShiftAllToRight_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem lBI in listBox.Items)
            {
                ListBoxItem newlBI = new ListBoxItem() { Content = lBI.Content };
                newlBI.MouseDoubleClick += new MouseButtonEventHandler(ListBoxItem_MouseDoubleClick_1);
                listBox1.Items.Add(newlBI);
            }
            listBox.Items.Clear();
        }

        // нажатие на кнопку переноса всех предприятий из правого listbox в левый
        private void btnShiftAllToLeft_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem lBI in listBox1.Items)
            {
                ListBoxItem newlBI = new ListBoxItem() { Content = lBI.Content };
                newlBI.MouseDoubleClick += new MouseButtonEventHandler(ListBoxItem_MouseDoubleClick);
                listBox.Items.Add(newlBI);
            }
            listBox1.Items.Clear();
        }

        // нажатие на кнопку Обработать результаты анкет в Анкеты > Вычисления
        private void btnDataProcessing_Click(object sender, RoutedEventArgs e)
        {
            if (cbDirInDataProcessing.SelectedValue == null || cbDirInDataProcessing.SelectedValue.ToString() == "")
            {
                MessageBox.Show("Выберите направление");
            }
            else if (listBox1.Items.Count < 2)
            {
                MessageBox.Show("Количество предприятий должно быть выбрано не меньше двух");
            }
            else
            {
                ListBusinesses LB = new ListBusinesses();
                foreach (ListBoxItem item in listBox1.Items)
                {
                    LB.listBusinesses.Add(new Business() { Name =  item.Content.ToString()});
                }

                _query.Type = "SELECT";
                _query.Table = "Answers";
                _query.Query = "SELECT * FROM questioning.qquestionsblocks WHERE idQuestionnaire = ( SELECT id FROM questioning.questionnaires WHERE direction = '" + cbDirInDataProcessing.SelectedValue + "')";
                _query.nameDirectionForAnswers = cbDirInDataProcessing.SelectedValue.ToString();
                _query.nameCompanyForAnswers = null;
                _answer = QueryToDB.SendQuery(_query);

                ListAnswersBlock LAB = (ListAnswersBlock)QueryToDB.ProcessResponse(_answer, _query);

                Table2 t2 = new Table2();
                t2.ConversationInputData(LB, LAB);
                t2.CalculateAll();

                List<RowForResultTable> listRowsForRT = new List<RowForResultTable>();

                foreach(RowForTable2 row in t2.rows)
                {
                    RowForResultTable rowForRT = new RowForResultTable(row.nameQB, row.masData[0], row.masData[3], row.masData[4], row.masData[6]);
                    listRowsForRT.Add(rowForRT);
                }

                WindForResult window = new WindForResult();
                window.listRows = listRowsForRT;
                window.Owner = this;
                window.Show();

                
            }
        }

        // при выборе направления в Анкеты > Вычисления
        private void cbDirInDataProcessing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _query.Type = "SELECT";
            _query.Table = "Businesses";
            _query.Query = "SELECT * FROM questioning.businesses "
                    + "WHERE (SELECT count(id) FROM questioning.qanswers "
                    + "WHERE idBusinesses=businesses.id AND idQuestion=(SELECT id FROM questioning.qquestions "
                    + "WHERE idQuestionBlock = (SELECT id FROM questioning.qquestionsblocks "
                    + "WHERE idQuestionnaire = (SELECT id FROM questioning.questionnaires WHERE direction = '" + cbDirInDataProcessing.SelectedValue + "' LIMIT 1)LIMIT 1)LIMIT 1) ) > 0";

            _answer = QueryToDB.SendQuery(_query);
            ListBusinesses LB = (ListBusinesses)QueryToDB.ProcessResponse(_answer, _query);

            listBox.Items.Clear();
            listBox1.Items.Clear();

            foreach (Business b in LB.listBusinesses)
            {
                ListBoxItem lBI = new ListBoxItem() { Content = b.Name }; // for listbox
                lBI.MouseDoubleClick += new MouseButtonEventHandler(ListBoxItem_MouseDoubleClick);

                listBox.Items.Add(lBI); // add listboxitem to LB
            }
        }

        // При выборе Направления в Редакторе Анкет
        private void cbDirInEditor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _query.Type = "SELECT";
            _query.Table = "QuestionBlocks";
            _query.Query = "SELECT * FROM questioning.tasktype WHERE tasktype.idDirections = (SELECT id FROM questioning.directions WHERE name = '" + cbDirInEditor.SelectedValue + "' LIMIT 1)";

            _answer = QueryToDB.SendQuery(_query);
            ListQuestionBlocks LQB = (ListQuestionBlocks)QueryToDB.ProcessResponse(_answer, _query);

            listBoxTasksLeft.Items.Clear();
            listBoxTasksRight.Items.Clear();

            foreach (QuestionBlock qb in LQB.listQuestionBlocks)
            {
                ListBoxItem lBI = new ListBoxItem() { Content = String.IsNullOrEmpty(qb.ShortName) ? "null" : qb.ShortName };
                lBI.MouseDoubleClick += new MouseButtonEventHandler(listBoxItemTasksLeft_MouseDoubleClick);

                listBoxTasksLeft.Items.Add(lBI);
            }
        }

        // нажатие на элемент listbox-а Список задач в редакторе анкет
        private void listBoxItemTasksLeft_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lBI = (ListBoxItem)sender;
            MoveListItem(lBI, listBoxTasksRight);
            lBI.MouseDoubleClick -= new MouseButtonEventHandler(listBoxItemTasksLeft_MouseDoubleClick);
            lBI.MouseDoubleClick += new MouseButtonEventHandler(listBoxItemTasksRight_MouseDoubleClick);
        }

        // нажатие на элемент listbox-а Список задач в редакторе анкет
        private void listBoxItemTasksRight_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lBI = (ListBoxItem)sender;
            MoveListItem(lBI, listBoxTasksLeft);
            lBI.MouseDoubleClick -= new MouseButtonEventHandler(listBoxItemTasksRight_MouseDoubleClick);
            lBI.MouseDoubleClick += new MouseButtonEventHandler(listBoxItemTasksLeft_MouseDoubleClick);
        }

        // нажатие на элемент listbox-а Предприятий в редакторе анкет
        private void listBoxItemBusinessLeft_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lBI = (ListBoxItem)sender;
            MoveListItem(lBI, listBoxBusinessRight);
            lBI.MouseDoubleClick -= new MouseButtonEventHandler(listBoxItemBusinessLeft_MouseDoubleClick);
            lBI.MouseDoubleClick += new MouseButtonEventHandler(listBoxItemBusinessRight_MouseDoubleClick);
        }

        // нажатие на элемент listbox-а Предприятий в редакторе анкет
        private void listBoxItemBusinessRight_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBoxItem lBI = (ListBoxItem)sender;
            MoveListItem(lBI, listBoxBusinessLeft);
            lBI.MouseDoubleClick -= new MouseButtonEventHandler(listBoxItemBusinessRight_MouseDoubleClick);
            lBI.MouseDoubleClick += new MouseButtonEventHandler(listBoxItemBusinessLeft_MouseDoubleClick);
        }


        // нажатие на кнопку переноса всех Задач в Редакторе из левого listbox в правый
        private void btnShiftAllToRightEditorTask_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem lBI in listBoxTasksLeft.Items)
            {
                ListBoxItem newlBI = new ListBoxItem() { Content = lBI.Content };
                newlBI.MouseDoubleClick += new MouseButtonEventHandler(listBoxItemTasksRight_MouseDoubleClick);
                listBoxTasksRight.Items.Add(newlBI);
            }
            listBoxTasksLeft.Items.Clear();
        }

        // нажатие на кнопку переноса всех Задач в Редакторе из правого listbox в левый
        private void btnShiftAllToLeftEditorTask_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem lBI in listBoxTasksRight.Items)
            {
                ListBoxItem newlBI = new ListBoxItem() { Content = lBI.Content };
                newlBI.MouseDoubleClick += new MouseButtonEventHandler(listBoxItemTasksLeft_MouseDoubleClick);
                listBoxTasksLeft.Items.Add(newlBI);
            }
            listBoxTasksRight.Items.Clear();
        }

        // нажатие на кнопку переноса всех Предприятий в Редакторе из левого listbox в правый
        private void btnShiftAllToRightEditorBusiness_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem lBI in listBoxBusinessLeft.Items)
            {
                ListBoxItem newlBI = new ListBoxItem() { Content = lBI.Content };
                newlBI.MouseDoubleClick += new MouseButtonEventHandler(listBoxItemBusinessRight_MouseDoubleClick);
                listBoxBusinessRight.Items.Add(newlBI);
            }
            listBoxBusinessLeft.Items.Clear();
        }

        // нажатие на кнопку переноса всех Предприятий в Редакторе из правого listbox в левый
        private void btnShiftAllToLeftEditorBusiness_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListBoxItem lBI in listBoxBusinessRight.Items)
            {
                ListBoxItem newlBI = new ListBoxItem() { Content = lBI.Content };
                newlBI.MouseDoubleClick += new MouseButtonEventHandler(listBoxItemBusinessLeft_MouseDoubleClick);
                listBoxBusinessLeft.Items.Add(newlBI);
            }
            listBoxBusinessRight.Items.Clear();
        }

        // Кнопка Сохранить и отправит в Редакторе форм
        private void btnSaveQuestioningEditor_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(cbDirInEditor.SelectedValue.ToString()) && listBoxTasksRight.Items.Count > 1)
            {
                Guid idQuestionnaire = Guid.NewGuid();
                Guid idQuestionBlock = Guid.NewGuid();

                _requestHandler.SaveQuestionnaire(idQuestionnaire.ToString(), cbDirInEditor.SelectedValue.ToString(), DateTime.Now.ToString("yyyy-MM-dd"));

                foreach (ListBoxItem item in listBoxTasksRight.Items)
                {
                    _requestHandler.SaveQuestionBlock(idQuestionBlock.ToString(), idQuestionnaire.ToString(), item.Content.ToString(), cbDirInEditor.SelectedValue.ToString());
                    idQuestionBlock = Guid.NewGuid();
                }

                LoadQuestionnairesEditor();
                MessageBox.Show("Анкета сохранена");
            }
            else
            {
                MessageBox.Show("Не все данные введены");
            }
        }

        // Кнопка Создать блок вопросов в Вопросы
        private void btnCreateQuestionBlock_Click(object sender, RoutedEventArgs e)
        {
            int index = 5;
            tabControl_2.SelectedIndex = index;
        }
    }
}
