using Microsoft.Xna.Framework;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.GameContent
{
    internal class Player
    {
        private Location location = new Location(0, 0, 0);

        public Player()
        {

        }
        public int X
        {
            get { return location.X; }
            set { location.X = value; }
        }
        public int Y
        {
            get { return location.Y; }
            set { location.Y = value; }
        }
        public int Z
        {
            get { return location.Z; }
            set { location.Z = value; }
        }

        internal void Move(int x, int y, int z)
        {
            location.index.X += x;
            location.index.Y += y;
            location.layer += z;
        }
    }
}
