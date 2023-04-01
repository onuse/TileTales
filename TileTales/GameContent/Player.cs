using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private int _objectId;

        public Player()
        {
            Location = Point3D.Zero;
        }
        public String Name
        {
            get;
            set;
        }

        public int X
        {
            get { return Location.X; }
            set { Location = new Point3D(value, Y, Z); }
        }
        public int Y
        {
            get { return Location.Y; }
            set { Location = new Point3D(X, value, Z); }
        }
        public int Z
        {
            get { return Location.Z; }
            set { Location = new Point3D(X, Y, value); }
        }

        public int ObjectId
        {
            get { return _objectId; }
        }

        public Texture2D Avatar { get; internal set; }
        public Point3D Location { get; internal set; }

        internal void Init(PlayerObjectInfo playerObjectInfo, ContentLibrary contentLib)
        {
            Avatar = contentLib.GetSprite(playerObjectInfo.Avatar + ".png");
            Name = playerObjectInfo.Name;
            Location = new Point3D(playerObjectInfo.X, playerObjectInfo.Y, playerObjectInfo.Z);
            _objectId = playerObjectInfo.ObjectId;

        }

        internal void Move(int x, int y, int z)
        {
            Location = Location.Translate(x, y, z);
        }

        internal void Teleport(int x, int y, int z)
        {
            Location = new Point3D(x, y, z);
        }

        internal void Teleport(Point3D newLoc)
        {
            Location = newLoc;
        }

        public override string ToString()
        {
            return $"Player: {X}, {Y}, {Z}";
        }
    }
}
