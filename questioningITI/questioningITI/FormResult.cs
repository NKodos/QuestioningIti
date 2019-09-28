using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace questioningITI
{
    public partial class FormResult : Form
    {
        public List<RowForResultTable> ListRows { get; set; }

        public FormResult()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            const int index = 0;

            foreach (var row in ListRows)
            {
                var ser = new Series() { Name = row.nameProfTask };
                ser.IsValueShownAsLabel = true;
                ser.Points.AddXY(row.nameProfTask, row.col_1);
                chart1.Series.Add(ser);
                chart1.Legends[index].Font = new Font("Times New Roman", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            }

            chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
        }
    }
}
