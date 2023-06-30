using System;
using System.Collections.Generic;

namespace BattleShips
{
    internal class HumanPlayer : Player
    {
        private GameBoard _gameBoard;
        private GameBoard _refToCpuBoard;
        private List<Ship> _cpulistOfShips;
        private List<Ship> _listOfShips;

        private List<String> _columnIndexes;

        public HumanPlayer()
        {

            _gameBoard = new GameBoard(GameRules.GameBoardNrOfRows, GameRules.GameBoardNrOfColumns);
            _listOfShips = new List<Ship>();

            _columnIndexes = new List<String> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        }
        public override void SetShips()
        {
            SetBattleshipsOnBoard();

            SetDestroyersOnBoard();
        }

        public override int Shoot()
        {
            int hit = 0;

            int hitShipID = 0;

            String displayMessage = ("Your shoot , input coordinates to shoot.");

            Coordinates coordinates = GetCoordinatesFromPlayer(displayMessage);

            _refToCpuBoard.UpdateArrayBoard(coordinates.Row, coordinates.Column);

            foreach (Ship ship in _cpulistOfShips)
            {
                if (!ship.IsShipDestroyed())
                {
                    hit = ship.wasShipHIt(coordinates.Row, coordinates.Column, out hitShipID);
                    if (hit > 0)
                    {
                        break;
                    }
                }
            }

            return hit;

        }
        public override List<Ship> GetReferencesToMyShips()
        {
            return _listOfShips;
        }

        public override GameBoard GetReferencesToMyGameBoard()
        {
            return _gameBoard;
        }
        public override bool CheckIFGameOver()
        {
            bool gameOver = false;

            int destroyedShipCounter = 0;

            foreach (Ship ship in _cpulistOfShips)
            {
                if (ship.IsShipDestroyed())
                {
                    destroyedShipCounter++;
                }

            }

            if (destroyedShipCounter == _cpulistOfShips.Count)
            {
                gameOver = true;
            }

            return gameOver;

        }
        public void SetReferencesToCpuShips(List<Ship> cpulistOfShips)
        {
            _cpulistOfShips = cpulistOfShips;

        }

        public void SetReferencesToCpuBoard(GameBoard cpuBoard)
        {
            _refToCpuBoard = cpuBoard;
        }

        private void SetBattleshipsOnBoard()
        {
            int NumberOFBattleships = GameRules.NumberOFBattleships;
            int LengthOFBattleship = GameRules.LengthOFBattleship;
            string NameOfBattleship = GameRules.NameOfBattleship;

            SetShipsOnBoard(NumberOFBattleships, LengthOFBattleship, NameOfBattleship);
        }

        private void SetDestroyersOnBoard()
        {
            int NumberOFDestroyers = GameRules.NumberOFDestroyers;
            int LengthOFDestroyers = GameRules.LengthOFDestroyer;
            string NameOfDestroyers = GameRules.NameOfDestroyer;

            SetShipsOnBoard(NumberOFDestroyers, LengthOFDestroyers, NameOfDestroyers);
        }

        private int CharToIntTranslation(String letter)
        {
            int number = -1;

            foreach (String chr in _columnIndexes)
            {
                if (chr == letter.ToUpper())
                {
                    number = _columnIndexes.IndexOf(chr);
                }

            }

            return number;

        }
        private bool GetInformationFromUserIfShipPositionISHorizontal()
        {
            bool horizontal = false;

            string input = "";

            do
            {
                Console.WriteLine();
                Console.WriteLine(String.Format("Do you want to place your ship horizontal , press Y for YES or N for NO."));
                Console.WriteLine();
                input = Console.ReadLine();

                if (input.ToUpper() == "Y")
                {
                    horizontal = true;
                }
                else if (input.ToUpper() == "N")
                {
                    horizontal = false;
                }
                else
                {
                    Console.WriteLine(String.Format("Your input was incorect. Try again"));
                }

            } while (input.ToUpper() != "Y" && input.ToUpper() != "N");

            return horizontal;
        }
        private Coordinates GetCoordinatesFromPlayer(String displayMessage)
        {
            Coordinates coordinates = null;
            string input = null;
            string errorMessage = null;

            do
            {
                Console.WriteLine();
                Console.WriteLine(String.Format(displayMessage));
                Console.WriteLine();

                input = Console.ReadLine();

                coordinates = ChangeUserInputIntoCoordinates(input, out errorMessage);

                if (errorMessage != null)
                {
                    Console.WriteLine(errorMessage);
                }

            } while (coordinates == null);

            return coordinates;

        }

        private Coordinates ChangeUserInputIntoCoordinates(string userInput, out string errorMessage)
        {
            Coordinates coordinates = null;
            errorMessage = null;
            int column;
            int row;

            if (userInput.Length > 1 && userInput.Length <= 3)
            {
                column = CharToIntTranslation(userInput.Substring(0, 1));
                if (column == -1)
                {
                    errorMessage = "You have input invalid column letter. ";
                }
                bool success = int.TryParse(userInput.Substring(1, userInput.Length - 1), out row);
                if (success)
                {
                    if (row < 0 || row > GameRules.GameBoardNrOfRows)
                    {
                        errorMessage += "Row you have input is out of boundaries.";
                    }
                }
                else
                {
                    errorMessage += String.Format("Conversion of '{0}' to row number failed. Please try again.", userInput.Substring(1, userInput.Length - 1));
                }
                if (errorMessage == null)
                {
                    coordinates = new Coordinates(row - 1, column);
                }

            }
            else
            {
                errorMessage = "Your input was too long or too short";
            }

            return coordinates;

        }

        private void SetShipsOnBoard(int numberOfShips, int shipLength, String shipName)
        {
            Coordinates coordinates = null;

            int sumOfFields;
            bool canShipBeplacedOnBoard = false;
            bool horizontal = false;
            bool shipSetProperly = false;
            string errorMessage = null;
            string displayMassgeForUser;
            int shipNr = 1;

            while (numberOfShips >= shipNr)
            {
                sumOfFields = 0;

                Console.WriteLine(String.Format("Time to set your {0} {1}", shipNr, shipName));

                horizontal = GetInformationFromUserIfShipPositionISHorizontal();

                do
                {
                    shipSetProperly = false;

                    errorMessage = null;

                    displayMassgeForUser = String.Format("Input {0} start coordinates, for example 'A5' or 'a5', where A represnts column and 5 row number.", shipName);

                    coordinates = GetCoordinatesFromPlayer(displayMassgeForUser);

                    canShipBeplacedOnBoard = CheckIfShipCanBeAddToBoard(shipLength, coordinates, horizontal, out errorMessage);

                    if (canShipBeplacedOnBoard)
                    {
                        _gameBoard.AddShipToBoard(coordinates.Row, coordinates.Column, shipLength, horizontal);
                        _listOfShips.Add(new Ship(coordinates.Row, coordinates.Column, horizontal, shipName, shipLength, _listOfShips.Count + 1));
                        shipSetProperly = true;
                        shipNr++;

                        Console.WriteLine(String.Format("Your {0} was set properlly.", shipName));
                        DisplayGameBoard(_gameBoard.GetArrayBoard(), true);

                    }

                    if (errorMessage != null)
                    {
                        Console.WriteLine(errorMessage);
                    }

                } while (!shipSetProperly);

            }

        }

        private bool CheckIfShipCanBeAddToBoard(int shipLength, Coordinates shipCoordinates, bool isShipPlacedHorizontal, out string errorMessage)
        {
            int sumOfFields = 0;
            errorMessage = null;
            bool shipCanBePlaced = true;

            if (isShipPlacedHorizontal)
            {
                // Check if player didn't place ship out of boudaries
                if (shipCoordinates.Column <= (GameRules.GameBoardNrOfColumns - shipLength))
                {
                    for (int i = 0; i < shipLength; i++)
                    {
                        sumOfFields += _gameBoard.GetArrayBoardValue(shipCoordinates.Row, shipCoordinates.Column + i);
                    }
                }
                else
                {
                    errorMessage = "You set your ship out of board.";
                    shipCanBePlaced = false;
                }

            }
            else
            {
                // Check if player didn't place ship out of boudaries
                if (shipCoordinates.Row <= (GameRules.GameBoardNrOfRows - shipLength))
                {
                    for (int i = 0; i < shipLength; i++)
                    {
                        sumOfFields += _gameBoard.GetArrayBoardValue(shipCoordinates.Row + i, shipCoordinates.Column);
                    }
                }
                else
                {
                    errorMessage = "You set your ship out of board.";
                    shipCanBePlaced = false;
                }

            }
            if (sumOfFields > 0)
            {
                errorMessage = "You set ship with coolision with nother ship.";
                shipCanBePlaced = false;
            }

            return shipCanBePlaced;

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
                    else if (arrayboard[row, column] == 1  )
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



    }
}
