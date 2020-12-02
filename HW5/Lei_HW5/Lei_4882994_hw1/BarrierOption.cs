using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lei_4882994_hw1
{
    sealed class BarrierOption : Option
    {
        //Barrier initial underlying must meet certain relationships otherwise the option is meaningless from the beginning.
        //inequality relationship
        public double Barrier { get; set; } //barrier price
        public bool Up { get; set; } //determine up or down
        public bool In { get; set; } //determin in or out
        public BarrierOption(bool call, double k, double s, double t, double r, double sig, double barr, bool upp, bool inn)
        {
            CALL = call; K = k; S = s; T = t; R = r; Sig = sig; Barrier = barr; Up = upp; In = inn;
        }
        public double priceoption(BarrierOption opt, double[,] randmtx, bool anti, bool CV, bool multi)
        {
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
        public double[] bkrdc(BarrierOption opt, double[,] finstat)//backward reduction given final values of option 
        {
            int trial = finstat.GetLength(0);
            int lastcol = finstat.GetLength(1) - 1;
            double[] rowmax = new double[1];
            double[] rowmin = new double[1];
            double[] patheff = new double[trial];//determine whether this path is effective
            double[] finstate = new double[trial]; //store option value at maturity
            //need max or min price information along each path, UP/DOWN x IN/OUT
            if (opt.CALL == true) //calculate option value at maturity
            {
                if (opt.Up == true)//up
                {
                    rowmax = auxiliary.RowMax(finstat); // up hence max value
                    for (int i = 0; i < trial; i++)
                    {
                        patheff[i] = (rowmax[i] >= opt.Barrier ? 1 : 0);// 1 means barrier is knocked
                    }
                        if (opt.In == true)//up & in
                        {
                            for (int i = 0; i < trial; i++)
                            {
                                finstate[i] = patheff[i] * Math.Max(0, finstat[i, lastcol] - opt.K);//call
                            }
                        }
                        else //up & out
                        {
                            for (int i = 0; i < trial; i++)
                            {
                                finstate[i] = (1 - patheff[i]) * Math.Max(0, finstat[i, lastcol] - opt.K);//call
                            }
                        }
                }
                else if (opt.Up == false) //down
                {
                    rowmin = auxiliary.RowMin(finstat); // down hence min value
                    for (int i = 0; i < trial; i++)
                    {
                        patheff[i] = (rowmin[i] <= opt.Barrier ? 1 : 0);// 1 means barrier is knocked
                    }
                    if (opt.In == true)//down & in
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = patheff[i] * Math.Max(0, finstat[i, lastcol] - opt.K);//call
                        }
                    }
                    else //down & out
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = (1 - patheff[i]) * Math.Max(0, finstat[i, lastcol] - opt.K);//call
                        }
                    }
                }
            }
            else if (opt.CALL == false) // put option
            {
                if (opt.Up == true)//up
                {
                    rowmax = auxiliary.RowMax(finstat); // up hence max value
                    for (int i = 0; i < trial; i++)
                    {
                        patheff[i] = (rowmax[i] >= opt.Barrier ? 1 : 0);// 1 means barrier is knocked
                    }
                    if (opt.In == true)//up & in
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = patheff[i] * Math.Max(0, opt.K - finstat[i, lastcol]);// put
                        }
                    }
                    else //up & out
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = (1 - patheff[i]) * Math.Max(0, opt.K - finstat[i, lastcol]);//put
                        }
                    }
                }
                else if (opt.Up == false) //down
                {
                    rowmin = auxiliary.RowMin(finstat); // down hence min value
                    for (int i = 0; i < trial; i++)
                    {
                        patheff[i] = (rowmin[i] <= opt.Barrier ? 1 : 0);// 1 means barrier is knocked
                    }
                    if (opt.In == true)//down & in
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = patheff[i] * Math.Max(0, opt.K - finstat[i, lastcol]);//put
                        }
                    }
                    else //down & out
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = (1 - patheff[i]) * Math.Max(0, opt.K - finstat[i, lastcol]);//put
                        }
                    }
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
        public double[] bkrdc(BarrierOption opt, double[,] finstat, double[] deltaCV)
        {
            int trial = finstat.GetLength(0);
            int lastcol = finstat.GetLength(1) - 1;
            double[] rowmax = new double[1];
            double[] rowmin = new double[1];
            double[] patheff = new double[trial];//determine whether this path is effective
            double[] finstate = new double[trial]; //store option value at maturity
            //need max or min price information along each path, UP/DOWN x IN/OUT
            if (opt.CALL == true) //calculate option value at maturity
            {
                if (opt.Up == true)//up
                {
                    rowmax = auxiliary.RowMax(finstat); // up hence max value
                    for (int i = 0; i < trial; i++)
                    {
                        patheff[i] = (rowmax[i] >= opt.Barrier ? 1 : 0);// 1 means barrier is knocked
                    }
                    if (opt.In == true)//up & in
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = patheff[i] * (Math.Max(0, finstat[i, lastcol] - opt.K)) - deltaCV[i];//call
                        }
                    }
                    else //up & out
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = (1 - patheff[i]) * (Math.Max(0, finstat[i, lastcol] - opt.K)) - deltaCV[i];//call
                        }
                    }
                }
                else if (opt.Up == false) //down
                {
                    rowmin = auxiliary.RowMin(finstat); // down hence min value
                    for (int i = 0; i < trial; i++)
                    {
                        patheff[i] = (rowmin[i] <= opt.Barrier ? 1 : 0);// 1 means barrier is knocked
                    }
                    if (opt.In == true)//down & in
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = patheff[i] * (Math.Max(0, finstat[i, lastcol] - opt.K)) - deltaCV[i];//call
                        }
                    }
                    else //down & out
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = (1 - patheff[i]) * (Math.Max(0, finstat[i, lastcol] - opt.K)) - deltaCV[i];//call
                        }
                    }
                }
            }
            else if (opt.CALL == false) // put option
            {
                if (opt.Up == true)//up
                {
                    rowmax = auxiliary.RowMax(finstat); // up hence max value
                    for (int i = 0; i < trial; i++)
                    {
                        patheff[i] = (rowmax[i] >= opt.Barrier ? 1 : 0);// 1 means barrier is knocked
                    }
                    if (opt.In == true)//up & in
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = patheff[i] * (Math.Max(0, opt.K - finstat[i, lastcol])) - deltaCV[i];// put
                        }
                    }
                    else //up & out
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = (1 - patheff[i]) * (Math.Max(0, opt.K - finstat[i, lastcol])) - deltaCV[i];//put
                        }
                    }
                }
                else if (opt.Up == false) //down
                {
                    rowmin = auxiliary.RowMin(finstat); // down hence min value
                    for (int i = 0; i < trial; i++)
                    {
                        patheff[i] = (rowmin[i] <= opt.Barrier ? 1 : 0);// 1 means barrier is knocked
                    }
                    if (opt.In == true)//down & in
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = patheff[i] * (Math.Max(0, opt.K - finstat[i, lastcol])) - deltaCV[i];//put
                        }
                    }
                    else //down & out
                    {
                        for (int i = 0; i < trial; i++)
                        {
                            finstate[i] = (1 - patheff[i]) * (Math.Max(0, opt.K - finstat[i, lastcol])) - deltaCV[i];//put
                        }
                    }
                }
            }
            double[] inistate = new double[trial]; //store backward reduction value of option
            for (int i = 0; i < trial; i++)
            {
                inistate[i] = finstate[i] * Math.Exp(-opt.R * opt.T); //discount each trial to compute option value and Standard Error
            }
            return inistate;//only one more step to get final result
        }


        public double[] greeks(BarrierOption opt, double[,] randmtx, bool anti, bool CV, bool multi)//calculate greeks for european option
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
            BarrierOption iori0 = new BarrierOption(opt.CALL, opt.K, opt.S * 0.99, opt.T, opt.R, opt.Sig, opt.Barrier, opt.Up, opt.In);
            BarrierOption iori1 = new BarrierOption(opt.CALL, opt.K, opt.S * 1.01, opt.T, opt.R, opt.Sig, opt.Barrier, opt.Up, opt.In);
            double p0 = iori0.priceoption(iori0, randmtx, anti, CV, multi);
            double p1 = iori1.priceoption(iori1, randmtx, anti, CV, multi);
            output[2] = (p1 - p0) / (0.02 * opt.S);//delta
            output[3] = ((p1 - price) / (0.01 * opt.S) - (price - p0) / (0.01 * opt.S)) / (0.01 * opt.S);//gamma
            BarrierOption haha0 = new BarrierOption(opt.CALL, opt.K, opt.S, opt.T, opt.R, opt.Sig * 0.999, opt.Barrier, opt.Up, opt.In);
            p0 = opt.priceoption(haha0, randmtx, anti, CV, multi);
            output[4] = (price - p0) / (0.001 * opt.Sig);//vega
            double incretime = opt.T * 0.01;
            BarrierOption haha22 = new BarrierOption(opt.CALL, opt.K, opt.S, opt.T + incretime, opt.R, opt.Sig, opt.Barrier, opt.Up, opt.In);
            p1 = opt.priceoption(haha22, randmtx, anti, CV, multi);
            output[5] = -(p1 - price) / (incretime);//theta
            BarrierOption haha000 = new BarrierOption(opt.CALL, opt.K, opt.S, opt.T, opt.R * 0.999, opt.Sig, opt.Barrier, opt.Up, opt.In);
            p0 = opt.priceoption(haha000, randmtx, anti, CV, multi);
            output[6] = (price - p0) / (0.001 * opt.R);//rho
            return output;
        }
    }
}
