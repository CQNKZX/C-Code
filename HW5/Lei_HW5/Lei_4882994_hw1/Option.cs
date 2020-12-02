using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lei_4882994_hw1
{
    class Option // information will be added when more types of option come into this project
    {
        //shared properties for each option
        public double S { get; set; }
        public double T { get; set; }
        public double R { get; set; }
        public double Sig { get; set; }
        //not share but must put them here because we are forced to implement Black-Scholes Control Variate
        public bool CALL { get; set; }
        public double K { get; set; }
    }
}
