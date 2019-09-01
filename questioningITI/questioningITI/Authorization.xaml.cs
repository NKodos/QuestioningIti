using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace questioningITI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        QueryToDB q1 = new QueryToDB();
        string answer = "";

        public Authorization()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow win = new MainWindow();
                win.Owner = this;

                q1.Type = "SELECT";
                q1.Table = "Users";
                q1.Query = "SELECT * FROM questioning.Users WHERE login = '" + txtLoginEntry.Text + "'";
                q1.password = txtPasswordEntry.Password;

                if (QueryToDB.SendQuery(q1) == "ok")
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
