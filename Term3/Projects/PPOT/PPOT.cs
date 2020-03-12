using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;

namespace PPOT
{
    class PPOT
    {
        static List<Player> players = new List<Player>();
        static short rounds = 1;
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();
        public static Object waitObject = new Object();
        public static Object waitObject2 = new Object();
        static int ready = 0;
        static void Main(string[] args)
        {
            Thread player1 = new Thread(() => PlayerLoop("Player1"));
            Thread player2 = new Thread(() => PlayerLoop("Player2"));
            Thread player3 = new Thread(() => PlayerLoop("Player3"));

            player1.Start();
            player2.Start();
            player3.Start();

            player1.Join();
            player2.Join();
            player3.Join();

        }
        static void PlayerLoop(string name)
        {
            Player player = new Player(name);
            lock (waitObject)
            {
                players.Add(player);
            }

            GameLoop(player);
        }
        static void GameLoop(Player player)
        {

            for (int i = 0; i < rounds; i++)
            {
                lock (waitObject) ready++;
                Choice choice;
                /*=====================================
                =               READY UP             =
                =====================================*/
                if (ready >= 3)
                {
                    Console.WriteLine($"Todos listos ({ready}) para la ronda {i + 1}");
                    ready = 0; //resetting for next round
                    lock (waitObject)
                    {
                        //Console.WriteLine($"{player.name} avisa a los demas");
                        Monitor.PulseAll(waitObject);
                    }
                }
                else
                {
                    lock (waitObject)
                    {
                        Console.WriteLine($"{player.name} esperando a que empieze ronda {i + 1}");
                        Monitor.Wait(waitObject);
                    }
                }

                /*=====================================
                =               CHOOSING             =
                =====================================*/

                foreach (Player playerList in players)
                {
                    if (playerList.name == player.name)
                    {
                        choice = playerList.Choose();
                        Console.WriteLine($"{player.name} choose {Enum.GetName(typeof(Choice), choice)} {ready}");
                        lock (waitObject) ready++;
                        
                    }
                }

                if (ready >= 3)
                {
                    Console.WriteLine($"Todos han escogido");
                    ready = 0; //resetting for next round
                    lock (waitObject)
                    {
                        //Console.WriteLine($"{player.name} avisa que todos escogieron");
                        Monitor.PulseAll(waitObject);
                    }
                }
                else
                {
                    lock (waitObject)
                    {
                        Console.WriteLine($"{player.name} esperando que todos escogan");
                        Monitor.Wait(waitObject);
                    }
                }

                /*=====================================
                =           POINT RESOLUTION         =
                =====================================*/
                lock (waitObject) ready++;

                if (ready >= 3)
                {
                    Console.WriteLine($"Sacando resultados");
                    ready = 0; //resetting for next round
                    foreach(Player player1 in players)
                    {
                        foreach(Player player2 in players)
                        {
                            Player winner = player1.Battle(player2);
                        }
                    }
                    lock (waitObject)
                    {
                        //Console.WriteLine($"{player.name} avisa que todos escogieron");
                        Monitor.PulseAll(waitObject);
                    }
                }
                else
                {
                    lock (waitObject)
                    {
                        Console.WriteLine($"{player.name} esperando resultados");
                        Monitor.Wait(waitObject);
                    }
                }

                //Resolucion de ganadores

            }
        }
        //Un jugador recibirá dos puntos si bate a sus otros dos competidores.
        //Dos jugadores recibirán un punto cada uno si entre ellos baten a un tercero.
        //Cualquier otro caso no reciben puntos.Luego los jugadores juegan otra partida.
 
        class Player
        {
            public string name { get; set; }
            public int victories { get; set; }
            public Choice ppot { get; set; }

            public Player(string name)
            {
                this.name = name;
            }
            public Choice Choose()
            {
                this.ppot = (Choice)Between(1, 3);
                return this.ppot;
            }
            public Player Battle(Player battler)
            {

                return this;
            }
        }=        enum Choice
        {
            Piedra = 1,
            Papel,
            Tijera
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
/*
 
Cada jugador de manera aleatoria escogerá piedra, papel o tijera.
Luego los jugadores compararán sus opciones y verán quien ganó la partida.

Un jugador recibirá dos puntos si bate a sus otros dos competidores.
Dos jugadores recibirán un punto cada uno si entre ellos baten a un tercero.
Cualquier otro caso no reciben puntos. Luego los jugadores juegan otra partida.

Utilice un hilo para simular a cada jugador.
Los jugadores pueden interactuar  directamente con cada uno de ellos.

Su programa deberá preguntar antes de iniciar la partida el número de partidas totales a jugar. 
El programa deberá mostrar una traza de los resultados de cada partida conforme avanza.

Al final del juego su programa deberá imprimir el total de puntos obtenidos por cada jugador. 

     */
