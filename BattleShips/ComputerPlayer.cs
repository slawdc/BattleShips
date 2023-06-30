using System;
using System.Collections.Generic;


namespace BattleShips
{
    internal class ComputerPlayer : Player
    {

        private GameBoard _computerBoard;
        private GameBoard _refToPlayerBoard;

        private List<Ship> _refToPlayerShips;
        private List<Ship> _listOfShips;

        private Random random;

        private List<Coordinates> _targetList;
        private List<Coordinates> _expectedCoordinates;

        private List<int> _IDsOfHitShips;
        private int _numberOfShipDestroyed;

        public ComputerPlayer(List<Ship> refPlayerShips, GameBoard refToPlayerBoard)
        {

            _computerBoard = new GameBoard(GameRules.GameBoardNrOfRows, GameRules.GameBoardNrOfColumns);
            _refToPlayerBoard = refToPlayerBoard;
            _refToPlayerShips = refPlayerShips;
            _IDsOfHitShips = new List<int>();

            _listOfShips = new List<Ship>();
            _expectedCoordinates = new List<Coordinates>();
            random = new Random();

            _numberOfShipDestroyed = 0;
            CreateTargetListOfCoordinates();

            SetShips();
        }



        public override int Shoot()
        {
            // shoot function return 0 = miss , 1 = hit , 2 = ship destroyed
            int hit = 0;
           
            Coordinates target;

            if (_expectedCoordinates.Count > 0)
            {
                target = GetExpectedCoordinates();
            }
            else
            {
                target = GetRandomTarget();
            }

            // update board UI
            _refToPlayerBoard.UpdateArrayBoard(target.Row, target.Column);

            hit = CheckIfShipWasHit(target);

            return hit;

        }

        public override bool CheckIFGameOver()
        {
            bool gameOver = false;

            int destroyedShipCounter = 0;

            foreach (Ship ship in _refToPlayerShips)
            {
                if (ship.IsShipDestroyed())
                {
                    destroyedShipCounter++;
                }

            }
            if (destroyedShipCounter == _refToPlayerShips.Count)
            {
                gameOver = true;
            }
            return gameOver;
        }

        private int CheckIfShipWasHit(Coordinates coordinates)
        {
            int hit = 0;
            int hitShipID;

            foreach (Ship ship in _refToPlayerShips)
            {
                // search for a hit only in not destroyed ships
                if (!ship.IsShipDestroyed())
                {
                    hit = ship.wasShipHIt(coordinates.Row, coordinates.Column, out hitShipID);
                    if (hit == 1)
                    {
                        //If ship was hit , we add close coordinates to check list which cpu will shoot in first place
                        AddExpecedCoordinates(coordinates.Row, coordinates.Column);
                        AddNewIDOFHitship(hitShipID);
                        break;
                    }
                    if (hit == 2)
                    {
                        CheckExpectedCoordinates();
                        _numberOfShipDestroyed++;
                        break;
                    }
                }
            }

            return hit;

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
                    _targetList.Add(new Coordinates(i, j));
                }
            }
        }

        public override void SetShips()
        {
            SetBattleshipsOnBoard();

            SetDestroyersOnBoard();
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

        private void SetShipsOnBoard(int numberOfShips, int shipLength, String shipName)
        {
            int row;
            int column;
            int sumOfFields;
            bool horizontal;

            while (numberOfShips != 0)
            {
                sumOfFields = 0;
                // random vertical or horizontal position of ship.
                horizontal = Convert.ToBoolean(random.Next(2));

                if (horizontal)
                {
                    row = random.Next(GameRules.GameBoardNrOfRows);
                    column = random.Next(GameRules.GameBoardNrOfColumns - shipLength);

                    for (int i = 0; i < shipLength; i++)
                    {
                        sumOfFields += _computerBoard.GetArrayBoardValue(row, column + i);
                    }

                }
                else
                {
                    row = random.Next(GameRules.GameBoardNrOfRows - shipLength);
                    column = random.Next(GameRules.GameBoardNrOfColumns);

                    for (int i = 0; i < shipLength; i++)
                    {
                        sumOfFields += _computerBoard.GetArrayBoardValue(row + i, column);
                    }

                }

                if (sumOfFields == 0)
                {
                    _computerBoard.AddShipToBoard(row, column, shipLength, horizontal);
                    _listOfShips.Add(new Ship(row, column, horizontal, shipName, shipLength, _listOfShips.Count + 1));

                    numberOfShips--;
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
                tempCoordinates.Add(new Coordinates(row + 1, column));
            }
            if (row != 0)
            {
                tempCoordinates.Add(new Coordinates(row - 1, column));
            }
            if (column + 1 < GameRules.GameBoardNrOfColumns)
            {
                tempCoordinates.Add(new Coordinates(row, column + 1));
            }

            if (column != 0)
            {
                tempCoordinates.Add(new Coordinates(row, column - 1));

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

        public override List<Ship> GetReferencesToMyShips()
        {
            return _listOfShips;
        }

        public override GameBoard GetReferencesToMyGameBoard()
        {
            return _computerBoard;
        }
    }
}
