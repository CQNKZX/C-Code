using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lei_4882994_hw1
{
    //Simulator is initially created but I use Simulator1 in stead, which is about 30%faster, however I dicide to keep this one because I may use it later
    sealed class Simulator //methods in this class should have overload property
    {
        //public int TRIAL { get; set; }
        //public int STEP { get; set; }
        public static double[] getprice(EuroOption haha, double[,] hahaha) //compute option price and Standard Error
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
            for (int i = 0; i < trial; i++) //price random walk
            {
                for (int j = 1; j < step + 1; j++)
                {
                    priceevol[i, j] = priceevol[i, j - 1] * Math.Exp((haha.R - haha.Sig * haha.Sig / 2) * timestep + haha.Sig * Math.Sqrt(timestep) * rndmtx[i, j - 1]);
                }
            }
            double[] finstate = new double[trial]; // store information at final state
            if (haha.CALL == true) //calculate option value at maturity
            {
                for (int i = 0; i < trial; i++)
                {
                    finstate[i] = Math.Max(0, priceevol[i, step] - haha.K);
                }
            }
            else if (haha.CALL == false)
            {
                for (int i = 0; i < trial; i++)
                {
                    finstate[i] = Math.Max(0, haha.K - priceevol[i, step]);
                }
            }
            double[] inistate = new double[trial];
            for (int i = 0; i < trial; i++)
            {
                inistate[i] = finstate[i] * Math.Exp(-haha.R * haha.T); //discount each trial to compute option value and Standard Error
            }
            double summ = 0;
            foreach (double asd in inistate)
            {
                summ = summ + asd;
            }
            double[] output = new double[2];
            output[0] = summ / trial; //average and discount
            double[] temp1 = inistate; //compute  Standard Error
            for (int i = 0; i < trial; i++)
            {
                temp1[i] = (inistate[i] - output[0]) * (inistate[i] - output[0]);
            }
            summ = 0;
            foreach (double asd in temp1)
            {
                summ = summ + asd;
            }
            output[1] = Math.Sqrt(summ / (trial - 1)) / Math.Sqrt(trial);
            return output;
        }
        public static double[] getgreeks(EuroOption haha, double[,] hahaha) //compute greeks
        {
            double[] greeks = new double[5];
            EuroOption haha0 = new EuroOption(haha.CALL, haha.K, haha.S * 0.999, haha.T, haha.R, haha.Sig);
            EuroOption haha2 = new EuroOption(haha.CALL, haha.K, haha.S * 1.001, haha.T, haha.R, haha.Sig);
            double[] price0 = getprice(haha0, hahaha);
            double[] price1 = getprice(haha, hahaha);
            double[] price2 = getprice(haha2, hahaha);
            greeks[0] = (price2[0] - price0[0]) / (0.002 * haha.S);//delta
            greeks[1] = ((price2[0] - price1[0]) / (0.001 * haha.S) - (price1[0] - price0[0]) / (0.001 * haha.S)) / (0.001 * haha.S);
            //greeks[1] = (price2[0] + price0[0] - 2* price1[0]) / (Math.Pow( 0.001 * haha.S, 2));//gamma
            EuroOption haha00 = new EuroOption(haha.CALL, haha.K, haha.S, haha.T, haha.R, haha.Sig * 0.999);
            EuroOption haha22 = new EuroOption(haha.CALL, haha.K, haha.S, haha.T, haha.R, haha.Sig * 1.001);
            price0 = getprice(haha00, hahaha);
            price2 = getprice(haha22, hahaha);
            greeks[2] = (price2[0] - price0[0]) / (0.002 * haha.Sig);//vega
            int step1 = hahaha.GetLength(1);
            double incretime = haha.T / step1;
            EuroOption haha222 = new EuroOption(haha.CALL, haha.K, haha.S, haha.T + incretime, haha.R, haha.Sig);
            price2 = getprice(haha222, hahaha);
            greeks[3] = -(price2[0] - price1[0]) / incretime;//theta
            EuroOption haha0000 = new EuroOption(haha.CALL, haha.K, haha.S, haha.T, haha.R * 0.999, haha.Sig);
            EuroOption haha2222 = new EuroOption(haha.CALL, haha.K, haha.S, haha.T, haha.R * 1.001, haha.Sig);
            price0 = getprice(haha0000, hahaha);
            price2 = getprice(haha2222, hahaha);
            greeks[4] = (price2[0] - price0[0]) / (0.002 * haha.R);//vaga
            return greeks;
        }
    }
}
