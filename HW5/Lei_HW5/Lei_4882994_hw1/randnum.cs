using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lei_4882994_hw1
{
    sealed class randnum //whenever you need random numbers, come here
    {
        public double[,] rndmtx(int row, int col, bool multi)//generate random number matrix
        {
            double[,] output = new double[row, col];
            if (multi == false)
            {
                objforthread_rand singlecore = new objforthread_rand(output, 0, output.GetLength(0) - 1);
                randnum.fill_specified_rows(singlecore);
            }
            else if (multi == true)
            {
                //crucial: no more than one threads can have access to the same element in array double[,]
                //that's why I don't need to use lock syntax
                //how many logical processor we have on this computer
                //I did some test, when number of threads is equal to number of logical processor, computation speed seems to be fastest
                int cores = System.Environment.ProcessorCount;
                //allocate index for different threads
                int numofindex = row / cores;//take advantage of this automatic round function in int / int
                int[,] indexarr = new int[cores, 2];
                for (int i = 0; i < cores; i++)
                {
                    indexarr[i, 0] = i * numofindex;
                    indexarr[i, 1] = (i == (cores - 1) ? (row - 1) : ((i + 1) * numofindex - 1));
                }
                //create threads and input objects for corresponding threads
                Thread[] tarr = new Thread[cores];
                objforthread_rand[] objarr = new objforthread_rand[cores];
                for (int i = 0; i < cores; i++)
                {
                    tarr[i] = new Thread(randnum.fill_specified_rows);
                }
                for (int i = 0; i < cores; i++)
                {
                    objarr[i] = new objforthread_rand(output, indexarr[i, 0], indexarr[i, 1]);
                }
                //fire them all then wait
                for (int i = 0; i < cores; i++)
                {
                    tarr[i].Start(objarr[i]);
                }
                for (int i = 0; i < cores; i++)
                {
                    tarr[i].Join();
                }
            }
            return output;
        }
        public double[,] AVRrndmtx(int row, int col, bool multi)//generate random numbers with AVR future
        {
            //cut its size by half because we are gonna use it twice( original once and negative once)
            randnum biubiubiu = new randnum();//instantiate
            return biubiubiu.rndmtx(row / 2, col, multi);
            //cut its size by half because of AVR
        }
        static void fill_specified_rows(object notreally)//not really all objects fit in
        {
            //get back to type we want
            objforthread_rand lala = (objforthread_rand)notreally;

            //get size of Inputarray
            int col = lala.Inputarray.GetLength(1);

            //just define some parameters for computation
            double temp1 = 0;
            double temp2 = 0;
            double v1 = 0;
            double v2 = 0;
            double c1 = 0;

            //avoid duplicated Random instance
            int seed = (int)DateTime.Now.Ticks;
            Random rnd = new Random(seed + lala.Rowstartindex);//instantiate random class
            for (int i = lala.Rowstartindex; i <= lala.Rowendindex; i++)
            {
                for (int j = 0; j < col; j += 2)
                {
                    double w = 0;
                    while (w == 0 || w > 1)
                    {
                        temp1 = rnd.NextDouble();
                        temp2 = rnd.NextDouble();
                        v1 = 2 * temp1 - 1; v2 = 2 * temp2 - 1;
                        w = v1 * v1 + v2 * v2;
                    }
                    c1 = Math.Sqrt(-2 * Math.Log(w) / w);
                    if (col - j > 1) // index is so easy to confuse
                    {
                        lala.Inputarray[i, j] = c1 * v1;
                        lala.Inputarray[i, j + 1] = c1 * v2;
                    }
                    else if (col - j <= 1)
                    {
                        lala.Inputarray[i, j] = c1 * v1;
                    }
                }
            }
        }
    }
}
