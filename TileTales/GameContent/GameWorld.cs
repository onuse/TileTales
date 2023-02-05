using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            CoordinateTranslator.getMapIndexForTileIndex(player.X, player.Y, contentLibrary, out int PlayerMapX, out int PlayerMapY);
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
    }
}
