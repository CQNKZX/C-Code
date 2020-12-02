using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lei_4882994_hw1
{
    sealed class BlackScholes // waiting for completion
    {
        //d1=@(S,K,sigma,r,T)(log(S/K)+(r+sigma^2/2)*T)/(sigma*sqrt(T));
        static double d1 (EuroOption qwe) //d1 is a notation from BSmodel
        {
            double output = (Math.Log(qwe.S / qwe.K) + (qwe.R + qwe.Sig * qwe.Sig / 2) * qwe.T) / (qwe.Sig * Math.Sqrt(qwe.T));
            return output;
        }
        //calldelta=@(S,K,sigma,r,T)cdf('norm',d1(S,K,sigma,r,T),0,1);
        //putdelta=@(S,K,sigma,r,T)cdf('norm',d1(S,K,sigma,r,T),0,1)-1;
        public static double delta(EuroOption qwe)
        {
            double output= 0;
            double d = d1(qwe);
            if (qwe.CALL == true)
            {
                output = Distributions.stdCDF(d);
            }
            else if (qwe.CALL == false)
            {
                output = Distributions.stdCDF(d) - 1;
            }
            return output;
        }
    }
}
