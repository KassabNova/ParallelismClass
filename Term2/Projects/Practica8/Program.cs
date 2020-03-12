using System;
using System.Diagnostics;
using System.Threading;

namespace Practica8
{
    class Program
    {
        static float[] a;
        static float[] b;
        static float x, y;
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();


            /*=====================================
            =          INTIALIZE  EVERYTHING      =
            =====================================*/
            int N = 100000000;
            a = new float[N];
            b = new float[N];
            //Thread t1 = new Thread(() => SeeSharp(0, N / 2));
            //Thread t2 = new Thread(() => SeeSharp(N / 2, N));

            Thread t1 = new Thread(() => SeeSharp(0, N / 4));
            Thread t2 = new Thread(() => SeeSharp(N / 4, N / 2));
            Thread t3 = new Thread(() => SeeSharp(N / 2, N / 4 * 3));
            Thread t4 = new Thread(() => SeeSharp(N / 4 * 3, N));
            for (int i = 0; i < N; i++)
            {
                a[i] = 0.0F + i;
                b[i] = 0.0F + i;
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

            x = a[N - 1] - a[0];
            y = x * b[0] / b[N - 1];
            watch.Stop();

            Console.WriteLine("Saliendo del programa");
        }

        //Las funciones foo y bar no tienen efectos secundarios.
        private static float foo(int i)
        {
            return i * 1.1F;
        }

        private static float bar(float val)
        {
            return val * 2.2F;
        }
        static void SeeSharp(int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                a[i] = foo(i);
                b[i] = bar(a[i]);
            }
        }
    }
}


//fork threads and divide the work among them

//join threads (barrier reached)
//x = a[N-1] � a[0];
//y = x * b[0] / b[N-1];