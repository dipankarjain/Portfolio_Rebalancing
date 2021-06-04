using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;

namespace PortfolioRebalancingManual
    {

        class Stock
        {
            public int _min, _max;
            public static int inv_amt;
            public float[] _returns, _excess_returns;
            public float _avg_return;
            public double _std_dev, _price;


            // A constructor
            public Stock(int min, int max, double price, float[] returns)
            {
                this._min = min;
                this._max = max;
                this._price = price;
                this._returns = returns;
                this._avg_return = this._returns.Average();
                    //avg();
                this._excess_returns = this._returns.Select(i => i - _avg_return).ToArray();
                this._std_dev = StandardDeviation(this._returns);
            }

            public static double StandardDeviation(float[] values)
            {
                double avg = values.Average();
                return Math.Sqrt(values.Sum(v => (v - avg) * (v - avg))/(values.Count()-1));
            }

        }

    class Program
    {
        static void Main()
        {
            var watch = new Stopwatch();
            watch.Start();
            float[][] returns = new float[][]
            {
                    new float [] {0.0253F, -0.0014F,   0.1758F,    0.0192F,    0.0954F,    -0.0313F,   0.0698F,    -0.0145F,   0.1109F,    0.256F  },
                    new float [] {0.5691F,  0.2963F,    -0.0018F,   0.0944F,    0.4936F,    -0.1855F,   0.2208F,    0.3219F,    -0.006F,    0.6204F },
                    new float [] {-0.0161F, 0.0629F,    -0.073F,    0.321F, 0.0655F,    0.3101F,    0.16F,  0.385F, 0.0945F,    0.4886F },
                    new float [] {0.1433F, -0.1863F,   0.2322F,    0.1626F,    -0.0179F,   0.09F,  0.3523F,    0.3125F,    -0.1112F,   0.246F  },
                    new float [] {0.1968F,  0.0193F,    0.1341F,    0.1845F,    0.1035F,    -0.0459F,   0.1762F,    0.2066F,    -0.0411F,   0.1662F },
                    new float [] {0.0253F, -0.0014F,   0.1758F,    0.0192F,    0.0954F,    -0.0313F,   0.0698F,    -0.0145F,   0.1109F,    0.256F  },
                    new float [] {0.5691F,  0.2963F,    -0.0018F,   0.0944F,    0.4936F,    -0.1855F,   0.2208F,    0.3219F,    -0.006F,    0.6204F },
                    new float [] {-0.0161F, 0.0629F,    -0.073F,    0.321F, 0.0655F,    0.3101F,    0.16F,  0.385F, 0.0945F,    0.4886F },
                    new float [] {0.1433F, -0.1863F,   0.2322F,    0.1626F,    -0.0179F,   0.09F,  0.3523F,    0.3125F,    -0.1112F,   0.246F  },
                    new float [] {0.1968F,  0.0193F,    0.1341F,    0.1845F,    0.1035F,    -0.0459F,   0.1762F,    0.2066F,    -0.0411F,   0.1662F }
            };
            Stock.inv_amt = Convert.ToInt32(Console.ReadLine());
            int NStocks = Convert.ToInt32(Console.ReadLine());

            Stock[] S = new Stock[NStocks];

            //List<int[]> arrayList = new List<int[]>();
            int[][] jaggedArray = new int[NStocks][];


            int len = 1;

            for (int i = 0; i < NStocks; i++)
            {
                var line = Console.ReadLine().Split();
                // Instantiate a new object, set it's number and
                // some other properties
                int min = int.Parse(line[0]);
                int max = int.Parse(line[1]);
                double price = double.Parse(line[2]);
                S[i] = new Stock(min, max, price, returns[i]);
                int u_min = (int)Math.Ceiling(min * Stock.inv_amt / price / 100);
                int u_max = (int)Math.Floor(max * Stock.inv_amt / price / 100);
                jaggedArray[i] = MyEnumerable.AlternateRange(u_min, u_max - u_min + 1, 5).ToArray();
                len *= (u_max - u_min + 1)/5;
            }

            double[,] CoVar = new double[NStocks, NStocks];
            double[,] CoRel = new double[NStocks, NStocks];

            /*
                Finding the covarianc eand correlation between all the stock pairs
            */
            for (int p = 0; p < NStocks; p++)
            {
                for (int q = 0; q < NStocks; q++)
                {
                    for (int r = 0; r <= 9; r++)
                    {
                        CoVar[p, q] += S[p]._excess_returns[r] * S[q]._excess_returns[r];
                    }
                    CoVar[p, q] = CoVar[p, q] / 9;
                    CoRel[p, q] = CoVar[p, q] / (S[p]._std_dev * S[q]._std_dev);
                }
            }

            IEnumerable<int[]> final = CartesianProduct<int>(jaggedArray);
            int[][] asArray = final.ToArray();

            double[] Value = new double[len];
            double[] wt = new double[NStocks];
            double[] stdwt = new double[NStocks];
            double M1 = 0, M2 = 0;
            double[,] R_R = new double[len, 3];

            for (int i = 0; i < len; i++)
            {
                for (int j = 0; j < NStocks; j++)
                {
                    //Value[i, j]= asArray[i][j];
                    Value[i] += S[j]._price * asArray[i][j];
                    wt[j] = S[j]._price * asArray[i][j] / Value[i];
                    stdwt[j] = wt[j] * S[j]._std_dev;
                }
                M2 = 0;
                for (int p = 0; p < NStocks; p++)
                {
                    M1 = 0;
                    for (int q = 0; q < NStocks; q++)
                    {
                        M1 += stdwt[q] * CoRel[q, p];

                    }
                    M2 += M1 * stdwt[p];
                    R_R[i, 0] += S[p]._avg_return * wt[p];
                }
                R_R[i, 1] = Math.Sqrt(M2);
                R_R[i, 2] = R_R[i, 1] / R_R[i, 0];
            }
            
            SaveArrayAsCSV(R_R, "efficientfrontier.csv");               // Save the risk and return in excel
            watch.Stop();
            Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");

            static IEnumerable<T[]> CartesianProduct<T>(T[][] arrays)
            {
                int[] lengths;
                lengths = arrays.Select(a => a.Length).ToArray();
                //puts lengths in arrays eg 10,20
                int Len = arrays.Length;
                // Total 10+20=30
                int[] inds = new int[Len];
                // array to 30

                int Len1 = Len - 1;
                //Len1 = 29

                while (inds[0] != lengths[0])
                {
                    T[] res = new T[Len];
                    for (int i = 0; i != Len; i++)
                    {
                        res[i] = arrays[i][inds[i]];
                    }
                    yield return res;
                    int j = Len1;
                    inds[j]++;
                    while (j > 0 && inds[j] == lengths[j])
                    {
                        inds[j--] = 0;
                        inds[j]++;
                    }
                }
            }
        }

        public static void SaveArrayAsCSV(double[,] arrayToSave, string fileName)
        {
            using (StreamWriter file = new StreamWriter(fileName))
            {
                WriteItemsToFile(arrayToSave, file);
            }
        }

        private static void WriteItemsToFile(double[,] items, TextWriter file)
        {
            file.Write($"Return,Risk\n");
            for (int i = 0; i < items.GetUpperBound(0); i++)
            {
                file.Write($"{items[i, 0]},{items[i, 1]}\n");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static class MyEnumerable {
            /// <summary>
            /// Get a range with the defined number of steps in between
            /// </summary>
            /// <param name="start"></param>
            /// <param name="count"></param>
            /// <param name="steps"></param>
            /// <returns></returns>
            public static IEnumerable<int> AlternateRange(int start, int count, int steps) {
                for (int i = start; i < start + count; i += steps) {
                    yield return i;
                }
            }
        }
    }
}
    


