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
        public readonly Player Player = new Player();
        private readonly TileTalesGame game;
        private readonly ContentLibrary contentLibrary;
        private List<Sprite> visibleSprites = new List<Sprite>();

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

            //contentLibrary.Update(gameTime, player);
        }

        public Chunk[] GetChunksAroundPlayer(int xAmount, int yAmount)
        {
            // Get the chunks around the player

            //int chunkWidth = contentLibrary.ChunkWidth;
            //int chunkHeight = contentLibrary.ChunkHeight;

            CoordinateHelper.GetMapIndexForWorldLocation(Player.X, Player.Y, contentLibrary, out int PlayerMapX, out int PlayerMapY);
            Chunk[] chunks = new Chunk[xAmount * yAmount];
            for (int y = 0; y < yAmount; y++)
            {
                for (int x = 0; x < xAmount; x++)
                {
                    int mapX = PlayerMapX + x - xAmount / 2;
                    int mapY = PlayerMapY + y - yAmount / 2;
                    int index = x + y * xAmount;
                    chunks[index] = contentLibrary.GetChunk(mapX, mapY, Player.Z);
                }
            }
            return chunks;
        }

        internal void MovePlayer(int x, int y)
        {
            Player.Move(x, y, 0);
        }

        internal void TeleportPlayer(int x, int y)
        {
            Player.Teleport(x, y, 0);
        }

        internal Point getMapIndex(int locationX, int locationY)
        {
            CoordinateHelper.GetMapIndexForWorldLocation(locationX, locationY, contentLibrary, out int mapIndexX, out int mapIndexY);
            return new Point(mapIndexX, mapIndexY);
        }
        
        internal static bool isSeen(int drawChunkX, int drawChunkY, int destWidth, int destHeight, int viewPortWidth, int viewPortHeight, int v1, int v2)
        {
            return true;
        }

        internal Player createPlayerObject(PlayerObjectInfo playerObjectInfo)
        {
            Player.Init(playerObjectInfo, contentLibrary);
            return Player;
        }

        internal Chunk GetChunk(int chunkX, int chunkY)
        {
            return contentLibrary.GetChunk(chunkX, chunkY, Player.Z);
        }

        internal void ScreenToWorldX(int screenX, int screenY, out int worldX, out int worldY)
        {
            //System.Diagnostics.Debug.WriteLine("GameWorld screenX: " + screenX + " screenY: " + screenY);
            CoordinateHelper.ScreenToWorldCoords(screenX, screenY, contentLibrary, Player, out worldX, out worldY);
        }

        internal List<Sprite> getAllSpritesInMap(Map map)
        {
            return visibleSprites;
        }
    }
}
