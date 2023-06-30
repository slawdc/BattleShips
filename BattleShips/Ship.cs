using System;
using System.Collections.Generic;

namespace BattleShips
{
    internal class Ship
    {
        private int _shipLength;
        private List<Coordinates> listOfCoordinates;
        private string _shipName;
        private bool _isShipDestroyed;
        private int _shipId;

        public Ship(int startRow, int startColumn, bool horizontal, string shipName, int shipLength, int shipID)
        {
            listOfCoordinates = new List<Coordinates>();

            _shipId = shipID;

            _shipName = shipName;

            _shipLength = shipLength;

            if (horizontal)
            {
                for (int i = 0; i < _shipLength; i++)
                {
                    listOfCoordinates.Add(new Coordinates(startRow, startColumn + i));
                }
            }
            else
            {
                for (int i = 0; i < _shipLength; i++)
                {
                    listOfCoordinates.Add(new Coordinates(startRow + i, startColumn));
                }
            }
        }

        public int wasShipHIt(int row, int column, out int shipId)
        {
            int hit = 0;
            shipId = 0;
            int hitCounter = 0;

            foreach (Coordinates coordinates in listOfCoordinates)
            {
                if (coordinates.Destroyed)
                {
                    hitCounter++;
                }

                if ((coordinates.Row == row && coordinates.Column == column) && !coordinates.Destroyed)
                {
                    coordinates.Destroyed = true;
                    hitCounter++;
                    hit = 1;
                    shipId = _shipId;
                }

            }

            if (hitCounter == _shipLength)
            {
                _isShipDestroyed = true;
                hit = 2;
            }

            return hit;

        }

        public bool IsShipDestroyed()
        {
            return _isShipDestroyed;

        }

        public void DisplayNameAndCoordinates()
        {
            foreach (Coordinates coordinate in listOfCoordinates)
            {
                Console.WriteLine("Name: " + _shipName + " " + ", row: " + coordinate.Row + " , col: " + coordinate.Column);
            }

        }

    }
}
