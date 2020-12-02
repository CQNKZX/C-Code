using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lei_4882994_hw1
{
    sealed class EuroOption : Option
    {
        public EuroOption(bool call, double k, double s, double t, double r, double sig)
        {
            CALL = call; K = k; S = s; T = t; R = r; Sig = sig;
        }
        //compute price with simulator and backward reduction, anti means antithetic reduction, CV means control variate
        public double priceoption(EuroOption opt, double[,] randmtx, bool anti, bool CV, bool multi)
        {
            double[,] finprice1 = new double[1,1];
            double[,] finprice2 = new double[1,1];
            double[] deltaCV1 = new double[1];
            double[] deltaCV2 = new double[1];
            double[] finopt1 = new double[1];
            double[] finopt2 = new double[1];
            double[,] randmtx1 = new double[1, 1];
            double[] inistate1 = new double[1];
            double[] inistate2 = new double[1];

            //calculate final price information
            finprice1 = Simulator1.priceevol(opt, randmtx, multi);//final price,always need to be calculated
            if (anti == true)
            {
                randmtx1 = auxiliary.negativearray(randmtx);
                finprice2 = Simulator1.priceevol(opt, randmtx1, multi);//final price if antithetic reduction used
            }

            //calculate deltaCV if necessary
            if (CV == true)
            {
                deltaCV1 = Simulator1.deltaCV(opt, randmtx, multi);//final price
                if (anti == true)
                {
                    randmtx1 = auxiliary.negativearray(randmtx);
                    deltaCV2 = Simulator1.deltaCV(opt, randmtx1, multi);//final price
                }
            }

            //results of different simulations
            if (CV == false)
            {
                inistate1 = bkrdc(opt, finprice1);
                if (anti == true)
                {
                    inistate2 = bkrdc(opt, finprice2);
                }
            }
            else if (CV == true)
            {
                inistate1 = bkrdc(opt, finprice1, deltaCV1);
                if (anti == true)
                {
                    inistate2 = bkrdc(opt, finprice2, deltaCV2);
                }
            }

            double[] result = new double[1];

            if (anti == false)
            {
                result = inistate1;
            }
            else if (anti == true)
            {
                result = auxiliary.avrgarray(inistate1, inistate2);
            }
            double output = statstk.mean(result);//method from statstk class
            //output[1] = temp1[1] / Math.Sqrt(randmtx.GetLength(0)) SE of computation
            return output;
        }

        //backward reduction given final values of option
        public double[] bkrdc(EuroOption opt, double[,] finstat)//backward reduction given final values of option 
        {
            int trial = finstat.GetLength(0);
            int lastcol = finstat.GetLength(1) - 1;
            double[] finstate = new double[trial]; //store option value at maturity
            if (opt.CALL == true) //calculate option value at maturity
            {
                for (int i = 0; i < trial; i++)
                {
                    finstate[i] = Math.Max(0, finstat[i , lastcol] - opt.K);//call
                }
            }
            else if (opt.CALL == false)
            {
                for (int i = 0; i < trial; i++)
                {
                    finstate[i] = Math.Max(0, opt.K - finstat[i, lastcol]);//put
                }
            }
            double[] inistate = new double[trial]; //store backward reduction value of option
            for (int i = 0; i < trial; i++)
            {
                inistate[i] = finstate[i] * Math.Exp(-opt.R * opt.T); //discount each trial to compute option value and Standard Error
            }
            return inistate;//only one more step to get final result
        }

        //overload backward reduction method for Control Variate
        public double[] bkrdc(EuroOption opt, double[,] finprice, double[] deltaCV)
        {
            int trial = finprice.GetLength(0);
            int lastcol = finprice.GetLength(1) - 1;
            double[] finstate = new double[trial]; //store option value at maturity
            if (opt.CALL == true) //calculate option value at maturity
            {
                for (int i = 0; i < trial; i++)
                {
                    finstate[i] = Math.Max(0, finprice[i , lastcol] - opt.K) - deltaCV[i];//call
                }
            }
            else if (opt.CALL == false)
            {
                for (int i = 0; i < trial; i++)
                {
                    finstate[i] = Math.Max(0, opt.K - finprice[i , lastcol]) - deltaCV[i];//put
                }
            }
            double[] inistate = new double[trial]; //store backward reduction value of option
            for (int i = 0; i < trial; i++)
            {
                inistate[i] = finstate[i] * Math.Exp(-opt.R * opt.T); //discount each trial to compute option value and Standard Error
            }
            return inistate;//only one more step to get final result
        }


        public double[] greeks(EuroOption opt, double[,] randmtx, bool anti, bool CV, bool multi)//calculate greeks for european option
        {
            double[] output = new double[7];
            //decompose price method for calculating delta and gamma and standard error
            double[,] finprice1 = new double[1, 1];
            double[,] finprice2 = new double[1, 1];
            double[] deltaCV1 = new double[1];
            double[] deltaCV2 = new double[1];
            double[] finopt1 = new double[1];
            double[] finopt2 = new double[1];
            double[,] randmtx1 = new double[1, 1];
            double[] inistate1 = new double[1];
            double[] inistate2 = new double[1];

            //calculate final price information
            finprice1 = Simulator1.priceevol(opt, randmtx, multi);//final price
            if (anti == true)
            {
                randmtx1 = auxiliary.negativearray(randmtx);
                finprice2 = Simulator1.priceevol(opt, randmtx1, multi);//final price if antithetic reduction used
            }

            //calculate deltaCV if necessary
            if (CV == true)
            {
                deltaCV1 = Simulator1.deltaCV(opt, randmtx, multi);//final price
                if (anti == true)
                {
                    randmtx1 = auxiliary.negativearray(randmtx);
                    deltaCV2 = Simulator1.deltaCV(opt, randmtx1, multi);//final price
                }
            }

            //results of different simulations
            if (CV == false)
            {
                inistate1 = bkrdc(opt, finprice1);
                if (anti == true)
                {
                    inistate2 = bkrdc(opt, finprice2);
                }
            }
            else if (CV == true)
            {
                inistate1 = bkrdc(opt, finprice1, deltaCV1);
                if (anti == true)
                {
                    inistate2 = bkrdc(opt, finprice2, deltaCV2);
                }
            }

            double[] result = new double[1];

            if (anti == false)
            {
                result = inistate1;
            }
            else if (anti == true)
            {
                result = auxiliary.avrgarray(inistate1, inistate2);
            }
            double price = statstk.mean(result);//optionprice
            output[0] = price;
            output[1] = statstk.std(result) / Math.Sqrt(randmtx.GetLength(0)); //SE of computation
            EuroOption iori0 = new EuroOption(opt.CALL, opt.K, opt.S * 0.99, opt.T, opt.R, opt.Sig);
            EuroOption iori1 = new EuroOption(opt.CALL, opt.K, opt.S * 1.01, opt.T, opt.R, opt.Sig);
            double p0 = iori0.priceoption(iori0, randmtx, anti, CV, multi);
            double p1 = iori1.priceoption(iori1, randmtx, anti, CV, multi);
            output[2] = (p1 - p0) / (0.02 * opt.S);//delta
            output[3] = ((p1 - price) / (0.01 * opt.S) - (price - p0) / (0.01 * opt.S)) / (0.01 * opt.S);//gamma
            EuroOption haha0 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T, opt.R, opt.Sig * 0.999);
            p0 = opt.priceoption(haha0, randmtx, anti, CV, multi);
            output[4] = (price - p0) / (0.001 * opt.Sig);//vega
            double incretime = opt.T * 0.01;
            EuroOption haha22 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T + incretime, opt.R, opt.Sig);
            p1 = opt.priceoption(haha22, randmtx, anti, CV, multi);
            output[5] = -(p1 - price) / (incretime);//theta
            EuroOption haha000 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T, opt.R * 0.999, opt.Sig);
            p0 = opt.priceoption(haha000, randmtx, anti, CV, multi);
            output[6] = (price - p0) / (0.001 * opt.R);//rho
            return output;
        }
    }
}
