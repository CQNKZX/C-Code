using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lei_4882994_hw1
{
    sealed class statstk //statistical tools will be created here. eg. sample std and sample mean
    {
        public static double[] std2d(double[,] qwe) //calculates mean and sample std of elements for a given 2-D array
        {
            int row = qwe.GetLength(0);
            int col = qwe.GetLength(1);
            double[] output = new double[2];
            double sum = 0; double sum1 = 0;
            foreach (double element in qwe)
            {
                sum = sum + element;
            }
            double avg = sum / (row * col);
            output[0] = avg;
            double[,] temp = qwe;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    temp[i, j] = (temp[i, j] - avg) * (temp[i, j] - avg);
                }
            }
            foreach (double element in temp)
            {
                sum1 = sum1 + element;
            }
            double var = sum1 / (row * col - 1); // minus one is convention
            output[1] = Math.Sqrt(var); //output contains [sample average, sample standard deviation]
            return output;
        }
        public static double[] std1d(double[] asd)//compute sample mean and sample std of one dimensional vector
        {
            int trial = asd.GetLength(0);
            double summ = 0;
            foreach (double element in asd)
            {
                summ = summ + element;
            }
            double[] output = new double[2];
            output[0] = summ / trial; //average and discount
            double[] temp1 = new double[trial]; //compute  Standard Error
            for (int i = 0; i < trial; i++)
            {
                temp1[i] = (asd[i] - output[0]) * (asd[i] - output[0]);
            }
            summ = 0;
            foreach (double element in temp1)
            {
                summ = summ + element;
            }
            output[1] = Math.Sqrt(summ / (trial - 1));
            return output;
        }
    }
}
