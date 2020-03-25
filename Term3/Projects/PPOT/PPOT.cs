using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;

namespace PPOT
{
    class PPOT
    {
        static List<Player> players = new List<Player>();
        static byte rounds = 30;
        private static readonly RNGCryptoServiceProvider _generator = new RNGCryptoServiceProvider();
        public static Object waitObject = new Object();
        public static Object waitObject2 = new Object();
        static byte ready = 0;
        static void Main(string[] args)
        {
            Thread player1 = new Thread(() => PlayerLoop("Player1"));
            Thread player2 = new Thread(() => PlayerLoop("Player2"));
            Thread player3 = new Thread(() => PlayerLoop("Player3"));
            string input;
            Console.Write("Numero de rondas:");
            input = Console.ReadLine();
            byte.TryParse(input, out rounds);
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
                players.Sort((player1,player2) => player1.name.CompareTo(player2.name));
            }

            GameLoop(player);
        }
        static void GameLoop(Player player)
        {
            for (byte i = 0; i < rounds; i++)
            {
                lock (waitObject) ready++;
                Choice choice;
                /*=====================================
                =               READY UP             =
                =====================================*/
                if (ready >= 3)
                {
                    Console.WriteLine($"\n\nTodos listos ({ready}) para la ronda {i + 1}");
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
                        //Console.WriteLine($"{player.name} esperando a que empieze ronda {i + 1}");
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
                        player.victories = 0; //resetting battle round victories
                        choice = playerList.Choose();
                        Console.WriteLine($"{player.name} choose {Enum.GetName(typeof(Choice), choice)}");
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
                            if(player1.name != player2.name)
                            {
                                Player winner = player1.Battle(player2);
                            }
                        }
                    }

                    //Finally
                    AssignPoints();//Give points based on requirements
                    //And Then Continue
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
            }
        }
        //Un jugador recibirá dos puntos si bate a sus otros dos competidores.
        //Dos jugadores recibirán un punto cada uno si entre ellos baten a un tercero.
        //Cualquier otro caso no reciben puntos.Luego los jugadores juegan otra partida.
        public static byte AssignPoints()
        {
            //ToDo Change this so it can be dynamic
            //Resolucion de ganadores
            if (players[0].victories == 1 && players[1].victories == 1 && players[2].victories == 1)
            {
                Console.WriteLine("Todos ganaron una ronda, asi que 0 puntos!");
                PrintScores();
                return 0;
            }
            if (players[0].victories == 0 && players[1].victories == 0 && players[2].victories == 0)
            {
                Console.WriteLine("Todos perdieron todas las rondas, asi que 0 puntos!");
                PrintScores();
                return 0;
            }
            foreach(Player player in players)
            {
                player.points += player.victories;
                player.victories = 0; //resetting battle round victories
            }
            PrintScores();
            return 0;

        }
        public static void PrintScores()
        {
            foreach (Player player in players)
            {
                Console.WriteLine($"Score del {player.name}: {player.points}");
            }
        }
        class Player
        {
            public string name { get; set; }
            public byte victories { get; set; }
            public byte points { get; set; }
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
                if(this.ppot == Choice.Piedra && battler.ppot == Choice.Papel)
                {
                    battler.victories++;
                    return battler;
                }
                if (this.ppot == Choice.Papel && battler.ppot == Choice.Tijera)
                {
                    battler.victories++;
                    return battler;
                }
                if (this.ppot == Choice.Tijera && battler.ppot == Choice.Piedra)
                {
                    battler.victories++;
                    return battler;
                }
                if (this.ppot == battler.ppot )
                {
                    return null;
                }
                return this;
            }
        }
        enum Choice
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

