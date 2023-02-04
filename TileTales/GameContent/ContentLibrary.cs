using Google.Protobuf;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.GameContent
{
    internal class ContentLibrary
    {
        private static readonly String FOLDER_SPRITES = "Content/assets/gfx/sprites";
        private static readonly String FOLDER_TILES = "Content/assets/gfx/tiles";
        private static readonly String FOLDER_MAPS = "Content/assets/gfx/maps";
        private Dictionary<string, Texture2D> tiles;
        private Dictionary<string, Texture2D> sprites;
        private Dictionary<string, Texture2D> maps;

        private GraphicsDevice _graphicsDevice;
        public ContentLibrary(GraphicsDevice graphicsDevice)
        {
            this._graphicsDevice = graphicsDevice;
        }
        public void LoadPrepackagedContent()
        {
            sprites = Utils.ContentReader.readTexturesInDirectory(_graphicsDevice, FOLDER_SPRITES);
            tiles = Utils.ContentReader.readTexturesInDirectory(_graphicsDevice, FOLDER_TILES);
            maps = Utils.ContentReader.readTexturesInDirectory(_graphicsDevice, FOLDER_MAPS);
        }

        public Texture2D GetSprite(string name)
        {
            return sprites[name];
        }

        public Texture2D GetTile(string name)
        {
            return tiles[name];
        }

        public Texture2D GetMap(string name)
        {
            return maps[name];
        }

        public void AddSprite(string name, Texture2D texture, Boolean saveToDisc)
        {
            sprites[name] = texture;
            if (saveToDisc)
            {
                Utils.ContentWriter.WriteFile(FOLDER_SPRITES + "/" + name, texture);
            }
        }

        public void AddTexture(string name, ByteString data, Boolean saveToDisc)
        {
            AddTexture(name, Utils.ContentReader.textureFromByteString(data, _graphicsDevice), saveToDisc);
        }

        public void AddTexture(string name, Texture2D texture, Boolean saveToDisc)
        {
            maps[name] = texture;
            if (saveToDisc)
            {
                Utils.ContentWriter.WriteFile(FOLDER_MAPS + "/" + name, texture);
            }
        }

        public static String CreateMapName(int x, int y, int z, int zoomLevel)
        {
            return x + "_" + y + "_" + z + ".png";
        }
    }
}
