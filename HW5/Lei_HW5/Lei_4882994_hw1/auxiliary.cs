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
        public static double[] RowMax(double[,] mike) //return max number of each row
        {
            int row = mike.GetLength(0);
            int col = mike.GetLength(1);
            double[] output = new double[row];
            for (int i = 0; i < row; i++)
            {
                output[i] = mike[i , 0];
                for (int j = 1; j < col; j++)
                {
                    output[i] = ((mike[i,j] > output[i]) ? mike[i,j] : output[i]);
                }
            }
            return output;
        }
        public static double[] RowMin(double[,] mike) //return max number of each row
        {
            int row = mike.GetLength(0);
            int col = mike.GetLength(1);
            double[] output = new double[row];
            for (int i = 0; i < row; i++)
            {
                output[i] = mike[i, 0];
                for (int j = 1; j < col; j++)
                {
                    output[i] = ((mike[i, j] < output[i]) ? mike[i, j] : output[i]);
                }
            }
            return output;
        }
    }
}
