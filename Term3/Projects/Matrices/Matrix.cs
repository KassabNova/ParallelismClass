using System;
using System.Diagnostics;
using System.Threading;

namespace Matrices
{

    class Matrix
    {
        static int arraySize = 0;
        static int[,] multiply;
        static int[,] first;
        static int[,] second;
        static int m, n, p, q;
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            TimeSpan time;
            string input;

            Console.WriteLine("\nINSERTA EL TAMAÑO DE LA MATRIZ CUADRADA");

            input = Console.ReadLine();
            Int32.TryParse(input, out arraySize);
            InitMatrix();

            ////////////////////////////////////////////////////////////////////

            Console.WriteLine("\n$$$$$CODIGO SECUENCIAL UN HILO$$$$$");
            //Empieza la carrera
            watch.Restart();
            Secuencial();
            watch.Stop();
            //Bang, termina
            time = watch.Elapsed;
            //ShowResults();
            Console.WriteLine("$$$CODIGO SECUENCIAL### Elapsed Time: {0}", time);

            ////////////////////////////////////////////////////////////////////

            Console.WriteLine("\n@@@@@CODIGO PARALELIZADO CON DOS HILOS@@@@@");
            //Empieza la carrera
            watch.Restart();
            DosHilos();
            watch.Stop();
            //Bang, termina
            time = watch.Elapsed;
            //ShowResults();
            Console.WriteLine("@@@CODIGO DOS HILOS### Elapsed Time: {0}", time);

            ////////////////////////////////////////////////////////////////////

            Console.WriteLine("\n%%%%%CODIGO PARALELIZADO CON CUATRO HILOS%%%%%");
            //Empieza la carrera
            watch.Restart();
            CuatroHilos();
            watch.Stop();
            //Bang, termina
            time = watch.Elapsed;
            //ShowResults();
            Console.WriteLine("%%%CODIGO CUATRO HILOS### Elapsed Time: {0}", time);

            ////////////////////////////////////////////////////////////////////

            Console.ReadLine();
        }
        public static void CodigoBase()
        {
            Console.WriteLine("Hello World!");

            int m, n, p, q, sum = 0, c, d, k, f;
            string input;
            /*=====================================
            =          INTIALIZE  EVERYTHING      =
            =====================================*/
            Console.WriteLine("Enter the number of rows and columns of first matrix");
            input = Console.ReadLine();
            Int32.TryParse(input, out m);
            input = Console.ReadLine();
            Int32.TryParse(input, out n);

            int[,] first = new int[m, n];

            Console.WriteLine("Enter the elements of first matrix");

            for (c = 0; c < m; c++)
            {
                for (d = 0; d < n; d++)
                {
                    input = Console.ReadLine();
                    Int32.TryParse(input, out f);
                    first[c, d] = f;
                }
            }


            Console.WriteLine("Enter the number of rows and columns of second matrix");
            input = Console.ReadLine();
            Int32.TryParse(input, out p);
            input = Console.ReadLine();
            Int32.TryParse(input, out q);
            if (n != p)
            {
                Console.WriteLine("Matrices with entered orders can't be multiplied with each other.");
            }
            /*=====================================
            =             MULTIPLICATION          =
            =====================================*/
            else
            {
                int[,] second = new int[p, q];
                int[,] multiply = new int[m, q];

                Console.WriteLine("Enter the elements of second matrix");

                for (c = 0; c < p; c++)
                {
                    for (d = 0; d < q; d++)
                    {
                        input = Console.ReadLine();
                        Int32.TryParse(input, out f);
                        second[c, d] = f;
                    }
                }


                for (c = 0; c < m; c++)
                {
                    for (d = 0; d < q; d++)
                    {
                        for (k = 0; k < p; k++)
                        {
                            sum = sum + first[c, k] * second[k, d];
                        }

                        multiply[c, d] = sum;
                        sum = 0;
                    }
                }

                Console.WriteLine("Product of entered matrices:-");

                for (c = 0; c < m; c++)
                {
                    for (d = 0; d < q; d++)
                    {
                        Console.Write(multiply[c, d] + " ");
                    }
                    Console.WriteLine("\t");
                }
            }
        }
        public static void Secuencial()
        {
            int sum = 0, c, d, k;
            for (c = 0; c < m; c++)
            {
                for (d = 0; d < q; d++)
                {
                    for (k = 0; k < p; k++)
                    {
                        sum = sum + first[c, k] * second[k, d];
                    }

                    multiply[c, d] = sum;
                    sum = 0;
                }
            }
        }
        public static void DosHilos()
        {
            int interval = arraySize / 2;
            Thread t1 = new Thread(() => SeeSharp(0, interval));
            Thread t2 = new Thread(() => SeeSharp(interval, arraySize));
            //Y empieza la carrera
            t1.Start();
            t2.Start();
            //Join Threads
            t1.Join();
            t2.Join();

        }
        public static void CuatroHilos()
        {
            int interval = arraySize / 4;
            Thread t1 = new Thread(() => SeeSharp(0, interval));
            Thread t2 = new Thread(() => SeeSharp(interval, interval * 3));
            Thread t3 = new Thread(() => SeeSharp(interval * 3, interval * 2));
            Thread t4 = new Thread(() => SeeSharp(interval * 2, arraySize));
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

        }
        /*=====================================
        =             MULTIPLICATION          =
        =====================================*/
        static void SeeSharp(int start, int end)
        {
            int sum = 0;
            for (int c = start; c < end; c++)
            {
                for (int d = start; d < end; d++)
                {
                    for (int k = 0; k < p; k++)
                    {
                        sum += first[c, k] * second[k, d];
                    }

                    multiply[c, d] = sum;
                    sum = 0;
                }
            }
        }
        /*=====================================
        =          INTIALIZE  EVERYTHING      =
        =====================================*/
        public static void InitMatrix()
        {
            Random rnd = new Random();
            m = arraySize;
            n = arraySize;
            p = arraySize;
            q = arraySize;
            first = new int[m, n];
            second = new int[p, q];
            multiply = new int[m, q];

            for (int c = 0; c < m; c++)
            {
                for (int d = 0; d < n; d++)
                {
                    first[c, d] = rnd.Next(1, 100); ;
                }
            }
            for (int c = 0; c < p; c++)
            {
                for (int d = 0; d < q; d++)
                {
                    second[c, d] = rnd.Next(1, 100); ;
                }
            }
        }
        /*=====================================
        =             SHOWING RESULTS         =
        =====================================*/
        public static void ShowResults()
        {
            Console.WriteLine("Product of entered matrices:-");
            for (int c = 0; c < m; c++)
            {
                for (int d = 0; d < q; d++)
                {
                    Console.Write(multiply[c, d] + " ");
                }
                Console.WriteLine("\t");
            }
        }

    }
}
/*
1) Analice y ejecute el código. #######

2) Adapte el código para generar dos matrices de 1000 * 1000 elementos, con números enteros aleatorios entre 1 y 100. #####

3) Mida el tiempo en el que se ejecuta la multiplicación de matrices. ######

4) Cambie el código a una versión paralela que se ejecute en 2 hilos.  #####

5) Revise la aceleración de la versión de un hilo y la de dos hilos. #####

6) Cambie el código nuevamente a una versión paralelizada que ejecute la multiplicación en 4 hilos. #####

7) Documente la aceleración de la versión de un hilo y la versión de 4 hilos. #####

8) Suba el los archivos .java con sus conclusiones. #####
     */




