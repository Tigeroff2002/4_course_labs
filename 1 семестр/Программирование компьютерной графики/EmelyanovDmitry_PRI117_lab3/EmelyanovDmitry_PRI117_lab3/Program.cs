using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmelyanovDmitry_PRI117_lab3
{
    class Program
    {
        private static async Task<double> CalcFormula(double xi)
        {
            double CalcFirst(double x)
            {
                return 1 / (Math.Pow((x - 2), 2) - x - 1);
            }

            double CalcSecond(double x)
            {
                return Math.Log(5 * x);
            }

            double CalcThird(double x)
            {
                return Math.Exp(7 * Math.Sqrt(x));
            }

            double CalcFourth(double x)
            {
                return 0.3 * (Math.Pow(x, 3) + Math.Pow(x, 2) - 1);
            }

            var result1Task = Task.Run(() => CalcFirst(xi)); 
            var result2Task = Task.Run(() => CalcSecond(xi)); 
            var result3Task = Task.Run(() => CalcThird(xi)); 
            var result4Task = Task.Run(() => CalcFourth(xi));

            return
                await result1Task.ConfigureAwait(false) +
                await result2Task.ConfigureAwait(false) +
                await result3Task.ConfigureAwait(false) +
                await result4Task.ConfigureAwait(false);
        }

        private static double CalcXi(int a, int i, double h)
        {
            return a + i * h;
        }

        static async Task Main(string[] args)
        {
            var a = 0;
            var b = 6;
            var n = 35;
            var h = (b - a) / (double)n;

            var results = await Task.WhenAll(
                Enumerable.Range(0, n + 1 )
                .Select(i => 
                    Task.Run(() => CalcFormula(CalcXi(a, i, h)))))
                .ConfigureAwait(false);

            for (int i = 0; i < results.Length; i++)
            {
                Console.WriteLine("Номер итерации: " + i
                    + ", Результат вычисления: " + results[i]);
            }

            Console.ReadKey();
        }
    }
}
