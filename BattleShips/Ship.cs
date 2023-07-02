using System.Collections.Generic;

namespace BattleShips
{
    internal class Ship
    {
        private int _length;
        private string _shipName;

        private int _shipId { get; }
        private List<Coordinates> _shipsCoordinates;

        public Ship(string shipName, int shipLength, int shipID)
        {
            _shipsCoordinates = new List<Coordinates>();
            _shipId = shipID;
            _shipName = shipName;
            _length = shipLength;

        }
        public int Length
        {
            get { return _length; }

        }
        public string ShipName
        {
            get { return _shipName; }

        }
        public int ShipID
        {
            get { return _shipId; }
        }

        public void AddCoordinates(Coordinates coordinates)
        {
            _shipsCoordinates.Add(coordinates);
        }

        public bool IsShipDestroyed()
        {
            bool isDestroyed = true;

            foreach (Coordinates coordinates in _shipsCoordinates)
            {
                if (!coordinates.Destroyed)

                    isDestroyed = false;
            }

            return isDestroyed;

        }

    }
}
