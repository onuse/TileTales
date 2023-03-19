using Google.Protobuf;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Timers;
using Myra.Graphics2D.UI;
using SkiaSharp;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TileTales.Graphics;
using TileTales.Utils;

namespace TileTales.GameContent
{
    internal class ContentLibrary
    {
        private static readonly String imageFileType = "png";
        private static readonly String FOLDER_SPRITES = "Content/assets/gfx/sprites";
        private static readonly String FOLDER_TILES = "Content/assets/gfx/tiles";
        private static readonly String FOLDER_MAPS = "Content/assets/gfx/maps";
        private static readonly String WORLD_MAP = "Content/assets/gfx/worldmap.png";
        private static readonly String NoTile = "000000";
        private static readonly String Water = "0000FF";
        //private readonly Dictionary<int, Dictionary<Point, WeakReference<Chunk>>> _chunkLayers = new Dictionary<int, Dictionary<Point, WeakReference<Chunk>>>();
        //private readonly Dictionary<Point, WeakReference<Chunk>> _chunks = new Dictionary<Point, WeakReference<Chunk>>();
        //private Dictionary<string, SKBitmap> tiles = new Dictionary<string, SKBitmap>();
        private readonly Dictionary<string, Tile> tiles = new();
        private Dictionary<string, SKBitmap> sprites = new();
        private readonly ConcurrentDictionary<Point3D, Map> maps = new();
        private readonly ChunkLibrary _chunkLibrary;

        private readonly GraphicsDevice _graphicsDevice;
        private readonly SKBitmap waterMap;
        private readonly SKColor water;
        private readonly Chunk waterChunk;

        private Texture2D worldMap;

        internal int MapWidth { get { return GameSettings.MapSize; } }
        internal int MapHeight { get { return GameSettings.MapSize; } }
        internal int TileWidth { get { return GameSettings.TileSize; } }
        internal int TileHeight { get { return GameSettings.TileSize; } }
        internal int ChunkWidth { get { return GameSettings.ChunkSize; } }
        internal int ChunkHeight { get { return GameSettings.ChunkSize; } }
        internal int WorldWidth { get { return GameSettings.WorldSize; } }
        internal int WorldHeight { get { return GameSettings.WorldSize; } }

        internal Settings GameSettings { get; set; }

        internal ContentLibrary(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _chunkLibrary = new ChunkLibrary(graphicsDevice, this);
        }
        internal void LoadPrepackagedContent()
        {
            worldMap = Utils.ContentReader.readTexture(_graphicsDevice, "Content/assets/gfx/worldmap.png");
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

        internal static void CreateWaterChunk()
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
            //waterChunk = _chunkFactory.CreateChunkFromMap(waterMap, 0.25f);
        }

        internal Texture2D GetSprite(string name)
        {
            SKBitmap sKBitmap = sprites[name];
            Texture2D finalTexture = Texture2D.FromStream(_graphicsDevice, sKBitmap.Encode(SKEncodedImageFormat.Png, 100).AsStream());
            return finalTexture;
        }

        internal Tile GetTile(string colorRGB)
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

        internal Tile GetTile(byte r, byte g, byte b)
        {
            return GetTile(string.Format("{0:X2}{1:X2}{2:X2}", r, g, b));
        }

        /*public SKBitmap GetTile(string colorRGB)
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
        }*/

        /*public SKBitmap GetTile(byte r, byte g, byte b)
        {
            return GetTile(string.Format("{0:X2}{1:X2}{2:X2}", r, g, b));
        }*/

        internal void AddTile(Tile tile)
        {
            //System.Diagnostics.Debug.WriteLine("AddTile tile.LegacyColor: " + tile.LegacyColor + " (#"+ tile.LegacyColor.ToString("X6") + "), image.width: " + tile.Image.Width);
            // ToDo change LegacyColor to ReplacementColor
            //SetTile(tile.LegacyColor, tile.Image);
            tiles[tile.LegacyColorAsString] = tile;
            /*System.Diagnostics.Debug.WriteLine("tile.Tags " + tile.Tags[0]);
            if (tile.Tags.Contains("isOcean"))
            {
                System.Diagnostics.Debug.WriteLine("Adding water");
                tiles[Water] = tile.Image;
            }*/
        }

        /*public void SetTile(byte r, byte g, byte b, SKBitmap texture)
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
        }*/

        internal Map GetMap(Point3D loc)
        {
            if (!maps.ContainsKey(loc))
            {
                return null;
            }
            return maps[loc];
        }

        internal void AddSprite(string name, SKBitmap texture, Boolean saveToDisc)
        {
            sprites[name] = texture;
            if (saveToDisc)
            {
                Utils.ContentWriter.WriteFile(FOLDER_SPRITES + "/" + name, texture);
            }
        }


        internal void AddMap(Map map)
        {
            if (map.ByteString == null || map.ByteString.Length == 0 || map.ByteString == ByteString.Empty)
            {
                return;
            }
            Map currentMap = GetMap(map.Location);
            if (currentMap != null)
            {
                if (currentMap.Version == map.Version)
                {
                    Log.Verbose("Map " + map + " already exists, skipping");
                    return;
                }
                Log.Verbose("Map " + map + " already exists, but is outdated, updating");
            }
            Log.Verbose("Adding map " + map);
            map.Image = Utils.ContentReader.bitmapFromByteString(map.ByteString);
            map.Texture = Utils.ContentReader.textureFromByteString(_graphicsDevice, map.ByteString);
            maps[map.Location] = map;
            Task.Run(() => _chunkLibrary.NewMap(map));
        }
        internal void UpdateCaches(Player player)
        {
            _chunkLibrary.UpdateLibrary(player.Location);
        }

        internal Chunk GetChunk(int x, int y, int z)
        {
            return _chunkLibrary.GetChunk(new Point3D(x, y, z));
        }

        internal void SetChunk(int x, int y, int z, Chunk chunk)
        {
            if (chunk == null)
            {
                return;
            }
            Point3D key = new Point3D(x, y, z);
            _chunkLibrary.SetChunk(key, chunk);
        }

        internal Texture2D GetWorldMap()
        {
            return worldMap;
        }

        internal List<Tile> GetAllTiles()
        {
            return tiles.Values.ToList();
        }

        internal float GetScale()
        {
            return GameSettings.Scale;
        }

        internal bool HasMap(Point3D loc)
        {
            return maps.ContainsKey(loc);
        }
    }
}
