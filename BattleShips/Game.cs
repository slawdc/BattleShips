using System;
using System.Collections.Generic;

namespace BattleShips
{
    internal class Game
    {
        private CPUPlayer _cpuPlayer;
        private HumanPlayer _humanPlayer;

        private GameBoard _humanGameboard;
        private GameBoard _cpuGameboard;

        private List<Ship> _cpuShips;
        private List<Ship> _humanShips;


        public Game()
        {
            _humanPlayer = new HumanPlayer(1);
            _cpuPlayer = new CPUPlayer(2);

            _humanGameboard = new GameBoard();
            _cpuGameboard = new GameBoard();

            GameInit();

        }

        private void GameInit()
        {
            _cpuShips = CreateListOFShips();
            _humanShips = CreateListOFShips();

            foreach (Ship ship in _cpuShips)
            {
                _cpuPlayer.SetShip(ship, ref _cpuGameboard);
         
            }

            foreach (Ship ship in _humanShips)
            {
                _humanPlayer.SetShip(ship, ref _humanGameboard);
                Console.WriteLine(String.Format("Your {0} was set properly on the board.", ship.ShipName));
                DisplayGameBoard(_humanGameboard, true);
            }

        }

        public void GameLoop()
        {

            bool cpuWiner = false;
            bool gameOver = false;
            int shoot;
            Coordinates targetCoordinates;

            do
            {
                do// player shoot until miss;
                {

                    Console.WriteLine("CPU GAMEBOARD");
                    DisplayGameBoard(_cpuGameboard, false);

                    targetCoordinates = _humanPlayer.Shoot();
                   
                    shoot = _cpuGameboard.ShootTarget(targetCoordinates);

                    DisplayShootInformation(shoot, "You");


                    if (_cpuGameboard.AreAllShipsDestroyed())
                    {
                        gameOver = true;

                    }

                } while (shoot > 0 ^ gameOver);

                if (!gameOver)
                {
                    do
                    {
                        targetCoordinates = _cpuPlayer.Shoot();

                        shoot = _humanGameboard.ShootTarget(targetCoordinates);

                        Console.WriteLine("HUMAN GAMEBOARD");
                        DisplayShootInformation(shoot, "Cpu");

                        DisplayGameBoard(_humanGameboard, true);

                        _cpuPlayer.UpdateCpuLogic(shoot, _humanGameboard.GetBoardCoordiante(targetCoordinates.Row, targetCoordinates.Column).ShipID, targetCoordinates);

                        if (_humanGameboard.AreAllShipsDestroyed())
                        {
                            gameOver = true;
                            cpuWiner = true;
                        }

                    } while (shoot > 0 ^ gameOver);


                }


            } while (!gameOver);

            DisplayWinner(cpuWiner);


        }

        private void DisplayShootInformation(int shoot, string who)
        {
            if (shoot == 0)
            {
                Console.WriteLine(String.Format("{0} missed.", who));
            }
            else if (shoot == 1)
            {
                Console.WriteLine(String.Format("{0} hit ship.", who));

            }
            else if (shoot == 2)
            {
                Console.WriteLine(String.Format("{0} sunk ship.", who));
            }

        }

        public void DisplayGameBoard(GameBoard gameBoard, bool displayShips)
        {

            Console.WriteLine();
            Console.WriteLine("   A B C D E F G H I J");


            for (int row = 0; row < gameBoard.GetBoardNrOfRows(); row++)
            {

                if (row == 9)
                {
                    Console.Write(row + 1 + " ");
                }
                else
                {
                    Console.Write(row + 1 + "  ");
                }

                for (int column = 0; column < gameBoard.GetBoardNrOfColumns(); column++)
                {

                    if (gameBoard.GetBoardCoordiante(row, column).ShipID == 0 && !gameBoard.GetBoardCoordiante(row, column).Destroyed)
                    {
                        Console.Write("  ");
                    }
                    else if (gameBoard.GetBoardCoordiante(row, column).ShipID == 0 && gameBoard.GetBoardCoordiante(row, column).Destroyed)
                    {
                        Console.Write(". ");
                    }
                    else if (gameBoard.GetBoardCoordiante(row, column).ShipID > 0 && !gameBoard.GetBoardCoordiante(row, column).Destroyed)
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
                    else if (gameBoard.GetBoardCoordiante(row, column).ShipID > 0 && gameBoard.GetBoardCoordiante(row, column).Destroyed)
                    {
                        Console.Write("X ");
                    }

                }

                Console.WriteLine();
            }

        }

        private void DisplayWinner(bool wasCpuWinner)
        {
            if (wasCpuWinner)
            {
                Console.WriteLine("SORRY CPU WON THE GAME.");
                Console.WriteLine("Cpu gameboard:");
                DisplayGameBoard(_cpuGameboard, true);

            }
            else
            {
                Console.WriteLine("CONGRATULATIONS YOU HAVE WON THE GAME !!!");
            }

            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
            Console.WriteLine("");

        }

        private List<Ship> CreateListOFShips()
        {
            int shipCounter = 1;
            List<Ship> list = new List<Ship>();

            for (int i = GameRules.NumberOFBattleships; i != 0; i--)
            {
                list.Add(new Ship(GameRules.NameOfBattleship, GameRules.LengthOFBattleship, shipCounter++));
            }

            for (int i = GameRules.NumberOFDestroyers; i != 0; i--)
            {
                list.Add(new Ship(GameRules.NameOfDestroyer, GameRules.LengthOFDestroyer, shipCounter++));
            }

            return list;
        }


    }
}
