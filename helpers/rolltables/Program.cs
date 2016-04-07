using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RollTables
{
    class Program
    {
        private static Random rndm = new Random();

        /// <summary>
        /// Roll die for a value.
        /// </summary>
        /// <param name="n">Number of Dice</param>
        /// <param name="x">Size of Dice</param>
        /// <returns></returns>
        private static int Roll(int n, int x)
        {
            int tot = 0;

            for (int i = 0; i < n; i++)
            {
                tot += 1 + rndm.Next(x);
            }

            return tot;
        }

        class DataPoint
        {
            public int NeededToHit
            {
                get; set;
            }
            public int Modifier
            {
                get; set;
            }
            public double PercentChance
            {
                get;
                set;
            }
        }

        static void Main(string[] args)
        {
            List<DataPoint> points_of_data = new List<DataPoint>();

            double simul_time = 50000;

            //Generate Table
            for (int needed_to_hit = 10; needed_to_hit < 31; needed_to_hit++)
            {
                for (int modifier = -5; modifier < 11; modifier++)
                {
                    DataPoint data = new DataPoint()
                    {
                        NeededToHit = needed_to_hit,
                        Modifier = modifier
                    };

                    int count = 0;
                    for (int i = 0; i < simul_time; i++)
                    {
                        int roll = Roll(1, 20);
                        if ((roll + modifier >= needed_to_hit) || (roll == 20))
                        {
                            count++;
                        }
                    }

                    data.PercentChance = count / simul_time;

                    System.Console.WriteLine("Done: 1d20+(" + modifier.ToString() + ") against " + needed_to_hit.ToString() + " to hit.");

                    points_of_data.Add(data);
                }
            }

            double[,] dataplot = new double[21, 16];

            using (var sr = new StreamWriter("Results.csv"))
            {
                sr.WriteLine(", 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30");
                foreach (var item in points_of_data)
                {
                    dataplot[item.NeededToHit - 10, item.Modifier + 5] = item.PercentChance;
                }

                for (int i = 0; i < 16; i++)
                {
                    string output = (i - 5).ToString();

                    for (int j = 0; j < 21; j++)
                    {
                        output += "," + dataplot[j, i].ToString();
                    }

                    sr.WriteLine(output);
                }
            }
        }
    }
}
