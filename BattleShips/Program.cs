using System;

namespace BattleShips
{

    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo input;

            Console.WriteLine(String.Format("Instructions:"));
            Console.WriteLine(String.Format("Inputing coordinates has form “A5”, where “A” is the column and “5” is the row, to specify a square to target."));
            Console.WriteLine(String.Format("O - ship position "));
            Console.WriteLine(String.Format("X - ship hit position "));
            Console.WriteLine(String.Format(". - hit missed "));

            string display = "Welcome to BattleShips, press S to Start new game or Q to quit game.";

            do
            {
                Console.WriteLine();
                Console.WriteLine(String.Format(display));
                Console.WriteLine();

                input = Console.ReadKey();
                Console.WriteLine();
                if (input.Key == ConsoleKey.S)
                {
                    Game game = new Game();
                    game.GameLoop();

                }
                else
                {
                    Console.WriteLine(String.Format("Your input was incorect. Try again."));
                }

                display = "Press S to Start new game or Q to quit game.";

            } while (input.Key != ConsoleKey.Q);

        }
    }
}
