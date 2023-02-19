using Google.Protobuf;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.UI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Reflection;
using TileTales.Graphics;

namespace TileTales.GameContent
{
    internal class ContentLibrary
    {
        private static readonly String imageFileType = "png";
        private static readonly String FOLDER_SPRITES = "Content/assets/gfx/sprites";
        private static readonly String FOLDER_TILES = "Content/assets/gfx/tiles";
        private static readonly String FOLDER_MAPS = "Content/assets/gfx/maps";
        private static String NoTile = "000000";
        private static String Water = "0000FF";
        //private readonly Dictionary<int, Dictionary<Point, WeakReference<Chunk>>> _chunkLayers = new Dictionary<int, Dictionary<Point, WeakReference<Chunk>>>();
        private readonly Dictionary<Location, Chunk> _chunks = new Dictionary<Location, Chunk> ();
        private readonly ChunkFactory _chunkFactory;
        //private readonly Dictionary<Point, WeakReference<Chunk>> _chunks = new Dictionary<Point, WeakReference<Chunk>>();
        private Dictionary<string, SKBitmap> tiles = new Dictionary<string, SKBitmap>();
        private Dictionary<string, SKBitmap> sprites = new Dictionary<string, SKBitmap>();
        private Dictionary<string, SKBitmap> maps = new Dictionary<string, SKBitmap>();

        private Settings _gameSettings;

        private GraphicsDevice _graphicsDevice;
        private SKBitmap waterMap;
        private SKColor water;
        private Chunk waterChunk;

        public int MapWidth { get { return _gameSettings.MAP_SIZE; } }
        public int MapHeight { get { return _gameSettings.MAP_SIZE; } }
        public int TileWidth { get { return _gameSettings.TILE_SIZE; } }
        public int TileHeight { get { return _gameSettings.TILE_SIZE; } }
        public int ChunkWidth { get { return _gameSettings.CHUNK_SIZE; } }
        public int ChunkHeight { get { return _gameSettings.CHUNK_SIZE; } }

        public ContentLibrary(GraphicsDevice graphicsDevice)
        {
            this._graphicsDevice = graphicsDevice;
            this._chunkFactory = new ChunkFactory(graphicsDevice, this);
        }
        public void LoadPrepackagedContent()
        {
            sprites = Utils.ContentReader.readTexturesInDirectory(_graphicsDevice, FOLDER_SPRITES);
            //tiles = Utils.ContentReader.readTilesInDirectory(_graphicsDevice, FOLDER_TILES);
            //maps = Utils.ContentReader.readTexturesInDirectory(_graphicsDevice, FOLDER_MAPS);

            //waterChunk = createWaterChunk();

            //TODO temp, remove
            /*TileWidth = 16;
            TileHeight = 16;
            MapWidth = 100;
            MapHeight = 100;*/
        }

        public void CreateWaterChunk()
        {
            SKBitmap waterMap = new SKBitmap(100, 100, true);
            SKColor water = SKColor.Parse(Water);
            for (int y = 0; y < 100; y++)
            {
                for (int x = 0; x < 100; x++)
                {
                    waterMap.SetPixel(x, y, water);
                }
            }
            waterChunk = _chunkFactory.CreateChunkFromMap(waterMap);
        }

        public Texture2D GetSprite(string name)
        {
            SKBitmap sKBitmap = sprites[name];
            Texture2D finalTexture = Texture2D.FromStream(_graphicsDevice, sKBitmap.Encode(SKEncodedImageFormat.Png, 100).AsStream());
            return finalTexture;
        }

        public SKBitmap GetTile(string colorRGB)
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
        
        public SKBitmap GetTile(byte r, byte g, byte b)
        {
            return GetTile(string.Format("{0:X2}{1:X2}{2:X2}", r, g, b));
        }

        internal void AddTile(Tile tile)
        {
            //System.Diagnostics.Debug.WriteLine("AddTile tile.LegacyColor: " + tile.LegacyColor + " (#"+ tile.LegacyColor.ToString("X6") + "), image.width: " + tile.Image.Width);
            // ToDo change LegacyColor to ReplacementColor
            SetTile(tile.LegacyColor, tile.Image);
            /*System.Diagnostics.Debug.WriteLine("tile.Tags " + tile.Tags[0]);
            if (tile.Tags.Contains("isOcean"))
            {
                System.Diagnostics.Debug.WriteLine("Adding water");
                tiles[Water] = tile.Image;
            }*/
        }

        public void SetTile(byte r, byte g, byte b, SKBitmap texture)
        {
            SetTile(string.Format("{0:X2}{1:X2}{2:X2}"), texture);
        }

        public void SetTile(int color, SKBitmap texture)
        {
            SetTile(color.ToString("X6"), texture);
        }

        public void SetTile(string colorRGB, SKBitmap texture)
        {
            tiles[colorRGB] = texture;
        }

        public SKBitmap GetMap(string name)
        {
            return maps[name];
        }

        public void AddSprite(string name, SKBitmap texture, Boolean saveToDisc)
        {
            sprites[name] = texture;
            if (saveToDisc)
            {
                Utils.ContentWriter.WriteFile(FOLDER_SPRITES + "/" + name, texture);
            }
        }

        public void AddMap(string name, ByteString data, Boolean saveToDisc, bool createChunk)
        {
            if (data == null || data.Length == 0 || data == ByteString.Empty)
            {
                return;
            }
            AddMap(name, Utils.ContentReader.textureFromByteString(data, _graphicsDevice), saveToDisc, createChunk);
        }

        public void AddMap(string name, SKBitmap texture, Boolean saveToDisc, bool createChunk)
        {
            maps[name] = texture;
            if (saveToDisc)
            {
                Utils.ContentWriter.WriteFile(FOLDER_MAPS + "/" + name, texture);
            }
            if (createChunk)
            {
                Chunk chunk = _chunkFactory.CreateChunkFromMap(texture);
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
            return waterChunk;
        }

        public void SetChunk(int x, int y, int z, Chunk chunk)
        {
            Location key = new Location(x, y, z);
            SetChunk(key, chunk);
        }

        private void SetChunk(Location key, Chunk chunk)
        {
            _chunks.Add(key, chunk);
        }

        internal void SetGameSettings(Settings gameSettings)
        {
            _gameSettings = gameSettings;
        }
    }
}
