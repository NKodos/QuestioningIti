using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace questioningITI
{
    public class RowForResultTable
    {
        public string nameProfTask = "";
        public double col_1;
        public double col_2;
        public double col_3;

        public RowForResultTable()
        {

        }

        public RowForResultTable(string nameProfTask, double s1, double q1, double q2, double qT)
        {
            this.nameProfTask = nameProfTask;
            CalcCol_1(s1, qT);
            CalcCol_2(q1, qT);
            CalcCol_3(q2, qT);
        }

        public void CalcCol_1(double s1, double qT)
        {
            col_1 = Math.Round(s1 / qT * 100, 3);
        }

        public void CalcCol_2(double q1, double qT)
        {
            col_2 = Math.Round(q1 / qT * 100, 3);
        }

        public void CalcCol_3(double q2, double qT)
        {
            col_3 = Math.Round(q2 / qT * 100, 3);
        }
    }
}
