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
            double[,] rndmtx = hahaha; //copy info from hahaha
            double[,] priceevol = new double[trial, step + 1]; //matrix than stores price information, I don't use vector because of later overload consideration.
            for (int j = 0; j < trial; j++)
            {
                priceevol[j, 0] = haha.S; //initial condition
            }
            for (int i = 0; i < trial; i++) //price random wal
            {
                for (int j = 1; j < step + 1; j++)
                {
                    priceevol[i, j] = priceevol[i, j - 1] * Math.Exp((haha.R - haha.Sig * haha.Sig / 2) * timestep + haha.Sig * Math.Sqrt(timestep) * rndmtx[i, j - 1]);
                }
            }
            double[] finstate = new double[trial]; // store information at final state
            for (int j = 0; j < trial; j++)
            {
                finstate[j] = priceevol[j, step];
            }
            return finstate;
        }
    }
}
