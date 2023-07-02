namespace BattleShips
{
    internal class Coordinates
    {
        public int Row;
        public int Column;
        public bool Destroyed;
        public int ShipID;

        public Coordinates(int row, int column , int shipID)
        {
            Row = row;
            Column = column;
            Destroyed = false;
            ShipID = shipID;

        }

    }
}
