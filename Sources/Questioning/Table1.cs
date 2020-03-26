using System;

namespace Questioning
{
    public class Table1 : StratifiedExperiment
    {
        public int m; // кол-во вопросов
        public string nameQuestionBlock;
        public string[,] inputData;
        public double[,] masX1;
        public double[,] masS1;
        public double[,] masXAndS2;
        public double x3;
        public double s3;

        // методы:

        public void AddNewTable1(RowForInputData row)
        {
            nameQuestionBlock = row.shortNameQB;
            m = row.listAnswers[0].Length;
            inputData = new string[k, m];
            n = 0;

            for (int i = 0; i < k; i++)
            {
                for(int j = 0; j < inputData.GetLength(1); j++)
                {
                    inputData[i, j] = row.listAnswers[i][j];
                }
                n += row.listAnswers[i][0].Length;
            }
        }

        public void CalcX1()
        {
            masX1 = new double[k, m];

            for (int i = 0; i < inputData.GetLength(0); i++)
            {
                for (int j = 0; j < inputData.GetLength(1); j++)
                {
                    masX1[i, j] = FindS(inputData[i, j])[0];
                }
            }
        }

        public void CalcS1()
        {
            masS1 = new double[k, m];

            for (int i = 0; i < inputData.GetLength(0); i++)
            {
                for (int j = 0; j < inputData.GetLength(1); j++)
                {
                    masS1[i, j] = FindS(inputData[i, j])[1];
                }
            }
        }

        public void CalcXAndS2()
        {
            masXAndS2 = new double[k, 2];

            for (int i = 0; i < masX1.GetLength(0); i++)
            {
                double[] mas = new double[masX1.GetLength(1)];
                for( int j = 0; j < masX1.GetLength(1); j++)
                {
                    mas[j] = masX1[i, j];
                }

                masXAndS2[i, 0] = FindS(mas)[0];
                masXAndS2[i, 1] = FindS(mas)[1];
            }
        }

        public void CalcXAndS3()
        {
            double[] mas = new double[masXAndS2.GetLength(0)];

            for (int i = 0; i < masXAndS2.GetLength(0); i++)
            {
                mas[i] = masXAndS2[i, 0];
            }

            x3 = FindS(mas)[0];
            s3 = FindS(mas)[1];
        }

        // высчитать все начения
        public void CalculateAll()
        {
            CalcX1();
            CalcS1();
            CalcXAndS2();
            CalcXAndS3();

        }


        // ********************считаем ДИСПЕРСИИ**************************
        public double[] FindS(string somemas)
        {
            string[,] mas = new string[somemas.Length + 1, 4];
            double[] resultMas = new double[2];

            //переносим массив с ответами в таблицу для вычислений
            for (int i = 0; i < somemas.Length; i++)
            {
                mas[i, 0] = somemas[i].ToString();
            }

            //находим сумму ответов
            FindSumFirstColumn(ref mas);

            //находим среднее арифметическое
            FindAvgFirstColumn(ref mas);

            //заполняем 3-ий стобец
            FindXMinusAvg(ref mas);

            //заполняем 4-ий стобец
            FindPowerTheardColumn(ref mas);

            //заполняем 4-ий стобец сумму
            FindSumPowerTheardColumn(ref mas);


            resultMas[0] = Math.Round(double.Parse(mas[mas.GetLength(0) - 1, mas.GetLength(1) - 3]), 3);
            resultMas[1] = Math.Round(double.Parse(mas[mas.GetLength(0) - 1, 3]) / double.Parse((mas.GetLength(0) - 2).ToString()), 3);
            return resultMas;
        }

        public double[] FindS(double[] somemas)
        {
            string[,] mas = new string[somemas.Length + 1, 4];
            double[] resultMas = new double[2];

            //переносим массив с ответами в таблицу для вычислений
            for (int i = 0; i < somemas.Length; i++)
            {
                mas[i, 0] = somemas[i].ToString();
            }

            //находим сумму ответов
            FindSumFirstColumn(ref mas);

            //находим среднее арифметическое
            FindAvgFirstColumn(ref mas);

            //заполняем 3-ий стобец
            FindXMinusAvg(ref mas);

            //заполняем 4-ий стобец
            FindPowerTheardColumn(ref mas);

            //заполняем 4-ий стобец сумму
            FindSumPowerTheardColumn(ref mas);


            resultMas[0] = Math.Round(double.Parse(mas[mas.GetLength(0) - 1, mas.GetLength(1) - 3]), 3);
            resultMas[1] = Math.Round(Convert.ToDouble(mas[mas.GetLength(0) - 1, 3]) / Convert.ToDouble(mas.GetLength(0) - 2), 3);
            return resultMas;
        }

        //находим сумму ответов
        private void FindSumFirstColumn(ref string[,] mas)
        {
            double sumFirstColumn = 0;
            for (int i = 0; i < mas.GetLength(0) - 1; i++)
            {
                sumFirstColumn += Convert.ToDouble(mas[i, 0]);
            }
            mas[mas.GetLength(0) - 1, 0] = sumFirstColumn.ToString();
        }

        //находим среднее арифметическое
        private void FindAvgFirstColumn(ref string[,] mas)
        {
            double count = (mas.GetLength(0) - 1);
            double sum = Convert.ToDouble(mas[mas.GetLength(0) - 1, 0]);
            double avg = sum / count;

            mas[mas.GetLength(0) - 1, 1] = avg.ToString();
        }

        //находим 3-ий столбец
        private void FindXMinusAvg(ref string[,] mas)
        {

            double avg = Convert.ToDouble(mas[mas.GetLength(0) - 1, 1]);
            for (int i = 0; i < mas.GetLength(0) - 1; i++)
            {
                double x = Convert.ToDouble(mas[i, 0]);
                mas[i, 2] = (x - avg).ToString();
            }
        }

        //находим 4-ий столбец
        private void FindPowerTheardColumn(ref string[,] mas)
        {
            for (int i = 0; i < mas.GetLength(0) - 1; i++)
            {
                double x = Convert.ToDouble(mas[i, 2]);
                mas[i, 3] = (x * x).ToString();
            }
        }

        //заполняем 4-ий стобец сумму
        private void FindSumPowerTheardColumn(ref string[,] mas)
        {
            double sum = 0;
            for (int i = 0; i < mas.GetLength(0) - 1; i++)
            {
                sum += Convert.ToDouble(mas[i, 3]);
            }
            mas[mas.GetLength(0) - 1, 3] = sum.ToString();

        }
    }
}
