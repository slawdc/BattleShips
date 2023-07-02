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
        public abstract int PlayerID { get; }
        public abstract Coordinates Shoot();
        public abstract void SetShip(Ship ship, ref GameBoard gameBoard);

    }
}
