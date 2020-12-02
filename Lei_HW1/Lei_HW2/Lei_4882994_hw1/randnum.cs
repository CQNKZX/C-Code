using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lei_4882994_hw1
{
    sealed class randnum //whenever you need random numbers, come here
    {
        public double[,] rndmtx(int row, int col)//generate random number matrix
        {
            double temp1 = 0; double temp2 = 0; double v1 = 0; double v2 = 0;// store value in iteration
            double c1 = 0;//parameters
            Random rnd = new Random();//instantiate random class
            double[,] output = new double[row, col];
            for (int row1 = 0; row1 < row; row1++)
            {
                for (int col1 = 0; col1 < col; col1 += 2)
                {

                    //while (col - col1 > 1)
                    double w = 0;
                    while (w == 0 || w > 1)
                    {
                        temp1 = rnd.NextDouble();
                        temp2 = rnd.NextDouble();
                        v1 = 2 * temp1 - 1; v2 = 2 * temp2 - 1;
                        w = v1 * v1 + v2 * v2;
                    }
                    c1 = Math.Sqrt(-2 * Math.Log(w) / w);
                    if (col - col1 > 1) // index is so easy to confuse
                    {
                        output[row1, col1] = c1 * v1;
                        output[row1, col1 + 1] = c1 * v2;
                    }
                    else if (col - col1 <= 1)
                    {
                        output[row1, col1] = c1 * v1;
                    }
                }
            }
            return output;
        }
        public double[,] AVRrndmtx(int row, int col)//generate random numbers with AVR future
        {
            if (row % 2 != 0) row = row + 1;//you cannot run AVR if trial is odd number, so fix it by adding 1
            int row1 = row / 2;
            double[,] half1 = new double[row1,col];
            double[,] output = new double[row,col];//will comsume more memory than ordinary method
            randnum biubiubiu = new randnum();//instantiate
            half1 = biubiubiu.rndmtx(row1, col);// call method
            for (int i = 0; i < row1; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    output[i, j] = half1[i, j];
                    output[row - 1 - i, j] = -half1[i, j];//fill output matrix with half1 and its negative mirror image
                }
            }
            return output;

        }
    }
}
