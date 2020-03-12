using System;
using System.Diagnostics;
using System.Threading;

namespace Practica9
{
    class Program
    {
        static float[,] a;
        //static float [][]b;
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            int M = 10000;
            int N = 20000;
            a = new float[M, N];

            //Este ciclo es solo para poblar con datos el arreglo
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    a[i, j] = 0.0F + i;
                }
            }

            //Estos son los ciclos que se deben paralelizar
            watch.Start();

            for (int j = 1; j < N; j++)
                for (int i = 0; i < M; i++)
                    a[i, j] = 2 * a[i, j - 1];

            watch.Stop();
            //Bang, termina
            TimeSpan time = watch.Elapsed;
            Console.WriteLine("Elapsed Time: {0}", time);
            Console.WriteLine("Saliendo del programa");
        }
    }
}

//PARALELICE EL CICLO ANTES DE INVERTIRLO
//for (j = 1; j < n; j++)
//fork threads and divide the work among them
//	   for (i = 0; i < m; i++)
//	      a[i][j] = 2 * a[i][j-1];
//join threads

//LUEGO INVIERTA EL CICLO.
//POR �LTIMO, PARALELICE EL CICLO NUEVAMENTE.