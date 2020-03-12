using System;
using System.Diagnostics;
using System.Threading;

namespace ParallelTasks
{
    class ParallelTasks
    {
        static readonly object pblock = new object();
        public static double sum = 0;
        static void Main(string[] args)
        {
            /*
             *  Para la version de dos hilos se debe comentar t1-t4 y descomentar
             *  los primeros t1-t2.
             *  
             *  De igual manera los start y join
             */
            int num_steps = 21000000;
            //int num_steps= 1000000000;
            double step;
            double pi;

            Stopwatch watch = new Stopwatch();

            step = 1.0 / (double)num_steps;

            Thread t1 = new Thread(() => SeeSharp(0, num_steps / 2, step));
            Thread t2 = new Thread(() => SeeSharp(num_steps / 2, num_steps, step));

            //Thread t1 = new Thread(() => SeeSharp(0, num_steps / 4, step));
            //Thread t2 = new Thread(() => SeeSharp(num_steps / 4, num_steps / 2, step));
            //Thread t3 = new Thread(() => SeeSharp(num_steps / 2, num_steps / 4 * 3, step));
            //Thread t4 = new Thread(() => SeeSharp(num_steps / 4 * 3, num_steps, step));

            //Y empieza la carrera
            watch.Start();

            t1.Start();
            t2.Start();
           // t3.Start();
           // t4.Start();

            t1.Join();
            t2.Join();
            //t3.Join();
            //t4.Join();
            pi = sum * step; // 3.1415926535897932384626433832

            watch.Stop();
            //Bang, termina

            TimeSpan time = watch.Elapsed;

            Console.WriteLine("Circle thingy: {0}", pi);
            Console.WriteLine("Elapsed Time: {0}", time);

            Console.ReadLine();
        }
        static void SeeSharp(int start, int end, double stp)
        {
            double x;
            for (int i = start; i < end; i++)
            {
                x = (i + 0.5) * stp;
                //Sección crítica
                lock(pblock)
                    sum +=  4.0 / (1.0 + x * x);
            }
        }
    }

}
