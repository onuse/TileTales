using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TileTales.WorldGeneration;

internal class Voronoi {
    public IEnumerable<object> Regions { get; internal set; }

    internal static Voronoi FromPoints(List<Vector2> seedPoints) {
        throw new NotImplementedException();
    }
}