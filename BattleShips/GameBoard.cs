using System.Collections.Generic;

namespace BattleShips
{
    internal class GameBoard
    {
        private Coordinates[,] _arrayboard;
        private List<Ship> _listOfShips;

        public GameBoard()
        {
            _listOfShips = new List<Ship>();

            _arrayboard = new Coordinates[GameRules.GameBoardNrOfRows, GameRules.GameBoardNrOfColumns];

            for (int i = 0; i < GameRules.GameBoardNrOfRows; i++)
            {
                for (int j = 0; j < GameRules.GameBoardNrOfColumns; j++)
                {
                    _arrayboard[i, j] = new Coordinates(i, j, 0);
                }

            }
        }

        public bool AreAllShipsDestroyed()
        {
            bool allShipDestroyed = true;

            foreach (Ship ship in _listOfShips)
            {
                if (!ship.IsShipDestroyed())
                {
                    allShipDestroyed = false;
                }
            
            }
            return allShipDestroyed; 
        }

        public int GetBoardNrOfRows()
        {
            return _arrayboard.GetLength(0);
        }

        public int GetBoardNrOfColumns()
        {
            return _arrayboard.GetLength(1);
        }

        public void AddShipToList(Ship ship)
        {
            _listOfShips.Add(ship);
        }

        public Coordinates GetBoardCoordiante(int row, int column)
        {
            return _arrayboard[row, column];
        }

        public int GetShipIdFromCoordinates(int row, int column)
        {
            return _arrayboard[row, column].ShipID;
        }

        public void UpdateArrayBoardCoordinates(Coordinates coordinates)
        {
            _arrayboard[coordinates.Row, coordinates.Column] = coordinates;
        }

        public int ShootTarget(Coordinates coordinates)
        {
            int shoot = 0;

            if (!_arrayboard[coordinates.Row, coordinates.Column].Destroyed && _arrayboard[coordinates.Row, coordinates.Column].ShipID > 0)
            {
                shoot = 1;
                _arrayboard[coordinates.Row, coordinates.Column].Destroyed = true;

                foreach (Ship ship in _listOfShips)
                {
                    if (ship.ShipID == _arrayboard[coordinates.Row, coordinates.Column].ShipID && ship.IsShipDestroyed())
                    {
                        shoot = 2;
                    }

                }

            }
            else
            {
                _arrayboard[coordinates.Row, coordinates.Column].Destroyed = true;
            }

            return shoot;

        }

    }
}
