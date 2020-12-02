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
        private EuroOption euopt;
        private double[,] input_rand;
        private double[] target_arr;
        private int rowstartindex;
        private int rowendindex;
        //define properties
        public EuroOption Euopt { get { return euopt; } }
        public double[,] Input_rand { get { return input_rand; } }
        public double[] Target_arr { get { return target_arr; } }
        public int Rowstartindex { get { return rowstartindex; } }
        public int Rowendindex { get { return rowendindex; } }
        //constructor
        public objforthread_sim(EuroOption aaa, double[,] qqq, double[] sss, int www, int eee)
        {
            euopt = aaa;
            input_rand = qqq;
            target_arr = sss;
            rowstartindex = www;
            rowendindex = eee;
        }
    }
}
