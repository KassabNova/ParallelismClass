using System;
using System.Diagnostics;
using System.Threading;

namespace Practica7
{
    class Program
    {
        static double[] a;
        static double[] b;
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            /*=====================================
            =          INTIALIZE  EVERYTHING      =
            =====================================*/
            int N = 100000000;

            a = new double[N];
            b = new double[N];


            //Thread t1 = new Thread(() => SeeSharp(0, N / 2));
            //Thread t2 = new Thread(() => SeeSharp(N / 2, N));

            Thread t1 = new Thread(() => SeeSharp(0, N / 4));
            Thread t2 = new Thread(() => SeeSharp(N / 4, N / 2));
            Thread t3 = new Thread(() => SeeSharp(N / 2, N / 4 * 3));
            Thread t4 = new Thread(() => SeeSharp(N / 4 * 3, N));
            for (int i = 0; i < N; i++)
            {
                a[i] = 0.0 + i;
                b[i] = 0.0 + i;
            }

            /*=====================================
            =          RUN AND JOIN 🏃‍💨🔫       =
            =====================================*/
            //Y empieza la carrera
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            //Join Threads
            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();

            for (int i = 1; i < N; i++)
            {
                b[i] = a[i - 1];            //Dependencia por i-1
            }
            watch.Stop();
            //Bang, termina
            TimeSpan time = watch.Elapsed;
            Console.WriteLine("Elapsed Time: {0}", time);
            Console.WriteLine("Saliendo del programa");
        }

        static void SeeSharp(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                if (b[i] > 0.0)                 //Paralelizable
                    a[i] = 2.0 * b[i];          //Paralelizable
                else                            //Paralelizable
                    a[i] = 2.0 * Math.Abs(b[i]);//Paralelizable

            }
        }
    }



}


