using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Practica10
{
    class Program
    {
        static List<int> primes1 = new List<int>();
        static List<int> primes2 = new List<int>();
        static List<int> primes3 = new List<int>();
        static List<int> primes4 = new List<int>();

        static private readonly object primeLock = new object();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Stopwatch watch = new Stopwatch();
            watch.Start();


            /*=====================================
            =          INTIALIZE  EVERYTHING      =
            TODO LO QUE ESTA COMENTADO ABAJO ES EL 
            PRIMER INTENTO DE PARALELIZAR

            EL NUEVO METODO GETPRIMELIST ES EL OPTIMO
            =====================================*/

            int N = 100000000;
            LinkedList<int> asdf = GetPrimeList(N);
            //Thread t1 = new Thread(() => SeeSharp(1, N / 4, primes1));
            //Thread t2 = new Thread(() => SeeSharp(N / 4, N / 2, primes2));
            //Thread t3 = new Thread(() => SeeSharp(N / 2, N / 4 * 3, primes3));
            //Thread t4 = new Thread(() => SeeSharp(N / 4 * 3, N, primes4));

            ///*=====================================
            //=          RUN AND JOIN 🏃‍💨🔫       =
            //=====================================*/

            ////Y empieza la carrera
            //t1.Start();
            //t2.Start();
            //t3.Start();
            //t4.Start();
            ////Join Threads
            //t1.Join();
            //t2.Join();
            //t3.Join();
            //t4.Join();

            watch.Stop();
            //Bang, termina
            TimeSpan time = watch.Elapsed;
            Console.WriteLine("Elapsed Time: {0}", time);
            Console.WriteLine("Saliendo del programa");
            Console.ReadLine();
        }

        static bool checkIsPrime(int numero)
        {
            int i;
            if (numero == 1)
                return false;
            for (i = 2; i < numero; i++)
            {
                if (numero % i == 0)
                    return false;
            }
            return true;
        }
        static void SeeSharp(int start, int end, List<int> primes)
        {
            for (int i = start; i < end; i++)
            {
                if (checkIsPrime(i))
                {
                    primes.Add(i);
                }
            }
        }



        public static LinkedList<int> GetPrimeList(int limit)
        {
            LinkedList<int> primeList = new LinkedList<int>();
            bool[] primeArray = new bool[limit];

            Console.WriteLine("Initialization started...");

            Parallel.For(0, limit, i => {
                if (i % 2 == 0)
                {
                    primeArray[i] = false;
                }
                else
                {
                    primeArray[i] = true;
                }
            }
            );
            Console.WriteLine("Initialization finished...");

            /*for (int i = 0; i < limit; i++) {
                if (i % 2 == 0) {
                    primeArray[i] = false;
                } else {
                    primeArray[i] = true;
                }
            }*/

            int sqrtLimit = (int)Math.Sqrt(limit);
            Console.WriteLine("Operation started...");
            Parallel.For(3, sqrtLimit, i => {
                lock (primeLock)
                {
                    if (primeArray[i])
                    {
                        for (int j = i * i; j < limit; j += i)
                        {
                            primeArray[j] = false;
                        }

                    }
                }
            }
            );
            Console.WriteLine("Operation finished...");
            /*for (int i = 3; i < sqrtLimit; i += 2) {
                if (primeArray[i]) {
                    for (int j = i * i; j < limit; j += i) {
                        primeArray[j] = false;
                    }
                }
            }*/

            primeList.AddLast(2);
            int count = 1;
            Console.WriteLine("Counting started...");
            Parallel.For(3, limit, i => {
                lock (primeLock)
                {
                    if (primeArray[i])
                    {
                        primeList.AddLast(i);
                        count++;
                    }
                }
            }
            );
            Console.WriteLine("Counting finished...");
            Console.WriteLine(count);

            /*for (int i = 3; i < limit; i++) {
                if (primeArray[i]) {
                    primeList.AddLast(i);
                }
            }*/

            return primeList;
        }
    }
}
