using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TileTales.GameContent
{
    /**
     *  The Canvas object is where all the drawing happens.
     *  It holds the scene graph (tile collections, sprites etc) and does the necessary calculations for perspective/zoom.
     */
    internal class Canvas
    {
        private readonly TileTalesGame game;
        private readonly GraphicsDeviceManager graphicsManager;
        private readonly GraphicsDevice graphics;
        private readonly ContentLibrary contentLib;

        private SpriteBatch batch;
        Texture2D chestTexture;
        private float scale = 1f;
        private float deltaScrollWheelValue = 0;
        private float currentScrollWheelValue = 0;

        public Canvas(TileTalesGame tileTalesGame, GraphicsDeviceManager graphicsManager)
        {
            this.game = tileTalesGame;
            this.contentLib = game.ContentLibrary;
            this.graphicsManager = graphicsManager;
            this.graphics = graphicsManager.GraphicsDevice;
        }

        public void LoadContent()
        {
            batch = new SpriteBatch(graphics);
            chestTexture = contentLib.GetSprite("chest.png");
        }

        public void Draw(GameWorld world, GameTime gameTime, int viewPortWidth, int viewPortHeight)
        {
            Color tint = Color.White;
            Player p = world.player;
            // draw chestTexture
            batch.Begin();
            //batch.Draw(chestTexture, new Vector2(100, 100), Color.White);
            //batch.End();

            // Draw 3 x 3 chunks around player
            int chunksX = 3;
            int chunksY = 3;
            Chunk[] chunks = world.GetChunksAroundPlayer(chunksX, chunksY);
            for (int y = 0; y < chunksY; y++)
            {
                for (int x = 0; x < chunksX; x++)
                {
                    int index = x + y * chunksX;
                    Chunk chunk = chunks[index];
                    if (chunk != null)
                    {
                        Texture2D texture = chunk.Texture;
                        int textWidth = (int)(texture.Bounds.Width * scale);
                        int textHeight = (int)(texture.Bounds.Height * scale);
                        int texturePosX = (-(textWidth / 2 * chunksX) + x * textWidth) + viewPortWidth / 2 + textWidth / 2;
                        int texturePosY = (-(textHeight / 2 * chunksY) + y * textHeight) +viewPortHeight / 2 + textWidth / 2;

                        texturePosX -= p.X * 16;
                        texturePosY -= p.Y * 16;

                        float rotation = 0f;
                        float layerDepth = 1f;
                        Vector2 pos = new Vector2(texturePosX, texturePosY);
                        Vector2 origin = new Vector2(p.X * 16 * scale, p.Y * 16 * scale);
                        //batch.Draw(chunk.Texture, pos, tint);
                        batch.Draw(chunk.Texture, pos, null, tint, rotation, origin, scale, SpriteEffects.None, layerDepth);
                    }
                }
            }
            batch.Draw(chestTexture, new Vector2(100, 100), tint);
            batch.End();
        }

        internal void scrollWheelValue(int scrollWheelValue)
        {
            float MAX_VALUE = 1000f;
            float MIN_VALUE = -1000f;

            deltaScrollWheelValue = scrollWheelValue - currentScrollWheelValue;
            currentScrollWheelValue += (deltaScrollWheelValue / 10);

            if (currentScrollWheelValue > 0)
            {
                currentScrollWheelValue = Math.Min(MAX_VALUE, Math.Max(MIN_VALUE, currentScrollWheelValue));
            }
            else
            {
                currentScrollWheelValue = Math.Max(MIN_VALUE, currentScrollWheelValue);
            }

            //System.Diagnostics.Debug.WriteLine("currentScrollWheelValue: " + currentScrollWheelValue);

            scale = 1f + (float)currentScrollWheelValue / 1000f;

            //System.Diagnostics.Debug.WriteLine("scale: " + scale);

            /*int value = scrollWheelValue - lastScrollWheelValue;
            if (value > 0)
            {
                scale = 1.1f;
            } else
            {
                scale = 0.9f;
            }
            lastScrollWheelValue = scrollWheelValue;*/
        }
    }
}
