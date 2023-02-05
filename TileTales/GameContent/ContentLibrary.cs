using Google.Protobuf;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TileTales.GameContent
{
    internal class ContentLibrary
    {
        private static readonly String imageFileType = "png";
        private static readonly String FOLDER_SPRITES = "Content/assets/gfx/sprites";
        private static readonly String FOLDER_TILES = "Content/assets/gfx/tiles";
        private static readonly String FOLDER_MAPS = "Content/assets/gfx/maps";
        //private readonly Dictionary<int, Dictionary<Point, WeakReference<Chunk>>> _chunkLayers = new Dictionary<int, Dictionary<Point, WeakReference<Chunk>>>();
        private readonly Dictionary<Location, Chunk> _chunks = new Dictionary<Location, Chunk> ();
        private readonly ChunkFactory _chunkFactory;
        //private readonly Dictionary<Point, WeakReference<Chunk>> _chunks = new Dictionary<Point, WeakReference<Chunk>>();
        private Dictionary<string, Texture2D> tiles;
        private Dictionary<string, Texture2D> sprites;
        private Dictionary<string, Texture2D> maps;

        private GraphicsDevice _graphicsDevice;

        private String NoTile = "000000";
        private String Water = "0000FF";

        public int ChunkWidth { get; private set; }
        public int ChunkHeight { get; private set; }

        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }

        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        public ContentLibrary(GraphicsDevice graphicsDevice)
        {
            this._graphicsDevice = graphicsDevice;
            this._chunkFactory = new ChunkFactory(graphicsDevice, this);
        }
        public void LoadPrepackagedContent()
        {
            sprites = Utils.ContentReader.readTexturesInDirectory(_graphicsDevice, FOLDER_SPRITES);
            tiles = Utils.ContentReader.readTilesInDirectory(_graphicsDevice, FOLDER_TILES);
            maps = Utils.ContentReader.readTexturesInDirectory(_graphicsDevice, FOLDER_MAPS);

            //TODO temp, remove
            TileWidth = 16;
            TileHeight = 16;
        }

        public Texture2D GetSprite(string name)
        {
            return sprites[name];
        }

        public Texture2D GetTile(string colorRGB)
        {
            if (colorRGB == NoTile)
            {
                return tiles[Water];
            }
            if (!tiles.ContainsKey(colorRGB))
            {
                return tiles[Water];
            }
            return tiles[colorRGB];
        }
        
        public Texture2D GetTile(byte r, byte g, byte b)
        {
            return GetTile(string.Format("{0:X2}{1:X2}{2:X2}", r, g, b));
        }

        public void SetTile(byte r, byte g, byte b, Texture2D texture)
        {
            SetTile(string.Format("{0:X2}{1:X2}{2:X2}"), texture);
        }

        public void SetTile(string colorRGB, Texture2D texture)
        {
            tiles[colorRGB] = texture;
            TileWidth = texture.Width;
            TileHeight = texture.Height;
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

        public void AddMap(string name, ByteString data, Boolean saveToDisc, bool createChunk)
        {
            AddMap(name, Utils.ContentReader.textureFromByteString(data, _graphicsDevice), saveToDisc, createChunk);
        }

        public void AddMap(string name, Texture2D texture, Boolean saveToDisc, bool createChunk)
        {
            maps[name] = texture;
            MapWidth = texture.Width;
            MapHeight = texture.Height;
            if (saveToDisc)
            {
                Utils.ContentWriter.WriteFile(FOLDER_MAPS + "/" + name, texture);
            }
            if (createChunk)
            {
                Chunk chunk = _chunkFactory.CreateChunkFromTexture(texture);
                Location location = createLocationFromMapName(name);
                SetChunk(location, chunk);
            }
        }

        private Location createLocationFromMapName(string name)
        {
            // Texture name is in format: x_y_z.png
            int _idx = name.IndexOf("_");
            int _lidx = name.LastIndexOf("_");
            int x = int.Parse(name.Substring(0, _idx));
            int y = int.Parse(name.Substring(_idx + 1, _lidx - _idx - 1));
            int z = int.Parse(name.Substring(_lidx + 1, name.LastIndexOf(".") - _lidx - 1));
            return new Location(x, y, z);
        }

        public static String CreateMapName(int x, int y, int z, int zoomLevel)
        {
            return string.Format("{0}_{1}_{2}.{3}", x, y, z, imageFileType);
            //return x + "_" + y + "_" + z + ".png";
        }
        
        public Chunk GetChunk(int x, int y, int z)
        {
            return GetChunk(new Location(x, y, z));
        }

        public Chunk GetChunk(Location key)
        {
            if (_chunks.ContainsKey(key))
            {
                return _chunks[key];
            }
            return null;
        }

        public void SetChunk(int x, int y, int z, Chunk chunk)
        {
            Location key = new Location(x, y, z);
            SetChunk(key, chunk);
        }

        private void SetChunk(Location key, Chunk chunk)
        {
            ChunkWidth = chunk.Texture.Width;
            ChunkHeight = chunk.Texture.Height;
            _chunks.Add(key, chunk);
        }
    }
}
