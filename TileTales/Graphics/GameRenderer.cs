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
using Google.Protobuf.WellKnownTypes;
using static System.Net.Mime.MediaTypeNames;
using static System.Formats.Asn1.AsnWriter;

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
        private Texture2D playerAvatar;
        private Texture2D worldMap;
        private Vector2 origin = new Vector2(0, 0);
        const float rotation = 0f;
        const float layerDepth = 1f;

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
            playerAvatar = contentLib.GetSprite("gobbe.png");
            worldMap = contentLib.GetWorldMap();
        }

        /*public void Draw2(GameWorld world, GameTime gameTime)
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            int PIXELS_PER_CHUNK = settings.CHUNK_SIZE;
            Color tint = Color.White;
            Player p = world.player;
            const float rotation = 0f;
            const float layerDepth = 1f;
            int viewPortWidth = settings.WindowWidth;
            int viewPortHeight = settings.WindowHeight;
            float scale = Settings.SCALE_VALUES[settings.ZoomLevel];
            float inverseScale = 1 / scale;

            int chunksX = 6;
            int chunksY = 6;
            Chunk[] chunks = world.GetChunksAroundPlayer(chunksX, chunksY);
            float playerX = p.X * scale;
            float playerY = p.Y * scale;
            int centerX = (int)Math.Round(viewPortWidth / 2f);
            int centerY = (int)Math.Round(viewPortHeight / 2f);
            int pixelInChunkX = ((int)((p.X % PIXELS_PER_CHUNK)));
            int pixelInChunkY = ((int)((p.Y % PIXELS_PER_CHUNK)));

            //System.Diagnostics.Debug.WriteLine("pixelInChunkX:" + pixelInChunkX + " pixelInChunkY:" + pixelInChunkY);

            SamplerState samplerState = (scale >= 1) ? SamplerState.PointClamp : SamplerState.AnisotropicClamp;
            batch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, samplerState, DepthStencilState.Default, null, null, null);
            for (int y = 0; y < chunksY; y++)
            {
                for (int x = 0; x < chunksX; x++)
                {
                    int index = x + y * chunksX;
                    Chunk chunk = chunks[index];
                    if (chunk != null)
                    {
                        Texture2D texture = chunk.Image;
                        int textWidth = (int)(texture.Width * scale);
                        int textHeight = (int)(texture.Height * scale);
                        int texturePosX = (int)Math.Floor(centerX - textWidth * chunksX / 2f + x * textWidth - playerX);
                        int texturePosY = (int)Math.Floor(centerY - textHeight * chunksY / 2f + y * textHeight - playerY);

                        Vector2 pos = new Vector2(texturePosX, texturePosY);
                        Vector2 origin = new Vector2(0, 0);

                        batch.Draw(texture, pos, null, tint, rotation, origin, scale, SpriteEffects.None, layerDepth);
                    }
                }
            }
            batch.End();
        }*/

        public void Draw(GameWorld world, GameTime gameTime)
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            float scale = Settings.SCALE_VALUES[settings.ZoomLevel];
            //SamplerState samplerState = (scale >= 1) ? SamplerState.PointClamp : SamplerState.AnisotropicClamp;
            SamplerState samplerState = SamplerState.PointClamp;
            batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, samplerState, DepthStencilState.Default, null, null, null);

            DrawWorldBackground(world, gameTime);
            DrawTiles(world, gameTime);

            batch.End();

        }

        public void DrawWorldBackground(GameWorld world, GameTime gameTime)
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            Color tint = Color.White;
            float viewWidth = settings.WindowWidth;
            float viewHeight = settings.WindowHeight;
            float scale = Settings.WORLDMAP_SCALE_VALUES[settings.ZoomLevel];
            Vector2 pos = new Vector2(-2048 * scale + viewWidth / 2, -2048 * scale + viewHeight / 2);
            //batch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, null, DepthStencilState.Default, null, null, null);

            batch.Draw(worldMap, pos, null, tint, rotation, origin, scale, SpriteEffects.None, layerDepth);

            //batch.End();

        }

        public void DrawTiles(GameWorld world, GameTime gameTime)
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            float pxPerTile = settings.TILE_SIZE;
            float pxPerTileHlf = pxPerTile / 2;
            float pxPerChunk = settings.CHUNK_SIZE;
            Color tint = Color.White;
            Player p = world.player;
            float playerX = p.X;
            float playerY = p.Y;
            float viewWidth = settings.WindowWidth;
            float viewHeight = settings.WindowHeight;
            float scale = Settings.SCALE_VALUES[settings.ZoomLevel];
            float centerX = viewWidth / 2f;
            float centerY = viewHeight / 2f;
            float txtOffsetX = playerX % pxPerChunk;
            float txtOffsetY = playerY % pxPerChunk;
            if (playerX < 0 && txtOffsetX != 0) {
                txtOffsetX = pxPerChunk + playerX % pxPerChunk;
            }
            if (playerY < 0 && txtOffsetY != 0)
            {
                txtOffsetY = pxPerChunk + playerY % pxPerChunk;
            }
            txtOffsetX += pxPerTileHlf;
            txtOffsetY += pxPerTileHlf;

            // How many chunks are shown on screen?
            float pxSeenX = viewWidth / scale;
            float pxSeenY = viewHeight / scale;
            // How many chunks needed? Rounded to nearest larger even number
            double chunkAmountX = Math.Round(Math.Ceiling(pxSeenX / pxPerChunk) / 2, MidpointRounding.AwayFromZero) * 2;
            double chunkAmountY = Math.Round(Math.Ceiling(pxSeenY / pxPerChunk) / 2, MidpointRounding.AwayFromZero) * 2;

            Point chunksIndex = world.getChunksIndex(p.X, p.Y);
            double yMax = chunksIndex.Y + chunkAmountY / 2f + 1;
            double screenIndexY = -chunkAmountY / 2f;
            for (int y = (int)(chunksIndex.Y - chunkAmountY / 2f); y < yMax; y++, screenIndexY++)
            {
                double screenIndexX = -chunkAmountX / 2f;
                double xMax = chunksIndex.X + chunkAmountX / 2f + 1;
                for (int x = (int)(chunksIndex.X - chunkAmountX / 2f); x < xMax; x++, screenIndexX++)
                {
                    Chunk chunk = world.GetChunk(x,y);
                    if (chunk == null || chunk.Image == null) continue;
                    if (settings.ZoomLevel >= 8)
                    {
                        float textureX = (float)(centerX - (txtOffsetX - screenIndexX * pxPerChunk) * scale);
                        float textureY = (float)(centerY - (txtOffsetY - screenIndexY * pxPerChunk) * scale);
                        Vector2 pos = new Vector2(textureX, textureY);
                        batch.Draw(chunk.Image, pos, null, tint, rotation, origin, scale, SpriteEffects.None, layerDepth);
                    } else
                    {
                        int textureX = (int)(centerX - (txtOffsetX - screenIndexX * pxPerChunk) * scale);
                        int textureY = (int)(centerY - (txtOffsetY - screenIndexY * pxPerChunk) * scale);
                        Vector2 pos = new Vector2(textureX, textureY);
                        batch.Draw(chunk.Image, pos, null, tint, rotation, origin, scale, SpriteEffects.None, layerDepth);
                    }
                }
            }
            
            Vector2 centerPos = new Vector2(centerX - (pxPerTileHlf * scale), centerY - (pxPerTileHlf * scale));
            batch.Draw(playerAvatar, centerPos, null, tint, rotation, origin, scale, SpriteEffects.None, layerDepth);

            //batch.End();
        }

            /*public void Draw(GameWorld world, GameTime gameTime)
            {
                Settings settings = game.GameSettings;
                if (settings == null) return;
                Color tint = Color.White;
                Player p = world.player;
                long playerX = p.X;
                long playerY = p.Y;
                int cnvWidth = settings.WindowWidth;
                int cnvHeight = settings.WindowHeight;

                int PIXELS_PER_CHUNK = settings.CHUNK_SIZE;
                double zFactor = Settings.SCALE_VALUES[settings.ZoomLevel];
                int destWidth = ((int)((PIXELS_PER_CHUNK * zFactor)));
                int destHeight = ((int)((PIXELS_PER_CHUNK * zFactor)));
                //  System.out.println("destHeight: " + destHeight);
                //  how many chunks do we need?
                int chunksX = (Math.Max(1, ((int)((cnvWidth
                                / (PIXELS_PER_CHUNK * zFactor))))) + 3);
                int chunksY = (Math.Max(1, ((int)((cnvHeight
                                / (PIXELS_PER_CHUNK * zFactor))))) + 3);
                int playerD = p.Z;
                Point chunksIndex = world.getChunksIndex(playerX, playerY);
                int chunkIndexX = chunksIndex.X;
                int chunkIndexY = chunksIndex.Y;
                //  what chunks to draw
                int chunkY = (chunkIndexY
                            - (chunksY / 2));
                int chunkX = (chunkIndexX
                            - (chunksX / 2));
                int drawChunkStartX = ((int)(((0
                            - (chunksX / 2))
                            * (PIXELS_PER_CHUNK * zFactor))));
                drawChunkStartX = (drawChunkStartX
                            + (cnvWidth / 2));
                int pixelInChunkX = ((int)((playerX % PIXELS_PER_CHUNK)));
                if ((playerX < 0))
                {
                    if ((pixelInChunkX == 0))
                    {
                        pixelInChunkX = (PIXELS_PER_CHUNK * -1);
                    }

                    drawChunkStartX = ((int)(drawChunkStartX
                                - ((PIXELS_PER_CHUNK + pixelInChunkX)
                                * zFactor)));
                }
                else
                {
                    drawChunkStartX = ((int)(drawChunkStartX
                                - (pixelInChunkX * zFactor)));
                }

                int drawChunkY = ((int)(((0
                            - (chunksY / 2))
                            * (PIXELS_PER_CHUNK * zFactor))));
                drawChunkY = (drawChunkY
                            + (cnvHeight / 2));
                int pixelInChunkY = ((int)((playerY % PIXELS_PER_CHUNK)));
                if ((playerY < 0))
                {
                    if ((pixelInChunkY == 0))
                    {
                        pixelInChunkY = (PIXELS_PER_CHUNK * -1);
                    }

                    drawChunkY = ((int)(drawChunkY
                                - ((PIXELS_PER_CHUNK + pixelInChunkY)
                                * zFactor)));
                }
                else
                {
                    drawChunkY = ((int)(drawChunkY
                                - (pixelInChunkY * zFactor)));
                }

                // int zSetting = mGameWorld.getZoomSetting();
                SamplerState samplerState = (zFactor >= 1) ? SamplerState.PointClamp : SamplerState.AnisotropicClamp;
                batch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, samplerState, DepthStencilState.Default, null, null, null);
                for (int y = 0; (y < chunksY); y++)
                {
                    chunkX = (chunkIndexX
                                - (chunksX / 2));
                    int drawChunkX = drawChunkStartX;
                    for (int x = 0; (x < chunksX); x++)
                    {
                        if (GameWorld.isSeen(drawChunkX, drawChunkY, destWidth, destHeight, cnvWidth, cnvHeight, 0, 0))
                        {
                            Chunk chunk = world.GetChunk(chunkX, chunkY);
                            if (((chunk != null)
                                        && (chunk.Texture != null)))
                            {
                                //mWorldRenderer.draw(g, zSetting, chunk.image, drawChunkX, drawChunkY, (drawChunkX + destWidth), (drawChunkY + destHeight), 0, 0, PIXELS_PER_CHUNK, PIXELS_PER_CHUNK, true);
                                //g.drawImage(image, x, y, endX, endY, srcX, srcY, srcEndX, srcEndY);

                                const float rotation = 0f;
                                const float layerDepth = 1f;
                                Vector2 pos = new Vector2(drawChunkX, drawChunkY);
                                Vector2 origin = new Vector2(0, 0);
                                Vector2 scale = new Vector2((float)zFactor, (float)zFactor);
                                batch.Draw(chunk.Texture, pos, null, tint, rotation, origin, scale, SpriteEffects.None, layerDepth);
                            }

                        }

                        drawChunkX = (drawChunkX + destWidth);
                        chunkX++;
                    }

                    drawChunkY = (drawChunkY + destHeight);
                    chunkY++;
                }

                batch.End();
            }*/
        }
}
