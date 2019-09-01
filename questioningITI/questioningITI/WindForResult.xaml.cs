using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;

namespace questioningITI
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class WindForResult : Window
    {
        public List<RowForResultTable> listRows { get; set; }

        public WindForResult()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.loadInfoInDataGrid();
        }

        void loadInfoInDataGrid()
        {
            DataTable dt = new DataTable();
            DataColumn col0 = new DataColumn("Профессиональные задачи", typeof(string));
            DataColumn col1 = new DataColumn("Относительные", typeof(double));
            DataColumn col2 = new DataColumn("значения", typeof(double));
            DataColumn col3 = new DataColumn("дисперсий", typeof(double));

            dt.Columns.Add(col0);
            dt.Columns.Add(col1);
            dt.Columns.Add(col2);
            dt.Columns.Add(col3);

            foreach(RowForResultTable inputRow in listRows)
            {
                DataRow newRow = dt.NewRow();
                newRow[0] = inputRow.nameProfTask;
                newRow[1] = inputRow.col_1;
                newRow[2] = inputRow.col_2;
                newRow[3] = inputRow.col_3;
                dt.Rows.Add(newRow);
            }
            dataGrid.ItemsSource = dt.DefaultView;
        }

        private void btnCreatehistogram_Click(object sender, RoutedEventArgs e)
        {
            FormResult FR = new FormResult();
            FR.listRows = listRows;
            FR.Show();
        }
    }
}
