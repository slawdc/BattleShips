using System;

namespace BattleShips
{
    internal class Game
    {
        private ComputerPlayer _cpuPlayer;
        private HumanPlayer _humanPlayer;

        private int _playerShoot;
        private int _cpuShoot;


        public Game()
        {
            _humanPlayer = new HumanPlayer();
            _humanPlayer.SetShips();

            _cpuPlayer = new ComputerPlayer(_humanPlayer.GetReferencesToMyShips(), _humanPlayer.GetReferencesToMyGameBoard());

            _humanPlayer.SetReferencesToCpuShips(_cpuPlayer.GetReferencesToMyShips());

            _humanPlayer.SetReferencesToCpuBoard(_cpuPlayer.GetReferencesToMyGameBoard());

        }

        public void GameLoop()
        {
           
            bool cpuWiner = false;
            bool gameOver = false;


            do
            {
                gameOver = PlayerShoot();

                if (!gameOver)
                {
                    gameOver = CpuShoot();

                    if (gameOver)
                    {
                        cpuWiner = true;
                    }

                }


            } while (!gameOver);

            DisplayWinner(cpuWiner);

        }

        private bool PlayerShoot()
        {
            bool gameOver = false;

            Console.WriteLine("Cpu gameboard.");
            DisplayGameBoard(_cpuPlayer.GetReferencesToMyGameBoard().GetArrayBoard(), false);

            do
            {
                _playerShoot = _humanPlayer.Shoot();

                if (_playerShoot == 0)
                {
                    Console.WriteLine("You missed.");

                }
                else if (_playerShoot == 1)
                {
                    Console.WriteLine("You hit ship.");

                }
                else if (_playerShoot == 2)
                {

                    Console.WriteLine("You sunk cpu's ship.");
                }
                if (_humanPlayer.CheckIFGameOver())
                {
                    gameOver = true;
                    
                }

              

            } while (_playerShoot > 0 ^ gameOver);

            return gameOver;

        }

        private bool CpuShoot()
        {
            bool gameOver = false;

            //shoot till cpu hit target
            do
            {
                Console.WriteLine("Cpu shoot.");

                _cpuShoot = _cpuPlayer.Shoot();

                if (_cpuShoot == 0)
                {
                    Console.WriteLine("Cpu missed.");
                }
                else if (_cpuShoot == 1)
                {
                    Console.WriteLine("Cpu hit your ship.");

                }
                else if (_cpuShoot == 2)
                {
                    Console.WriteLine("Cpu sunk your ship.");
                }

                if (_cpuPlayer.CheckIFGameOver())
                {
                    gameOver = true;
                 
                }

                Console.WriteLine("Your gameboard.");

                DisplayGameBoard(_humanPlayer.GetReferencesToMyGameBoard().GetArrayBoard(), true);

            } while (_cpuShoot > 0 ^ gameOver);

            return gameOver;
        }

        public void DisplayGameBoard(int[,] arrayboard, bool displayShips)
        {
            Console.WriteLine();
            Console.WriteLine("   A B C D E F G H I J");

            for (int row = 0; row < arrayboard.GetLength(0); row++)
            {

                if (row == 9)
                {
                    Console.Write(row + 1 + " ");
                }
                else
                {
                    Console.Write(row + 1 + "  ");
                }

                for (int column = 0; column < arrayboard.GetLength(1); column++)
                {
                    if (arrayboard[row, column] == 0)
                    {
                        Console.Write("  ");

                    }
                    else if (arrayboard[row, column] == 1 )
                    {
                        if (displayShips)
                        {
                            Console.Write("O ");
                        }
                        else
                        {
                            Console.Write("  ");
                        }
                        
                    }
                    else if (arrayboard[row, column] == 2)
                    {

                        Console.Write("X ");
                    }
                    else if (arrayboard[row, column] == 3)
                    {

                        Console.Write(". ");
                    }

                }

                Console.WriteLine();
            }

        }

        private void DisplayWinner(bool wasCpuWiiner)
        {
            if (wasCpuWiiner)
            {
                Console.WriteLine("SORRY CPU WON THE GAME.");
                Console.WriteLine("Cpu gameboard.");
                DisplayGameBoard(_cpuPlayer.GetReferencesToMyGameBoard().GetArrayBoard(), true);

            }
            else
            {
                Console.WriteLine("CONGRATULATIONS YOU HAVE WON THE GAME !!!");
            }




        }

    }
}
