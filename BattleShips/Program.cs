using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShips
{

    internal class Program
    {
        static void Main(string[] args)
        {
            string input = null;

            Console.WriteLine(String.Format("Instructions:"));
            Console.WriteLine(String.Format("O - ship position "));
            Console.WriteLine(String.Format("X - ship hit position "));
            Console.WriteLine(String.Format(". - hit missed "));


            string display = "Welcome to BattleShips, press S to Start new game or Q to quit game.";

            do
            {
                Console.WriteLine();
                Console.WriteLine(String.Format(display));
                Console.WriteLine();
                input = Console.ReadLine();

                if (input.ToUpper() == "S")
                {
                    Game game = new Game();
                    game.GameLoop();
            
                }             
                else
                {
                    Console.WriteLine(String.Format("Your input was incorect. Try again."));
                }

                display = "Press S to Start new game or Q to quit game.";

            } while (input.ToUpper() != "Q");

        }
    }
}
