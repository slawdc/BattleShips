using System;
using System.Collections.Generic;


namespace BattleShips
{
    internal class CPUPlayer : Player
    {
        public override int PlayerID { get; }

        private List<Coordinates> _targetList;
        private List<Coordinates> _expectedCoordinates;
        private List<int> _IDsOfHitShips;
        private int _numberOfShipDestroyed;
        private Random random;
        public CPUPlayer(int playerID)
        {
            PlayerID = playerID;
            _IDsOfHitShips = new List<int>();
            _expectedCoordinates = new List<Coordinates>();
            random = new Random();
            _numberOfShipDestroyed = 0;
            CreateTargetListOfCoordinates();
        }
        public override Coordinates Shoot()
        {
            Coordinates target;

            if (_expectedCoordinates.Count > 0)
            {
                target = GetExpectedCoordinates();
            }
            else
            {
                target = GetRandomTarget();
            }

            return target;

        }
        public override void SetShip(Ship ship, ref GameBoard gameBoard)
        {
            int row;
            int column;
            int sumOfFields; // check if already ship is on board
            bool horizontal;

            List<Coordinates> tempCoordinates = new List<Coordinates>();

            do
            {

                tempCoordinates.Clear();
                sumOfFields = 0;

                // random vertical or horizontal position of ship.
                horizontal = Convert.ToBoolean(random.Next(2));

                if (horizontal) // if ship is placed horizontal
                {
                    row = random.Next(GameRules.GameBoardNrOfRows);
                    column = random.Next(GameRules.GameBoardNrOfColumns - ship.Length);

                    for (int i = 0; i < ship.Length; i++)
                    {
                        tempCoordinates.Add(new Coordinates(row, column + i, ship.ShipID));
                        sumOfFields += gameBoard.GetShipIdFromCoordinates(row, column + i);
                    }

                }
                else // if ship is placed vertical
                {
                    row = random.Next(GameRules.GameBoardNrOfRows - ship.Length);
                    column = random.Next(GameRules.GameBoardNrOfColumns);

                    for (int i = 0; i < ship.Length; i++)
                    {
                        tempCoordinates.Add(new Coordinates(row + i, column, ship.ShipID));
                        sumOfFields += gameBoard.GetShipIdFromCoordinates(row + i, column);
                    }

                }

                if (sumOfFields == 0)
                {
                    foreach (Coordinates coordinates in tempCoordinates)
                    {
                        coordinates.ShipID = ship.ShipID;
                        ship.AddCoordinates(coordinates);
                        gameBoard.UpdateArrayBoardCoordinates(coordinates);

                    }

                    gameBoard.AddShipToList(ship);

                }

            } while (sumOfFields != 0);

        }

        public void UpdateCpuLogic(int shoot, int fieldID, Coordinates coordinates)
        {
            if (shoot == 1)
            {
                //If ship was hit , we add close coordinates to check list which cpu will shoot in first place
                AddExpecedCoordinates(coordinates.Row, coordinates.Column);

                AddNewIDOFHitship(fieldID);

            }
            if (shoot == 2)
            {
                CheckExpectedCoordinates();
                _numberOfShipDestroyed++;

            }

        }

        private Coordinates GetRandomTarget()
        {
            Coordinates target;

            int index = random.Next(_targetList.Count);

            target = _targetList[index];

            _targetList.RemoveAt(index);

            return target;

        }
        private Coordinates GetExpectedCoordinates()
        {
            Coordinates target;

            int index = random.Next(_expectedCoordinates.Count);

            target = _expectedCoordinates[index];

            _expectedCoordinates.RemoveAt(index);

            return target;

        }

        private void AddNewIDOFHitship(int shipID)
        {
            bool wasIDAdded = false;

            foreach (int ID in _IDsOfHitShips)
            {
                if (ID == shipID)
                {
                    wasIDAdded = true;
                }
            }
            if (!wasIDAdded)
            {
                _IDsOfHitShips.Add(shipID);
            }
        }

        private void CreateTargetListOfCoordinates()
        {
            _targetList = new List<Coordinates>();

            for (int i = 0; i < GameRules.GameBoardNrOfRows; i++)
            {
                for (int j = 0; j < GameRules.GameBoardNrOfColumns; j++)
                {
                    _targetList.Add(new Coordinates(i, j, 0));
                }
            }
        }

        private void CheckExpectedCoordinates()
        {

            if (_numberOfShipDestroyed == _IDsOfHitShips.Count)
            {
                foreach (Coordinates tempCoors in _expectedCoordinates)
                {
                    _targetList.Add(tempCoors);
                }

                _expectedCoordinates.Clear();
            }

        }
        private void AddExpecedCoordinates(int row, int column)
        {// Setting close coordinates if last computer shoot was hit.

            List<Coordinates> tempCoordinates = new List<Coordinates>();

            // Checking if close coordinates are not out of boundries, and adding them to temp list.
            if (row + 1 < GameRules.GameBoardNrOfRows)
            {
                tempCoordinates.Add(new Coordinates(row + 1, column, 0));
            }
            if (row != 0)
            {
                tempCoordinates.Add(new Coordinates(row - 1, column, 0));
            }
            if (column + 1 < GameRules.GameBoardNrOfColumns)
            {
                tempCoordinates.Add(new Coordinates(row, column + 1, 0));
            }

            if (column != 0)
            {
                tempCoordinates.Add(new Coordinates(row, column - 1, 0));

            }

            for (int i = _targetList.Count - 1; i >= 0; i--)
            {
                foreach (Coordinates tempCoors in tempCoordinates)
                {
                    if (_targetList[i].Row == tempCoors.Row && _targetList[i].Column == tempCoors.Column)
                    {
                        _expectedCoordinates.Add(tempCoors);

                        _targetList.RemoveAt(i);

                    }
                }
            }
        }


    }
}
