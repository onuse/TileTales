using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TileTales.WorldGeneration;

internal class TectonicPlate {
    private int id;
    private object region;

    public TectonicPlate(int id, object region) {
        this.id = id;
        this.region = region;
    }

    public Vector2 CenterOfMass { get; internal set; }

    internal void Draw(SpriteBatch spriteBatch) {
        throw new NotImplementedException();
    }

    internal void Move(float timeStep) {
        throw new NotImplementedException();
    }
}