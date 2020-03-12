using System;
using System.Threading;

namespace Counter
{
    class Counter
    {
        public static void Main()
        {
            Console.WriteLine("The Counter has beggun... \n");
            Console.WriteLine("\nINSERTA EL 0(NumPad) CUANDO QUIERAS SALIR");

            Thread t1 = new Thread(() => SeeSharp());
            Thread t2 = new Thread(() => KeyboardHook());
            //Y empieza la carrera
            t1.Start();
            t2.Start();
            //Join Threads
            t1.Join();
            t2.Join();

            Console.WriteLine("\nIt has stopped");
        }

        static void SeeSharp()
        {
            int counter = 0;

            while (true)
            {
                Console.WriteLine("\n"+counter++);
                Thread.Sleep(5000);
            }
        }
        static void KeyboardHook()
        {
            while (true)
            {
                ConsoleKeyInfo Key = Console.ReadKey(false);
                if (Key.Key == ConsoleKey.NumPad0)
                {
                    Environment.Exit(0);
                }
            }
        }

    }
}
