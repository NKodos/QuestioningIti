using QuestioningLibrary;
using System;
using System.Collections.Generic;

namespace questioningITI
{
    public struct RowForTable2
    {
        public string nameQB;
        public double[] masData;
    }

    public struct RowForInputData
    {
        public string nameQB;
        public string shortNameQB;
        public List<string[]> listAnswers;
    }

    public class Table2 : Table1
    {
        public List<RowForTable2> rows = new List<RowForTable2>();

        // методы:

        //Преобразование входных данных
        public void ConversationInputData(ListBusinesses LB, ListAnswersBlock LAB)
        {
            List<RowForInputData> rowsFromInputData = new List<RowForInputData>();

            k = 0; 

            foreach (AnswersBlock AB in LAB.listAB)
            {
                foreach (Business b in LB.listBusinesses)
                {
                    if (AB.nameCompany == b.Name)
                    {
                        k++;
                        foreach(QuestionBlock QB in AB.LQB.listQuestionBlocks)
                        {
                            bool flag = false;
                            foreach (RowForInputData row in rowsFromInputData)
                            {
                                if (QB.Title == row.nameQB)
                                {
                                    string[] mas = new string[QB.listQuestions.Count];

                                    for (int i = 0; i < QB.listQuestions.Count; i++)
                                    {
                                        foreach (Answer an in QB.listQuestions[i].listAnswers)
                                        {
                                            mas[i] += an.idVariantsOfAnswers;
                                        }
                                    }
                                    row.listAnswers.Add(mas);
                                    flag = true;
                                }
                            }

                            if (!flag)
                            {
                                RowForInputData row = new RowForInputData() { nameQB = QB.Title, shortNameQB = QB.ShortName, listAnswers = new List<string[]>() };

                                string[] mas = new string[QB.listQuestions.Count];

                                for (int i = 0; i < QB.listQuestions.Count; i++)
                                {
                                    if (QB.listQuestions[i].listAnswers.Count > 0 && mas != null)
                                    {
                                        foreach (Answer an in QB.listQuestions[i].listAnswers)
                                        {
                                            mas[i] += an.idVariantsOfAnswers;
                                        }
                                    }
                                    else
                                    {
                                        mas = null;
                                    }
                                }

                                if (mas != null)
                                {
                                    row.listAnswers.Add(mas);
                                    rowsFromInputData.Add(row);
                                }
                            }
                        }
                        break;
                    }
                }
            }
            foreach(RowForInputData row in rowsFromInputData)
            {
                Table1 t1 = new Table1();
                t1.AddNewTable1(row);
                listTable.Add(t1);
            }
        }

        // s1^2
        public double CalcS1_2(Table1 t1)
        {
            double sum = 0;

            for (int i = 0; i < t1.masS1.GetLength(0); i++)
            {
                for (int j = 0; j < t1.masS1.GetLength(1); j++)
                {
                    sum += t1.masS1[i, j];
                }
            }

            return Math.Round(sum / n, 3);
        }

        // s2^2
        public double CalcS2_2(Table1 t1)
        {
            double sum = 0;

            for (int i = 0; i < t1.masXAndS2.GetLength(0); i++)
            {
                sum += t1.masXAndS2[i, 1];
            }

            return Math.Round(n*(sum / k), 3);
        }

        // s3^2
        public double CalcS3_2(Table1 t1)
        {
            return Math.Round(n * t1.m * t1.s3, 3);
        }

        // q1
        public double CalcQ1(double s2_2, double s1_2)
        {
            return Math.Round((s2_2 - s1_2) / n, 3);
        }

        // q2
        public double CalcQ2(double s3_2, double s2_2, int m)
        {
            return Math.Round((s3_2 - s2_2) / (n * m), 3);
        }

        // q3
        public double CalcQ3(double q1, double s1_2)
        {
            return Math.Round(q1 + s1_2, 3);
        }

        // qT
        public double CalcQT(double q3, double q2)
        {
            return Math.Round(q3 + q2, 3);
        }

        // посчитать все
        public new void CalculateAll()
        {
            foreach (Table1 t1 in listTable)
            {
                RowForTable2 row = new RowForTable2() { masData = new double[7] };

                t1.CalculateAll();
                row.nameQB = t1.nameQuestionBlock;

                double s1_2 = CalcS1_2(t1);
                double s2_2 = CalcS2_2(t1);
                double s3_2 = CalcS3_2(t1);
                double q1 = CalcQ1(s2_2, s1_2);
                double q2 = CalcQ2(s3_2, s2_2, t1.m);
                double q3 = CalcQ3(q1, s1_2);
                double qT = CalcQT(q3, q2);

                row.masData[0] = s1_2;
                row.masData[1] = s2_2;
                row.masData[2] = s3_2;
                row.masData[3] = q1;
                row.masData[4] = q2;
                row.masData[5] = q3;
                row.masData[6] = qT;

                rows.Add(row);
            }
            
        }

        // результат преобразовать в строку
        public string ResultToString()
        {
            string result = "";

            foreach (RowForTable2 row in rows)
            {
                result += row.nameQB + "   ";

                foreach (double x in row.masData)
                {
                    result += x + " ";
                }
                result += "   ###   ";
            }

            return result;
        }
    }
}
