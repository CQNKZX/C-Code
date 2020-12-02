using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lei_4882994_hw1
{
    sealed class Simulator1//simulate price evolution and get results
    {
        //Kind of confused about the automatic get/set property, you'd better create a new instance 
        public static double[] priceevol(EuroOption haha, double[,] hahaha, bool multi) //compute underlying price evolution and output state at maturity
        {
            //how many rows do we have
            int row = hahaha.GetLength(0);
            //matrix that stores price change information
            double[] priceevol = new double[row];
            if (multi == false)
            {
                objforthread_sim singlecore = new objforthread_sim(haha, hahaha, priceevol, 0, row - 1);
                Simulator1.fill_rows_price(singlecore);
            }
            else if (multi == true)
            {
                //crucial: no more than one threads can have access to the same element in array double[,]
                //that's why I don't need to use lock syntax
                //how many logical processor we have on this computer
                //I did some test, when number of threads is equal to number of logical processor, computation speed seems to be fastest
                int cores = System.Environment.ProcessorCount;

                //allocate index for different threads
                //allocate different rows to different thread
                int numofindex = row / cores;//take advantage of this automatic round function in int / int
                int[,] indexarr = new int[cores, 2];
                for (int i = 0; i < cores; i++)
                {
                    indexarr[i, 0] = i * numofindex;
                    //last thread should have all things left
                    indexarr[i, 1] = (i == (cores - 1) ? (row - 1) : ((i + 1) * numofindex - 1));
                }

                //create threads and input objects for corresponding threads
                Thread[] t_arr = new Thread[cores];
                objforthread_sim[] obj_arr = new objforthread_sim[cores];
                for (int i = 0; i < cores; i++)
                {
                    t_arr[i] = new Thread(Simulator1.fill_rows_price);
                }
                for (int i = 0; i < cores; i++)
                {
                    obj_arr[i] = new objforthread_sim(haha, hahaha, priceevol, indexarr[i, 0], indexarr[i, 1]);
                }

                //fire them all then wait
                for (int i = 0; i < cores; i++)
                {
                    t_arr[i].Start(obj_arr[i]);
                }
                for (int i = 0; i < cores; i++)
                {
                    t_arr[i].Join();
                }
            }
            return priceevol;
        }

        //define method for priceevol multi-threading
        static void fill_rows_price(object notreally)//Again, not really all objects fit in
        {
            //get back to type we want
            objforthread_sim lala = (objforthread_sim)notreally;

            //get size of Target double[]
            int col = lala.Target_arr.GetLength(0);

            //get size of input random double[,]
            int trial = lala.Input_rand.GetLength(0);//get dimensions for random number matrix
            int step = lala.Input_rand.GetLength(1);

            //time step information from option
            double timestep = lala.Euopt.T / step; //time increment
            double rate = lala.Euopt.R;
            double vol = lala.Euopt.Sig;

            //simulation and pump data into Target array
            for (int i = lala.Rowstartindex; i < lala.Rowendindex; i++) //initial price
            {
                lala.Target_arr[i] = lala.Euopt.S;
            }
            for (int i = lala.Rowstartindex; i < lala.Rowendindex; i++) //price random walk
            {
                for (int j = 0; j < step; j++)
                {
                    lala.Target_arr[i] = lala.Target_arr[i] * Math.Exp((rate - vol * vol / 2) * timestep + vol * Math.Sqrt(timestep) * lala.Input_rand[i, j]);
                }
            }
        }


        //I decide to decompose the computation of price evolvement and Control Variate
        //method which provide control variate information 
        public static double[] deltaCV(EuroOption haha3, double[,] hahaha, bool multi)
        {
            //how many rows do we have
            int row = hahaha.GetLength(0);
            //matrix that stores price change information
            EuroOption hahah1 = new EuroOption(haha3.CALL, haha3.K, haha3.S, haha3.T, haha3.R, haha3.Sig);
            double[] output = new double[row];
            if (multi == false)
            {
                objforthread_sim singlecore = new objforthread_sim(hahah1, hahaha, output, 0, row - 1);
                Simulator1.fill_rows_deltaCV(singlecore);
            }
            else if (multi == true)
            {
                //crucial: no more than one threads can have access to the same element in array double[,]
                //that's why I don't need to use lock syntax
                //how many logical processor we have on this computer
                //I did some test, when number of threads is equal to number of logical processor, computation speed seems to be fastest
                int cores = System.Environment.ProcessorCount;

                //allocate index for different threads
                //allocate different rows to different thread
                int numofindex = row / cores;//take advantage of this automatic round function in int / int
                int[,] indexarr = new int[cores, 2];
                for (int i = 0; i < cores; i++)
                {
                    indexarr[i, 0] = i * numofindex;
                    //last thread should have all things left
                    indexarr[i, 1] = (i == (cores - 1) ? (row - 1) : ((i + 1) * numofindex - 1));
                }

                //create threads and input objects for corresponding threads
                Thread[] t_arr = new Thread[cores];
                objforthread_sim[] obj_arr = new objforthread_sim[cores];
                for (int i = 0; i < cores; i++)
                {
                    t_arr[i] = new Thread(Simulator1.fill_rows_deltaCV);
                }
                for (int i = 0; i < cores; i++)
                {
                    obj_arr[i] = new objforthread_sim(hahah1, hahaha, output, indexarr[i, 0], indexarr[i, 1]);
                }

                //fire them all then wait
                for (int i = 0; i < cores; i++)
                {
                    t_arr[i].Start(obj_arr[i]);
                }
                for (int i = 0; i < cores; i++)
                {
                    t_arr[i].Join();
                }
            }
            return output;
        }



        //define method for deltaCV multi-threading
        static void fill_rows_deltaCV(object notreally)//Again, not really all objects fit in
        {
            //get back to type we want
            objforthread_sim lala = (objforthread_sim)notreally;

            //haha is an identical option with haha1
            EuroOption hehe1 = new EuroOption(lala.Euopt.CALL, lala.Euopt.K, lala.Euopt.S, lala.Euopt.T, lala.Euopt.R, lala.Euopt.Sig);

            //get size of Target double[]
            int col = lala.Target_arr.GetLength(0);

            //get size of input random double[,]
            int trial = lala.Input_rand.GetLength(0);//get dimensions for random number matrix
            int step = lala.Input_rand.GetLength(1);

            //time step information from option
            double timestep = hehe1.T / step; //time increment
            double rate = hehe1.R;
            double vol = hehe1.Sig;
            double maturity = hehe1.T;
            double onestepinc = Math.Exp(hehe1.R * timestep);
            double[] priceevol = new double[trial];//array that stores price information
            double[] priceevol0 = new double[trial];  //array that stores price information, the two are alternatively used

            //simulation and pump data into Target array
            for (int i = lala.Rowstartindex; i < lala.Rowendindex; i++)
            {
                priceevol0[i] = hehe1.S; //initial condition
            }
            for (int i = lala.Rowstartindex; i < lala.Rowendindex; i++) //price random walk
            {
                for (int j = 0; j < step; j++)
                {
                    hehe1.T = maturity - j * timestep;
                    hehe1.S = priceevol0[i];
                    priceevol[i] = priceevol0[i] * Math.Exp((rate - vol * vol / 2) * timestep + vol * Math.Sqrt(timestep) * lala.Input_rand[i, j]);
                    double deltaj = BlackScholes.delta(hehe1);
                    lala.Target_arr[i] = lala.Target_arr[i] + deltaj * (priceevol[i] - priceevol0[i] * onestepinc);
                    priceevol0[i] = priceevol[i];
                }
            }
        }
    }
}
