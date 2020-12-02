using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lei_4882994_hw1
{
    //create a class for multithread random number generator
    sealed class objforthread_rand
    {
        //define private variables
        private double[,] inputarray;
        private int rowstartindex;
        private int rowendindex;
        //define properties
        public double[,] Inputarray { get { return inputarray; } }
        public int Rowstartindex { get { return rowstartindex; } }
        public int Rowendindex { get { return rowendindex; } }
        //constructor
        public objforthread_rand(double[,] qqq, int www, int eee)
        {
            inputarray = qqq;
            rowstartindex = www;
            rowendindex = eee;
        }
    }
}
