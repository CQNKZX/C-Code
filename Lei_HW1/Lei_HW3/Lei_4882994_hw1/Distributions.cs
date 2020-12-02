using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lei_4882994_hw1
{
    sealed class Distributions
    {
        /// Returns the PDF of the standard normal distribution.
        public static double stdPDF(double x)
        {
            const double SqrtTwoPiInv = 0.398942280401433;
            return SqrtTwoPiInv * Math.Exp(-0.5 * x * x);
        }
        /// Returns the CDF of the standard normal distribution.
        public static double stdCDF(double x)
        {
            //Approimation based on Abramowitz & Stegun (1964)

            if (x < 0)
                return 1.0 - stdCDF(-x);
            const double b0 = 0.2316419;
            const double b1 = 0.319381530;
            const double b2 = -0.356563782;
            const double b3 = 1.781477937;
            const double b4 = -1.821255978;
            const double b5 = 1.330274429;
            double pdf = stdPDF(x);
            double a = 1.0 / (1.0 + b0 * x);
            return 1.0 - pdf * (b1 * a + b2 * Math.Pow(a, 2) + b3 * Math.Pow(a, 3) + b4 * Math.Pow(a, 4) + b5 * Math.Pow(a, 5));
        }
    }
}
