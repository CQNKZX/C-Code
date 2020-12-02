using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lei_4882994_hw1
{
    sealed class Simulator1//provide supports for computing option prices
    {
        public static double[] priceevol(EuroOption haha, double[,] hahaha) //compute underlying price evolution and output state at maturity
        {
            int trial = hahaha.GetLength(0);//get dimensions for random number matrix
            int step = hahaha.GetLength(1);
            double timestep = haha.T / step; //time increment
            double[,] rndmtx = hahaha; //copy info from input random matrix
            double[] priceevol = new double[trial]; //matrix that stores price change information
            for (int i = 0; i < trial; i++)
            {
                priceevol[i] = haha.S; //initial condition
            }
            for (int i = 0; i < trial; i++) //price random walk
            {
                for (int j = 0; j < step; j++)
                {
                    priceevol[i] = priceevol[i] * Math.Exp((haha.R - haha.Sig * haha.Sig / 2) * timestep + haha.Sig * Math.Sqrt(timestep) * rndmtx[i, j]);
                }
            }
            return priceevol;
        }
    }
}
