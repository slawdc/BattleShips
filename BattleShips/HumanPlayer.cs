using System;
using System.Collections.Generic;

namespace BattleShips
{
    internal class HumanPlayer : Player
    {
        public override int PlayerID { get; }
        private List<String> _columnIndexes;

        public HumanPlayer(int playerID)
        {
            PlayerID = playerID;          
            _columnIndexes = new List<String> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        }
        public override void SetShip(Ship ship, ref GameBoard gameBoard)
        {
            bool horizontal = false;
            Coordinates playerCoordinates;
            bool wasShipAddedToBoard = false;
            string errorMessage = null;

            Console.WriteLine(String.Format("Time to set your {0} ship, named: '{1}'", ship.ShipID, ship.ShipName));

            horizontal = GetInformationFromUserIfShipPositionISHorizontal();

            do
            {
                errorMessage = null;

                Console.WriteLine(String.Format("Input initial coordinates, for : {0}.", ship.ShipName));

                playerCoordinates = GetCoordinatesFromPlayer();

                wasShipAddedToBoard = TrySetShipOnBoard(ref ship, ref gameBoard,  playerCoordinates,  horizontal, out  errorMessage);

                if (errorMessage != null)
                {
                    Console.WriteLine(errorMessage);
                }

            } while (!wasShipAddedToBoard);

        }

        public override Coordinates Shoot()
        {
            Console.WriteLine(String.Format("Input coordinates you want to attack"));

            Coordinates coordinates = GetCoordinatesFromPlayer();

            return coordinates;
        }

        private int CharToIntTranslation(String letter)
        {
            int number = -1;

            foreach (String chr in _columnIndexes)
            {
                if (chr == letter.ToUpper())
                {
                    number = _columnIndexes.IndexOf(chr);
                    break;
                }

            }

            return number;

        }
        private bool GetInformationFromUserIfShipPositionISHorizontal()
        {
            bool horizontal = false;

            ConsoleKeyInfo input ;

            do
            {
                Console.WriteLine();
                Console.WriteLine(String.Format("Do you want to place your ship horizontal , press H for HORIZONTAL or V for VERTICAL."));
                Console.WriteLine();
                input = Console.ReadKey();
                Console.WriteLine();

                if (input.Key == ConsoleKey.H)
                {
                    horizontal = true;
                }
                else if (input.Key == ConsoleKey.V)
                {
                    horizontal = false;
                }
                else
                {
                    Console.WriteLine(String.Format("Your input was incorect. Try again"));
                }

                

            } while (input.Key != ConsoleKey.H && input.Key != ConsoleKey.V);

            return horizontal;
        }
        private Coordinates GetCoordinatesFromPlayer()
        {
            Coordinates coordinates = null;
            string input = null;
            string errorMessage = null;

            do
            {
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
                    coordinates = new Coordinates(row - 1, column, 0);
                }

            }
            else
            {
                errorMessage = "Your input was too long or too short";
            }

            return coordinates;

        }

        private bool TrySetShipOnBoard(ref Ship ship, ref GameBoard gameBoard, Coordinates playerCoordinates, bool horizontal, out string errorMessage)
        {
            int sumOfFields = 0;
            errorMessage = null;
            bool shipWasAddedTooBoard = true;
            List<Coordinates> tempCoordinates = new List<Coordinates>();


            if (horizontal)
            {
                // Check if player didn't place ship out of boudaries
                if (playerCoordinates.Column <= (GameRules.GameBoardNrOfColumns - ship.Length))
                {
                    for (int i = 0; i < ship.Length; i++)
                    {
                        sumOfFields += gameBoard.GetBoardCoordiante(playerCoordinates.Row, playerCoordinates.Column + i).ShipID;
                        tempCoordinates.Add(new Coordinates(playerCoordinates.Row, playerCoordinates.Column + i, ship.ShipID));
                    }
                }
                else
                {
                    errorMessage = "You set your ship out of board.";
                    shipWasAddedTooBoard = false;
                }

            }
            else // else vertical
            {
                // Check if player didn't place ship out of boudaries
                if (playerCoordinates.Row <= (GameRules.GameBoardNrOfRows - ship.Length))
                {
                    for (int i = 0; i < ship.Length; i++)
                    {
                        tempCoordinates.Add(new Coordinates(playerCoordinates.Row + i, playerCoordinates.Column, ship.ShipID));
                        sumOfFields += gameBoard.GetBoardCoordiante(playerCoordinates.Row + i, playerCoordinates.Column).ShipID;
                    }
                }
                else
                {
                    errorMessage = "You set your ship out of board.";
                    shipWasAddedTooBoard = false;
                }

            }
            if (sumOfFields == 0 && shipWasAddedTooBoard)
            {
                foreach (Coordinates coordinates in tempCoordinates)
                {
                    coordinates.ShipID = ship.ShipID;
                    ship.AddCoordinates(coordinates);
                    gameBoard.UpdateArrayBoardCoordinates(coordinates);
                    
                }

                gameBoard.AddShipToList(ship);

            }
            else if (sumOfFields > 0)
            {
                errorMessage = "You set ship with coolision with nother ship.";
                shipWasAddedTooBoard = false;
            }

            return shipWasAddedTooBoard;

        }

    }
}
