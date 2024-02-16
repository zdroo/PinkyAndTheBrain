using System;
using System.Net.NetworkInformation;
using System.Reflection;

namespace Rats
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool isOver = false;
            string mapName = "\\maze2.maze";
            string mapPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + mapName;

            Console.WriteLine("Welcome to Pinky and the Brain!" + "\n" + "To move Pinky press 'W' 'A' 'S' 'D' keys." + "\n" + "To move Brain press 'I' 'J' 'K' 'L' keys.");
            Console.WriteLine("Press 'Enter' key to start the game");
            Console.ReadLine();

            RatRaceGame game = new RatRaceGame(new ConsoleInputRetriever());

            if(game.IsValidMap(mapPath))
            {
                game.LoadMap(mapPath); // load the map from file
                game.PrintMap();
                game.AddRatOnMap(game.Pinky);
                game.AddRatOnMap(game.Brain);
                game.PrintMap();
                while (true)
                {
                    game.MoveRat(game.Pinky);  // move Pinky
                    game.PrintMap();
                    if (game.CountSprouts() == 0 && isOver == false)
                    {
                        WriteGameOver(game);
                        isOver= true;
                    }
                    game.MoveRat(game.Brain);  // move Brain
                    game.PrintMap();
                    if (game.CountSprouts() == 0 && isOver == false)
                    {
                        WriteGameOver(game);
                        isOver = true;
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid map. Load another maze and try again.");
                Console.WriteLine("Press Enter to Exit");
                Console.ReadLine();
            }
        }
        public static void WriteGameOver(RatRaceGame game)
        {
            Console.WriteLine("\n" + "Game over!");
            Console.WriteLine("Pinky's score: " + game.Pinky.Score);
            Console.WriteLine("Brain's score: " + game.Brain.Score);
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
