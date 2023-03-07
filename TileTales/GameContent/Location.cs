using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.GameContent
{
    internal class Location : IEquatable<Location>, IComparable<Location>
    {
        public Point index;
        public int layer;

        public Location(int posX, int posY, int posZ)
        {
            index.X = posX;
            index.Y = posY;
            layer = posZ;
        }
        public int X
        {
            get { return index.X; }
            set { index.X = value; }
        }
        public int Y
        {
            get { return index.Y; }
            set { index.Y = value; }
        }
        public int Z
        {
            get { return layer; }
            set { layer = value; }
        }

        public override string ToString()
        {
            return X + "_" + Y + "_" + Z;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Location);
        }

        public bool Equals(Location other)
        {
            return other != null &&
                   X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }

        public int CompareTo(Location other)
        {
            if (other == null) return 1;

            int result = X.CompareTo(other.X);
            if (result != 0) return result;

            result = Y.CompareTo(other.Y);
            if (result != 0) return result;

            return Z.CompareTo(other.Z);
        }

        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }
    }
}
