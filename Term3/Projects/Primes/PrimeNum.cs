using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Primes
{
    class PrimeNum
    {
        static private readonly object primeLock = new object();
        static List<List<int>> listaDeListas = new List<List<int>>();
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            TimeSpan time;
            string input;
            int amount;
            Console.WriteLine("\nINSERTA LA CANTIDAD DE NUMEROS");

            input = Console.ReadLine();
            Int32.TryParse(input, out amount);
            Generator(amount);


            int workerThreads;
            int portThreads;

            ThreadPool.GetMaxThreads(out workerThreads, out portThreads);
            Console.WriteLine("\nMaximum worker threads: \t{0}" +
                "\nMaximum completion port threads: {1}",
                workerThreads, portThreads);

            ThreadPool.GetAvailableThreads(out workerThreads,
                out portThreads);
            Console.WriteLine("\nAvailable worker threads: \t{0}" +
                "\nAvailable completion port threads: {1}\n",
                workerThreads, portThreads);


            Thread t1 = new Thread(() => Generator(amount));
            ////////////////////////////////////////////////////////////////////

            Console.WriteLine("\n$$$$$CODIGO SECUENCIAL UN HILO$$$$$");
            //Empieza la carrera
            watch.Restart();
            //LinkedList<int> asdf = GetPrimeList(100);
            watch.Stop();
            //Bang, termina
            time = watch.Elapsed;
            // ShowResults();
            Console.WriteLine("$$$CODIGO SECUENCIAL### Elapsed Time: {0}", time);
        }

        public static void Generator(int amount)
        {

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
