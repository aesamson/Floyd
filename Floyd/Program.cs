#define PARALLEL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Floyd
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var path = GetPathMattr();

            demo();
        }

        static int[,] GetPathMattr()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            const int limit = 100000, dim = 1000;
            var length = new int[dim, dim];
            var path = new int[dim, dim];

            const double distrib = 0.8;
            for (var i = 0; i < dim; i++)
                for (var j = 0; j < dim; j++)
                    length[i, j] = rnd.NextDouble() < distrib ? 0 : rnd.Next(limit) + 1;

            for (var i = 0; i < dim; i++)
                for (var j = 0; j < dim; j++)
                    if (length[i, j] != limit) path[i, j] = j + 1;

            var time = new Stopwatch();
            time.Start();
#if PARALLEL
            for (var k = 0; k < dim; k++)
                Parallel.For(0, dim, i =>
                                         {
                                             for (var j = 0; j < dim; j++)
                                                 if (i != j && i != k && k != j &&
                                                     length[i, k] != limit && length[k, j] != limit &&
                                                     length[i, k] + length[k, j] < length[i, j])
                                                 {
                                                     length[i, j] = length[i, k] + length[k, j];
                                                     path[i, j] = path[i, k];
                                                 }
                                         });
# else
            for (var k = 0; k < dim; k++)
                for (var i = 0; i < dim; i++)
                    for (var j = 0; j < dim; j++)
                        if (i != j && i != k && k != j && length[i, k] != limit && length[k, j] != limit &&
                            length[i, k] + length[k, j] < length[i, j])
                        {
                            length[i, j] = length[i, k] + length[k, j];
                            path[i, j] = path[i, k];
                        }
#endif
            time.Stop();
            Console.WriteLine(time.ElapsedMilliseconds);

            //GC.Collect();
            //Console.WriteLine(GC.GetTotalMemory(true));

            return path;
        }

        static void demo()
        {
            const int u = 1000, d = 6;
            var length = new int[d, d]
                             {
                                 {0, 2, u, 5, u, u},
                                 {u, 0, 4, u, u, u},
                                 {u, u, 0, 1, u, 7},
                                 {-3, 4, 6, 0, 5, u},
                                 {u, u, 8, u, 0, -1},
                                 {u, u, u, u, u, 0}
                             };
            var path = new int[d, d];

            for (var i = 0; i < d; i++)
                for (var j = 0; j < d; j++)
                    if (length[i, j] != u) path[i, j] = j + 1;

            for (var k = 0; k < d; k++)
                for (var i = 0; i < d; i++)
                    for (var j = 0; j < d; j++)
                        if (i != j && i != k && k != j && length[i, k] != u && length[k, j] != u &&
                            length[i, k] + length[k, j] < length[i, j])
                        {
                            length[i, j] = length[i, k] + length[k, j];
                            path[i, j] = path[i, k];
                        }



            Console.WriteLine();
            Console.WriteLine("матрица длин путей");
            Console.WriteLine("   1 2 3 4 5 6");
            Console.WriteLine(" _____________");

            for (var i = 0; i < d; i++)
            {
                Console.Write((i + 1) + "| ");
                for (var j = 0; j < d; j++)
                    Console.Write("{0} ", length[i, j]);
                Console.WriteLine("\n |");
            }

            Console.WriteLine();
            Console.WriteLine("матрица путей");
            Console.WriteLine("   1 2 3 4 5 6");
            Console.WriteLine(" _____________");

            for (var i = 0; i < d; i++)
            {
                Console.Write((i + 1) + "| ");
                for (var j = 0; j < d; j++)
                    Console.Write("{0} ", path[i, j]);
                Console.WriteLine("\n |");
            }
        }
    }
}
