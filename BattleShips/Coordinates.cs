namespace BattleShips
{
    internal class Coordinates
    {
        public int Row;
        public int Column;
        public bool Destroyed;

        public Coordinates(int row, int column)
        {
            Row = row;
            Column = column;
            Destroyed = false;

        }

    }
}
