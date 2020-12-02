using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lei_4882994_hw1
{
    class objforthread_sim
    {
        //define private variables
        private Option opt;
        private double[,] input_rand;
        private double[,] target_arr;
        private double[] target_arr_cv;
        private int rowstartindex;
        private int rowendindex;
        //define properties
        public Option Opt { get { return opt; } }
        public double[,] Input_rand { get { return input_rand; } }
        public double[,] Target_arr { get { return target_arr; } }
        public double[] Target_arr_cv { get { return target_arr_cv; } }
        public int Rowstartindex { get { return rowstartindex; } }
        public int Rowendindex { get { return rowendindex; } }
        //constructor
        public objforthread_sim(Option aaa, double[,] qqq, double[,] sss, int www, int eee)
        {
            opt = aaa;
            input_rand = qqq;
            target_arr = sss;
            rowstartindex = www;
            rowendindex = eee;
        }
        public objforthread_sim(Option aaa, double[,] qqq, double[] ssss, int www, int eee)
        {
            opt = aaa;
            input_rand = qqq;
            target_arr_cv = ssss;
            rowstartindex = www;
            rowendindex = eee;
        }
    }
}
