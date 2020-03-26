using System;
using System.Windows;

namespace Questioning
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        QueryToDB _queryToDb = new QueryToDB();

        public Authorization()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var win = new MainWindow {Owner = this};

                _queryToDb.Type = "SELECT";
                _queryToDb.Table = "Users";
                _queryToDb.Query = "SELECT * FROM questioning.Users WHERE login = '" + txtLoginEntry.Text + "'";
                _queryToDb.password = txtPasswordEntry.Password;

                if (QueryToDB.SendQuery(_queryToDb) == "ok")
                    win.Show();
                else
                    MessageBox.Show("Ошибка авторизации");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
