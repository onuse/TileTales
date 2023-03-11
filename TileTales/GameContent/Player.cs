using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Net.Tiletales.Network.Proto.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace TileTales.GameContent
{
    internal class Player
    {
        private Point3D location = new Point3D(0, 0, 0);
        private int _objectId;

        public Player()
        {
            
        }
        public int X
        {
            get { return location.X; }
            set { location = new Point3D(value, Y, Z); }
        }
        public int Y
        {
            get { return location.Y; }
            set { location = new Point3D(X, value, Z); }
        }
        public int Z
        {
            get { return location.Z; }
            set { location = new Point3D(X, Y, value); }
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
            location = location.translate(x, y, z);
        }

        public override string ToString()
        {
            return $"Player: {X}, {Y}, {Z}";
        }

        internal void Teleport(int x, int y, int z)
        {
            location = new Point3D(x, y, z);
        }
    }
}
