using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lei_4882994_hw1
{
    sealed class auxiliary // provide some matrix operation tools for array
    {
        public static double[,] negativearray(double[,] mike) //nagative array
        {
            int row = mike.GetLength(0);
            int col = mike.GetLength(1);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    mike[i, j] = - mike[i, j];
                }
            }
            return mike;
        }
        public static double[] negativearray(double[] mike) //nagative array
        {
            int row = mike.GetLength(0);
            for (int i = 0; i < row; i++)
            {
                    mike[i] = -mike[i];
            }
            return mike;
        }
        public static double[] sumarray(double[] mike, double[] jack) //array plus array of the same length
        {
            int row = mike.GetLength(0);
            double[] output = new double[row];
            for (int i = 0; i < row; i++)
            {
                output[i] = mike[i] + jack[i];
            }
            return output;
        }
        public static double[] avrgarray(double[] mike, double[] jack) //get average of two array
        {
            int row = mike.GetLength(0);
            double[] output = new double[row];
            for (int i = 0; i < row; i++)
            {
                output[i] = (mike[i] + jack[i]) / 2;
            }
            return output;
        }
    }
}
