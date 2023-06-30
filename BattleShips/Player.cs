using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace BattleShips
{
    internal abstract class Player
    {
        public abstract int Shoot();

        public abstract void SetShips();

        public abstract List<Ship> GetReferencesToMyShips();

        public abstract GameBoard GetReferencesToMyGameBoard();

        public abstract bool CheckIFGameOver();

    }
}
