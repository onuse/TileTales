using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using SkiaSharp;
using MonoGame.Extended;
using TileTales.GameContent;
using Myra.Graphics2D.UI;

namespace TileTales.Graphics
{
    /**
     *  The Canvas object is where all the drawing happens.
     *  It holds the scene graph (tile collections, sprites etc) and does the necessary calculations for perspective/zoom.
     */
    internal class GameRenderer
    {
        private readonly TileTalesGame game;
        private readonly GraphicsDeviceManager graphicsManager;
        private readonly GraphicsDevice graphics;
        private readonly ContentLibrary contentLib;

        private SpriteBatch batch;

        GameSettings gameSettings = new GameSettings(1000, 1000, 16, 16);

        public GameRenderer(TileTalesGame tileTalesGame, GraphicsDeviceManager graphicsManager)
        {
            game = tileTalesGame;
            contentLib = game.ContentLibrary;
            this.graphicsManager = graphicsManager;
            graphics = graphicsManager.GraphicsDevice;
        }

        public void LoadContent()
        {
            batch = new SpriteBatch(graphics);
        }

        public void Draw(GameWorld world, GameTime gameTime, int viewPortWidth, int viewPortHeight, float scale)
        {
            Color tint = Color.White;
            Player p = world.player;

            const int chunksX = 32;
            const int chunksY = 32;
            Chunk[] chunks = world.GetChunksAroundPlayer(chunksX, chunksY);
            float playerX = p.X * scale;
            float playerY = p.Y * scale;

            batch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.Default, null, null, null);
            for (int y = 0; y < chunksY; y++)
            {
                for (int x = 0; x < chunksX; x++)
                {
                    int index = x + y * chunksX;
                    Chunk chunk = chunks[index];
                    if (chunk != null)
                    {
                        Texture2D texture = chunk.Texture;
                        int textWidth = (int)(texture.Width * scale);
                        int textHeight = (int)(texture.Height * scale);
                        int texturePosX = (int)Math.Round(x * textWidth + (viewPortWidth - textWidth * chunksX) / 2 - playerX);
                        int texturePosY = (int)Math.Round(y * textHeight + (viewPortHeight - textHeight * chunksY) / 2 - playerY);

                        float rotation = 0f;
                        float layerDepth = 1f;
                        Vector2 pos = new Vector2(texturePosX, texturePosY);
                        Vector2 origin = new Vector2(0, 0);

                        batch.Draw(texture, pos, null, tint, rotation, origin, scale, SpriteEffects.None, layerDepth);
                    }
                }
            }
            batch.End();
        }

        /*public void Draw(GameWorld world, GameTime gameTime, int viewPortWidth, int viewPortHeight, int zoomSetting)
        {
            Color tint = Color.White;
            Player p = world.player;

            long playerX = p.X;
            long playerY = p.Y;


            int PIXELS_PER_CHUNK = gameSettings.PIXELS_PER_CHUNK;

            int destWidth = (int)(PIXELS_PER_CHUNK * scale);
            int destHeight = (int)(PIXELS_PER_CHUNK * scale);
            // System.out.println("destHeight: " + destHeight);

            // how many chunks do we need?
            int chunksX = Math.Max(1, (int)(viewPortWidth / (PIXELS_PER_CHUNK * scale))) + 3;
            int chunksY = Math.Max(1, (int)(viewPortHeight / (PIXELS_PER_CHUNK * scale))) + 3;

            Point chunksIndex = world.getChunksIndex(playerX, playerY);
            int chunkIndexX = chunksIndex.X;
            int chunkIndexY = chunksIndex.Y;

            // what chunks to draw
            int chunkY = chunkIndexY - (chunksY / 2);
            int chunkX = chunkIndexX - (chunksX / 2);
            int drawChunkStartX = (int)((0 - (chunksX / 2)) * (PIXELS_PER_CHUNK * scale));
            drawChunkStartX += (viewPortWidth / 2);
            int pixelInChunkX = (int)(playerX % PIXELS_PER_CHUNK);
            if (playerX < 0)
            {
                if (pixelInChunkX == 0)
                {
                    pixelInChunkX = -PIXELS_PER_CHUNK;
                }
                drawChunkStartX -= (int)((PIXELS_PER_CHUNK + pixelInChunkX) * scale);
            }
            else
            {
                drawChunkStartX -= (int)(pixelInChunkX * scale);
            }

            int drawChunkY = (int)((0 - (chunksY / 2)) * (PIXELS_PER_CHUNK * scale));
            drawChunkY += (viewPortHeight / 2);
            int pixelInChunkY = (int)(playerY % PIXELS_PER_CHUNK);
            if (playerY < 0)
            {
                if (pixelInChunkY == 0)
                {
                    pixelInChunkY = -PIXELS_PER_CHUNK;
                }
                drawChunkY -= (int)((PIXELS_PER_CHUNK + pixelInChunkY) * scale);
            }
            else
            {
                drawChunkY -= (int)(pixelInChunkY * scale);
            }

            batch.Begin();
            for (int y = 0; y < chunksY; y++)
            {
                chunkX = chunkIndexX - (chunksX / 2);
                int drawChunkX = drawChunkStartX;
                for (int x = 0; x < chunksX; x++)
                {
                    if (GameWorld.isSeen(drawChunkX, drawChunkY, destWidth, destHeight, viewPortWidth, viewPortHeight, 0, 0))
                    {
                        Location chunkId = new Location(chunkX, chunkY, 0);
                        Chunk chunk = contentLib.GetChunk(chunkId);
                        if ((chunk != null) && (chunk.Texture != null))
                        {

                            float rotation = 0f;
                            float layerDepth = 1f;
                            Vector2 pos = new Vector2(drawChunkX, drawChunkY);
                            Vector2 origin = new Vector2(viewPortWidth / 2, viewPortHeight / 2);
                            batch.Draw(chunk.Texture, pos, null, tint, rotation, origin, scale, SpriteEffects.None, layerDepth);
                        }
                        else
                        {
                            // this tile has not yet been loaded, draw water
                            batch.DrawRectangle(new RectangleF(drawChunkX, drawChunkY, destWidth, destHeight), Color.Black);
                        }
                    }
                    drawChunkX += destWidth;
                    chunkX++;
                }
                drawChunkY += destHeight;
                chunkY++;
            }
            batch.End();
        }*/
    }
}
