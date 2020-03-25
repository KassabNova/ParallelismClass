﻿using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace BubbleSort
{
    class FinalProyect
    {

        private static  int MAXELEMS = 10000000;
        private static int DEFELEMS =  178000;
        private static int MINELEMS = 0;
		private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();
		private const int maxNumber = 100;
		private const int minNumber = 1;
		static int[] elem;
        static int elems = DEFELEMS;
		public static Thread[] threads;
        static void Main(string[] args)
        {
			Stopwatch watch = new Stopwatch();
			TimeSpan time;
			int i;
			int errores = 0;

			if (args.Length == 0)
			{
				elem = new int[DEFELEMS];

				elems = DEFELEMS;
			}
			else if (args.Length == 1)
			{
				elem = new int[MAXELEMS];
				elems = Int32.Parse(args[0]);
				if (elems < MINELEMS || elems > MAXELEMS)
				{
					Console.WriteLine($"Numero invalido, el minimo es {MINELEMS} y el maximo es {MAXELEMS}");
					Environment.Exit(1);
				}
			}
			else
			{
				Console.WriteLine("Numero invalido de argumentos");
				Environment.Exit(1);
			}

			Console.WriteLine($"Ordenando {elems} elementos");

			// Inicializa el arreglo con numeros random
			for (i = 0; i < elems; i++)
				elem[i] = Between(minNumber, maxNumber);

			//Empieza la carrera
			watch.Restart();
			sort(elem, elems);
			watch.Stop();
			//Bang, termina
			time = watch.Elapsed;

			Console.WriteLine("Termina sort \n");

			//Revisa errores
			for (i = 0; i < elems - 1; i++)
			{
				if (elem[i] > elem[i + 1])
				{
					errores++;
					Console.WriteLine($"elem[{i}] = {elem[i]} <--> elem[{i + 1}] = {elem[i + 1]}");
				}
			}

			//ShowResults();
			Console.WriteLine($"###Errores {errores}");
			Console.WriteLine($"###Elapsed Time: {time}");
		}

		public static void sort(int[] inicio, int elems)
		{
			int i, j;
			int temp;

			//Cuarto nivel
			Thread t1 = new Thread(() => BubbleSort(0, elems / 8, inicio));
			Thread t2 = new Thread(() => BubbleSort(elems / 8, elems / 8 * 2, inicio));
			Thread t3 = new Thread(() => BubbleSort(elems / 8 * 2, elems / 8 * 3, inicio));
			Thread t4 = new Thread(() => BubbleSort(elems / 8 * 3, elems/2, inicio));
			Thread t5 = new Thread(() => BubbleSort(elems / 2, elems / 8 * 5, inicio));
			Thread t6 = new Thread(() => BubbleSort(elems / 8 * 5, elems / 8 * 6, inicio));
			Thread t7 = new Thread(() => BubbleSort(elems / 8 * 6, elems / 8 * 7, inicio));
			Thread t8 = new Thread(() => BubbleSort(elems / 8 * 7, elems, inicio));
			//Tercer nivel
			Thread t09 = new Thread(() => BubbleSort(0, elems / 4, inicio));
			Thread t10 = new Thread(() => BubbleSort(elems / 4, elems / 2, inicio));
			Thread t11 = new Thread(() => BubbleSort(elems / 2, elems / 4 * 3, inicio));
			Thread t12 = new Thread(() => BubbleSort(elems / 4 * 3, elems, inicio));
			//Segundo nivel							  
			Thread t13 = new Thread(() => BubbleSort(0, elems / 2, inicio));
			Thread t14 = new Thread(() => BubbleSort(elems / 2, elems, inicio));
			//Primer nivel							  
			Thread t15 = new Thread(() => BubbleSort(0, elems, inicio));

			//Cuarto nivel
			t1.Start();
			t2.Start();
			t3.Start();
			t4.Start();
			t5.Start();
			t6.Start();
			t7.Start();
			t8.Start();

			t1.Join();
			t2.Join();
			t3.Join();
			t4.Join();
			t5.Join();
			t6.Join();
			t7.Join();
			t8.Join();

			//Tercer nivel
			t09.Start();
			t10.Start();
			t11.Start();
			t12.Start();

			t09.Join();
			t10.Join();
			t11.Join();
			t12.Join();

			//Segundo nivel
			t13.Start();
			t14.Start();

			t13.Join();
			t14.Join();



			//Primer nivel
			t15.Start();
			t15.Join();

			//for (i = 0; i < elems - 1; i++)
			//{
			//	for (j = i + 1; j < elems; j++)
			//	{

			//		if (inicio[i] > inicio[j])
			//		{
			//			temp = inicio[i];
			//			inicio[i] = inicio[j];
			//			inicio[j] = temp;
			//		}

			//	}
			//}


			return;
		}
		static void BubbleSort(int start, int end, int[] inicio)
		{
			int sum = 0;
			int temp;
			int i, j;
			for (i = start; i < end - 1; i++)
			{

				for (j = i + 1; j < end; j++)
				{

					if (inicio[i] > inicio[j])
					{
						temp = inicio[i];
						inicio[i] = inicio[j];
						inicio[j] = temp;
					}

				}
			}
		}
		static void SeeSharp(int start, int end, int[] inicio)
		{
			int sum = 0;
			int temp;
			int i, j;
			for (i = start; i < end - 1; i++)
			{

				for (j = i + 1; j < end; j++)
				{

					if (inicio[i] > inicio[j])
					{
						temp = inicio[i];
						inicio[i] = inicio[j];
						inicio[j] = temp;
					}

				}
			}
		}

		
		public static int Between(int minimumValue, int maximumValue)
		{
			byte[] randomNumber = new byte[1];

			_generator.GetBytes(randomNumber);

			double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

			// We are using Math.Max, and substracting 0.00000000001, 
			// to ensure "multiplier" will always be between 0.0 and .99999999999
			// Otherwise, it's possible for it to be "1", which causes problems in our rounding.
			double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

			// We need to add one to the range, to allow for the rounding done with Math.Floor
			int range = maximumValue - minimumValue + 1;

			double randomValueInRange = Math.Floor(multiplier * range);

			return (int)(minimumValue + randomValueInRange);
		}
	}
}
