using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Net.Tiletales.Network.Proto.Game;
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
        private int _objectId;

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

        public int ObjectId
        {
            get { return _objectId; }
        }

        internal void init(PlayerObjectInfo playerObjectInfo)
        {
            X = playerObjectInfo.X;
            Y = playerObjectInfo.Y;
            Z = playerObjectInfo.Z;
            _objectId = playerObjectInfo.ObjectId;

        }

        internal void Move(int x, int y, int z)
        {
            location.index.X += x;
            location.index.Y += y;
            location.layer += z;
        }

        public override string ToString()
        {
            return $"Player: {X}, {Y}, {Z}";
        }

        internal void Teleport(int x, int y, int z)
        {
            location.index.X = x;
            location.index.Y = y;
            location.layer = z;
        }
    }
}
