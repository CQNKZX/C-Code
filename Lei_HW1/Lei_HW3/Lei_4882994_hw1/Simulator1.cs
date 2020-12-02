using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lei_4882994_hw1
{
    sealed class Simulator1//simulate price evolution and get results
    {
        public static double[] priceevol(EuroOption haha, double[,] hahaha) //compute underlying price evolution and output state at maturity
        {
            int trial = hahaha.GetLength(0);//get dimensions for random number matrix
            int step = hahaha.GetLength(1);
            double timestep = haha.T / step; //time increment
            double[] priceevol = new double[trial]; //matrix that stores price change information
            for (int i = 0; i < trial; i++)
            {
                priceevol[i] = haha.S; //initial condition
            }
            for (int i = 0; i < trial; i++) //price random walk
            {
                for (int j = 0; j < step; j++)
                {
                    priceevol[i] = priceevol[i] * Math.Exp((haha.R - haha.Sig * haha.Sig / 2) * timestep + haha.Sig * Math.Sqrt(timestep) * hahaha[i, j]);
                }
            }
            return priceevol;
        }
        //I decide to decompose the computation of price evolvement and Control Variate
        public static double[] deltaCV(EuroOption haha1, double[,] hahaha)//price evolvement under delta-hedge
        {
            EuroOption haha = new EuroOption(haha1.CALL, haha1.K, haha1.S, haha1.T, haha1.R, haha1.Sig);//haha is identical with haha1
            int trial = hahaha.GetLength(0);  //get dimensions for random number matrix
            int step = hahaha.GetLength(1);
            double maturity = haha.T;
            double timestep = haha.T / step;  //time increment
            double onestepinc = Math.Exp(haha.R * timestep);
            double[] priceevol = new double[trial];//array that stores price information
            double[] priceevol0 = new double[trial];  //array that stores price information, the two are alternatively used
            double[] output = new double[trial];
            for (int i = 0; i < trial; i++)
            {
                priceevol0[i] = haha.S; //initial condition
            }
            for (int i = 0; i < trial; i++) //price random walk
            {
                for (int j = 0; j < step; j++)
                {
                    haha.T = maturity - j * timestep;
                    haha.S = priceevol0[i];
                    priceevol[i] = priceevol0[i] * Math.Exp((haha.R - haha.Sig * haha.Sig / 2) * timestep + haha.Sig * Math.Sqrt(timestep) * hahaha[i, j]);
                    double deltaj = BlackScholes.delta(haha);
                    //should we let the price difference increase by a riskfree rate ???
                    //output[i] = output[i] + deltaj * (priceevol[i] - priceevol0[i] * onestepinc) * Math.Exp(timestep * (step - 1 - j));
                    output[i] = output[i] + deltaj * (priceevol[i] - priceevol0[i] * onestepinc);
                    priceevol0[i] = priceevol[i];
                }
            }
            return output;
        }
    }
}
