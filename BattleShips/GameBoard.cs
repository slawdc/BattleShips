namespace BattleShips
{
    internal class GameBoard
    {
        private int[,] _arrayboard;

        public GameBoard(int nrOfRows, int nrOfColumns)
        {
            _arrayboard = new int[nrOfRows, nrOfColumns];
        }

        public int[,] GetArrayBoard()
        {
            return _arrayboard;
        }

        public int GetArrayBoardValue(int row, int column)
        {
            return _arrayboard[row, column];
        }

        public void SetArrayBoardValue(int row, int column, int value)
        {
            _arrayboard[row, column] = value;
        }

        public void UpdateArrayBoard(int row, int column)
        {
            // if on field was ship (field value  = 1), set hit value =  2
            if (_arrayboard[row, column] == 1)
            {
                _arrayboard[row, column] = 2;
            }
            // if field was empty set missed shoot
            else if (_arrayboard[row, column] == 0)
            {
                _arrayboard[row, column] = 3;
            }
        }

        public void AddShipToBoard(int row, int column, int length, bool horizontal)
        {
            if (horizontal)
            {
                for (int i = 0; i < length; i++)
                {
                    SetArrayBoardValue(row, column + i, 1);
                }
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    SetArrayBoardValue(row + i, column, 1);
                }

            }

        }

    }
}
