﻿using Microsoft.Xna.Framework;
using System;
using TileTales.Graphics;

namespace TileTales.GameContent {
    internal readonly struct Point3D: IEquatable<Point3D>, IComparable<Point3D> {
        public static Point3D Zero { get; }

        public readonly Point Index;
        public readonly int Layer;

        public Point3D(int posX, int posY, int posZ) {
            Index = new Point(posX, posY);
            Layer = posZ;
        }

        public int X => Index.X;
        public int Y => Index.Y;
        public int Z => Layer;

        public override string ToString() {
            return X + "_" + Y + "_" + Z;
        }

        public bool Equals(Point3D other) {
            return X == other.X && Y == other.Y && Z == other.Z;
        }
        public override bool Equals(object obj) {
            if (obj is null) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tile)obj);
        }

        public int CompareTo(Point3D other) {
            int result = X.CompareTo(other.X);
            if (result != 0) return result;

            result = Y.CompareTo(other.Y);
            if (result != 0) return result;

            return Z.CompareTo(other.Z);
        }

        public override int GetHashCode() {
            return HashCode.Combine(X, Y, Z);
        }

        internal Point3D Translate(int x, int y, int z) {
            return new Point3D(X + x, Y + y, Z + z);
        }
    }

}
