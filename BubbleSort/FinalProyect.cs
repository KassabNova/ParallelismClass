using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace BubbleSort
{
    class FinalProyect
    {

        private static  int MAXELEMS = 10000000;
        private static int DEFELEMS = 200000;
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
			int sum = 0;
			int temp;
			int i, j;
			for (i = 0; i < elems - 1; i++)
			{

				for (j = i + 1; j < elems; j++)
				{

					if (inicio[i] > inicio[j])
					{
						temp = inicio[i];
						inicio[i] = inicio[j];
						inicio[j] = temp;
					}

				}
			}
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
		static void Merge(int[] input, int left, int middle, int right)
		{
			int[] leftArray = new int[middle - left + 1];
			int[] rightArray = new int[right - middle];

			Array.Copy(input, left, leftArray, 0, middle - left + 1);
			Array.Copy(input, middle + 1, rightArray, 0, right - middle);

			int i = 0;
			int j = 0;
			for (int k = left; k < right + 1; k++)
			{
				if (i == leftArray.Length)
				{
					input[k] = rightArray[j];
					j++;
				}
				else if (j == rightArray.Length)
				{
					input[k] = leftArray[i];
					i++;
				}
				else if (leftArray[i] <= rightArray[j])
				{
					input[k] = leftArray[i];
					i++;
				}
				else
				{
					input[k] = rightArray[j];
					j++;
				}
			}
		}
		static void MergeSort(int[] input, int left, int right)
		{
			if (left < right)
			{
				int middle = (left + right) / 2;

				MergeSort(input, left, middle);
				MergeSort(input, middle + 1, right);

				Merge(input, left, middle, right);
			}
		}
		//Funcion para sacar números random más random
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
