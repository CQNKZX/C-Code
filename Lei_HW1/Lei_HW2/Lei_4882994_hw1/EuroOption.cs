﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lei_4882994_hw1
{
    sealed class EuroOption : Option
    {
        public bool CALL { get; set; }
        public double K { get; set; }
        public double S { get; set; }
        public double T { get; set; }
        public double R { get; set; }
        public double Sig { get; set; }
        public EuroOption(bool call, double k, double s, double t, double r, double sig)
        {
            CALL = call; K = k; S = s; T = t; R = r; Sig = sig;
        }
        public double[] priceoption(EuroOption opt, double[,] randmtx)//compute price with simulator and backward reduction
        {
            double[] finsta = Simulator1.priceevol(opt, randmtx);//get final state
            double[] inistat = bkrdc(opt, finsta);//backward reduction
            double[] output = new double[2];
            double[] temp1 = statstk.std1d(inistat);//method from statstk class
            output[0] = temp1[0]; //option price = mean value of all trials
            output[1] = temp1[1] / Math.Sqrt(randmtx.GetLength(0));//Standard Error
            return output;
        }
        //overload method to extract result of different trials for AVR. won't influence return value so greeks method stay unchanged.
        //public double[] priceoption(EuroOption opt, double[,] randmtx, out double[] trialprice)
        //{
        //    double[] finsta = Simulator1.priceevol(opt, randmtx);//get final state
        //    double[] inistat = bkrdc(opt, finsta);//backward reduction
        //    double[] output = new double[2];
        //    double[] temp1 = statstk.std1d(inistat);//method from statstk class
        //    output[0] = temp1[0]; //option price = mean value of all trials
        //    //output[1] = temp1[1] / Math.Sqrt(randmtx.GetLength(0)); This value is actually useless 
        //    trialprice = inistat;
        //    return output;
        //}
        public double[] bkrdc(EuroOption opt, double[] finstat)//backward reduction given final values of option 
        {
            int trial = finstat.GetLength(0);
            double[] finstate = new double[trial]; //store option value at maturity
            if (opt.CALL == true) //calculate option value at maturity
            {
                for (int i = 0; i < trial; i++)
                {
                    finstate[i] = Math.Max(0, finstat[i] - opt.K);//call
                }
            }
            else if (opt.CALL == false)
            {
                for (int i = 0; i < trial; i++)
                {
                    finstate[i] = Math.Max(0, opt.K - finstat[i]);//put
                }
            }
            double[] inistate = new double[trial]; //store backward reduction value of option
            for (int i = 0; i < trial; i++)
            {
                inistate[i] = finstate[i] * Math.Exp(-opt.R * opt.T); //discount each trial to compute option value and Standard Error
            }
            return inistate;//only one more step to get final result
        }
        public double[] greeks(EuroOption opt, double[,] rndmtx)//calculate greeks for european option
        {
            double[] output = new double[7];
            double[] biubiu = Simulator1.priceevol(opt, rndmtx);//vector that store result of  each trials
            int lenth = biubiu.GetLength(0);
            double[] biubiu0 = new double[lenth];
            double[] biubiu2 = new double[lenth];
            for (int i = 0; i < lenth; i++)
            {
                biubiu0[i] = 0.99 * biubiu[i];
                biubiu2[i] = 1.01 * biubiu[i];
            }
            double[] temp0 = statstk.std1d(bkrdc(opt, biubiu0));
            double[] temp1 = statstk.std1d(bkrdc(opt, biubiu));
            double[] temp2 = statstk.std1d(bkrdc(opt, biubiu2));
            output[0] = (temp2[0] - temp0[0]) / (0.02 * opt.S);//delta
            output[1] = ((temp2[0] - temp1[0]) / (0.01 * opt.S) - (temp1[0] - temp0[0]) / (0.01 * opt.S)) / (0.01 * opt.S);//gamma
            //output[1] = (temp2[0] + temp0[0] - 2 * temp1[0]) / (Math.Pow((0.001 * opt.S), 2));
            EuroOption haha0 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T, opt.R, opt.Sig * 0.999);
            EuroOption haha2 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T, opt.R, opt.Sig * 1.001);
            biubiu0 = Simulator1.priceevol(haha0, rndmtx); temp0 = statstk.std1d(bkrdc(haha0, biubiu0));
            biubiu2 = Simulator1.priceevol(haha2, rndmtx); temp2 = statstk.std1d(bkrdc(haha2, biubiu2));
            output[2] = (temp2[0] - temp0[0]) / (0.002 * opt.Sig);//vega
            int step1 = rndmtx.GetLength(1);
            //double incretime = opt.T / step1;
            double incretime = opt.T * 0.01;
            EuroOption haha22 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T + incretime, opt.R, opt.Sig);
            biubiu2 = Simulator1.priceevol(haha22, rndmtx); temp2 = statstk.std1d(bkrdc(haha22, biubiu2));
            output[3] = -(temp2[0] - temp1[0]) / (incretime);//theta
            EuroOption haha000 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T, opt.R * 0.999, opt.Sig);
            EuroOption haha222 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T, opt.R * 1.001, opt.Sig);
            biubiu0 = Simulator1.priceevol(haha000, rndmtx); temp0 = statstk.std1d(bkrdc(haha000, biubiu0));
            biubiu2 = Simulator1.priceevol(haha222, rndmtx); temp2 = statstk.std1d(bkrdc(haha222, biubiu2));
            output[4] = (temp2[0] - temp0[0]) / (0.002 * opt.R);//rho
            output[5] = temp1[0];//optionprice
            output[6] = temp1[1] / (Math.Sqrt(rndmtx.GetLength(0)));//pricing standard error
            return output;
        }
        //overload pricing method for AVR, only thing different is I want a vector that contains all the trial prices
        public double[] greeksAVR(EuroOption opt, double[,] rndmtx)//calculate greeks for european option
        {
            double[] output = new double[7];
            double[] biubiu = Simulator1.priceevol(opt, rndmtx);//vector that store result of  each trials
            int lenth = biubiu.GetLength(0);
            double[] biubiu0 = new double[lenth];
            double[] biubiu2 = new double[lenth];
            for (int i = 0; i < lenth; i++)
            {
                biubiu0[i] = 0.99 * biubiu[i];
                biubiu2[i] = 1.01 * biubiu[i];
            }
            double[] temp0 = statstk.std1d(bkrdc(opt, biubiu0));
            double[] temp2 = statstk.std1d(bkrdc(opt, biubiu2));

            //Start computing Standard Error
            double[] trialprice = bkrdc(opt, biubiu);
            int lenthh = trialprice.GetLength(0);
            double[] price1 = new double[lenthh / 2];
            //set up price vector of negatively corelated paths
            for (int i = 0; i < (lenthh / 2 - 1); i++)
            {
                price1[i] = (trialprice[i] + trialprice[lenthh - 1 - i]) / 2;//symmetry maintains form random number matrix
            }
            double[] temp1 = statstk.std1d(price1);//std
            //Found Standard Error 
            output[5] = temp1[0];
            output[6] = temp1[1] / (Math.Sqrt(lenthh));
            output[0] = (temp2[0] - temp0[0]) / (0.02 * opt.S);//delta
            output[1] = ((temp2[0] - temp1[0]) / (0.01 * opt.S) - (temp1[0] - temp0[0]) / (0.01 * opt.S)) / (0.01 * opt.S);//gamma
            //output[1] = (temp2[0] + temp0[0] - 2 * temp1[0]) / (Math.Pow((0.001 * opt.S), 2));
            EuroOption haha0 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T, opt.R, opt.Sig * 0.999);
            EuroOption haha2 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T, opt.R, opt.Sig * 1.001);
            biubiu0 = Simulator1.priceevol(haha0, rndmtx); temp0 = statstk.std1d(bkrdc(haha0, biubiu0));
            biubiu2 = Simulator1.priceevol(haha2, rndmtx); temp2 = statstk.std1d(bkrdc(haha2, biubiu2));
            output[2] = (temp2[0] - temp0[0]) / (0.002 * opt.Sig);//vega
            int step1 = rndmtx.GetLength(1);
            //double incretime = opt.T / step1;
            double incretime = opt.T * 0.01;
            EuroOption haha22 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T + incretime, opt.R, opt.Sig);
            biubiu2 = Simulator1.priceevol(haha22, rndmtx); temp2 = statstk.std1d(bkrdc(haha22, biubiu2));
            output[3] = -(temp2[0] - temp1[0]) / (incretime);//theta
            EuroOption haha000 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T, opt.R * 0.999, opt.Sig);
            EuroOption haha222 = new EuroOption(opt.CALL, opt.K, opt.S, opt.T, opt.R * 1.001, opt.Sig);
            biubiu0 = Simulator1.priceevol(haha000, rndmtx); temp0 = statstk.std1d(bkrdc(haha000, biubiu0));
            biubiu2 = Simulator1.priceevol(haha222, rndmtx); temp2 = statstk.std1d(bkrdc(haha222, biubiu2));
            output[4] = (temp2[0] - temp0[0]) / (0.002 * opt.R);//rho
            return output;
        }
    }
}
