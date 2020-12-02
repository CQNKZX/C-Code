// if you only want code, this is it. I'm not sure which files should I discard, so I turn in a whole project and a single txt.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lei_4882994_hw5
{
    class Program
    {
        static void Main(string[] args)
        {
            int a, b, c;
            double rnd1 = 0; double rnd2 = 0; double rnd1p = 0; double rnd2p = 0;// store values of random numbers
            double temp1 = 0; double temp2 = 0; double v1 = 0; double v2 = 0;// store value in iteration
            double rho = 0; double w = 0; double c1 = 0;//parameters in some method
            Random rnd = new Random();
            Console.WriteLine("Generate random numbers or random pairs? Press 1 for numbers, 2 for pairs");//choose number or pair
            a = Convert.ToInt32(Console.ReadLine());
            if (a == 2)//if pair is chosen, specify correlation rho
            {
                Console.WriteLine("if you want pairs, please enter the correaltion coefficient between 0 and 1");
                rho = Convert.ToDouble(Console.ReadLine());
            }
            Console.WriteLine("Which method you would like to use? Press 1 for 12 numbers, 2 for Box-Muller, 3 for polar rejection");
            b = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("How many pairs of number or pair you want to generate?");
            c = Convert.ToInt32(Console.ReadLine());
            switch(b)//specify method based on b.
            {
                case 1: //12 number method
                    for (int count = c; count > 0; count--)
                    {
                        for (int count1 = 12; count1 > 0; count1--)
                        {
                            temp1 = rnd.NextDouble();
                            temp2 = rnd.NextDouble();
                            rnd1 += temp1;
                            rnd2 += temp2;
                        }
                        rnd1 -= 6;
                        rnd2 -= 6;
                        if (a == 1)// if you choose number
                        {
                            Console.WriteLine(rnd1);
                            Console.WriteLine(rnd2);
                        }
                        else// if you choose pair
                        {
                            rnd1p = rnd1;
                            rnd2p = rho * rnd1 + Math.Sqrt(1 - rho * rho) * rnd2;
                            Console.WriteLine("(" + rnd1p + "," + rnd2p + ")");
                        }
                    }
                break;
                case 2://box-muller method
                    for (int count = c; count > 0; count--)
                    {
                        temp1 = rnd.NextDouble();
                        temp2 = rnd.NextDouble();
                        if (temp1 + temp2 == 0)//temp1 and temp2 may equal 0, though the prabobility is almost 0
                        {
                            count++;// modify count while not generating numbers.
                        }
                        else
                        {
                            rnd1 = Math.Sqrt(-2 * Math.Log(temp1)) * Math.Cos(2 * Math.PI * temp2);
                            rnd2 = Math.Sqrt(-2 * Math.Log(temp1)) * Math.Sin(2 * Math.PI * temp2);
                            if (a == 1)// if you choose numbers
                            {
                                Console.WriteLine(rnd1);
                                Console.WriteLine(rnd2);
                            }
                            else// if you choose pairs
                            {
                                rnd1p = rnd1;
                                rnd2p = rho * rnd1 + Math.Sqrt(1 - rho * rho) * rnd2;
                                Console.WriteLine("(" + rnd1p + "," + rnd2p + ")");
                            }
                        }
                    }
                break;
                case 3:// polar rejection method
                    for (int count = c; count > 0; count--)
                    {
                        temp1 = rnd.NextDouble();
                        temp2 = rnd.NextDouble();
                        v1 = 2 * temp1 - 1; v2 = 2 * temp2 - 1;//adjust the distribution interval form [0 1] to [-1 1]
                        w = v1 * v1 + v2 * v2;
                        if (w == 0 || w > 1)//reject and modify count
                        {
                            count++;
                        }
                        else
                        {
                            c1 = Math.Sqrt(-2 * Math.Log(w) / w);
                            rnd1 = c1 * v1;
                            rnd2 = c1 * v2;
                            if (a == 1)//output numbers
                            {
                                Console.WriteLine(rnd1);
                                Console.WriteLine(rnd2);
                            }
                            else// output pairs
                            {
                                rnd1p = rnd1;
                                rnd2p = rho * rnd1 + Math.Sqrt(1 - rho * rho) * rnd2;
                                Console.WriteLine("(" + rnd1p + "," + rnd2p + ")");
                            }
                        }
                    }
                break;
            }
            Console.ReadLine();
        }
    }
}

