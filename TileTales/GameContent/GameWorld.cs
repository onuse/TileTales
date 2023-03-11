using Microsoft.Xna.Framework;
using Net.Tiletales.Network.Proto.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.Graphics;
using TileTales.Utils;

namespace TileTales.GameContent
{
    internal class GameWorld
    {
        public readonly Player player = new Player();
        private readonly TileTalesGame game;
        private readonly ContentLibrary contentLibrary;

        public GameWorld(TileTalesGame tileTalesGame)
        {
            this.game = tileTalesGame;
            this.contentLibrary = game.ContentLibrary;
        }

        public Point3D LastMapFetchLocation { get; set; }

        public void LoadContent()
        {
            // Load all the chunks around the player
            // Load all the sprites around the player
        }

        public void Update(GameTime gameTime)
        {
            // Update all the chunks around the player
            // Update all the sprites around the player
        }

        public Chunk[] GetChunksAroundPlayer(int xAmount, int yAmount)
        {
            // Get the chunks around the player

            //int chunkWidth = contentLibrary.ChunkWidth;
            //int chunkHeight = contentLibrary.ChunkHeight;

            CoordinateTranslator.getMapIndexForWorldLocation(player.X, player.Y, contentLibrary, out int PlayerMapX, out int PlayerMapY);
            Chunk[] chunks = new Chunk[xAmount * yAmount];
            for (int y = 0; y < yAmount; y++)
            {
                for (int x = 0; x < xAmount; x++)
                {
                    int mapX = PlayerMapX + x - xAmount / 2;
                    int mapY = PlayerMapY + y - yAmount / 2;
                    int index = x + y * xAmount;
                    chunks[index] = contentLibrary.GetChunk(mapX, mapY, player.Z);
                }
            }
            return chunks;
        }

        internal void MovePlayer(int x, int y)
        {
            player.Move(x, y, 0);
        }

        internal void TeleportPlayer(int x, int y)
        {
            player.Teleport(x, y, 0);
        }

        internal Point getMapIndex(int locationX, int locationY)
        {
            CoordinateTranslator.getMapIndexForWorldLocation(locationX, locationY, contentLibrary, out int mapIndexX, out int mapIndexY);
            return new Point(mapIndexX, mapIndexY);
        }
        
        internal static bool isSeen(int drawChunkX, int drawChunkY, int destWidth, int destHeight, int viewPortWidth, int viewPortHeight, int v1, int v2)
        {
            return true;
        }

        internal Player createPlayerObject(PlayerObjectInfo playerObjectInfo)
        {
            player.init(playerObjectInfo);
            return player;
        }

        internal Player GetPlayer()
        {
            return player;
        }

        internal Chunk GetChunk(int chunkX, int chunkY)
        {
            return contentLibrary.GetChunk(chunkX, chunkY, player.Z);
        }

        internal void ScreenToWorldX(int screenX, int screenY, out int worldX, out int worldY)
        {
            //System.Diagnostics.Debug.WriteLine("GameWorld screenX: " + screenX + " screenY: " + screenY);
            CoordinateTranslator.ScreenToWorldCoords(screenX, screenY, contentLibrary, player, out worldX, out worldY);
        }
    }
}
