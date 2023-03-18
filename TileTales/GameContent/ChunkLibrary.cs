﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.Graphics;
using TileTales.Utils;

namespace TileTales.GameContent
{
    /**
     * This class acts as a cache with caching rules based on map locations in relation to where the player is and will be.
     * Only the 4 chunks closest to the player should be loaded with highest resolution.
     */
    internal class ChunkLibrary
    {
        private static readonly float s_fullResThreshold = 2.0f;
        private static readonly float s_quarterResThreshold = 8.0f;
        private static readonly float s_purgeThreshold = 16.0f;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly ContentLibrary _contentLibrary;
        private readonly ChunkFactory _chunkFactory;
        private readonly ConcurrentDictionary<Point3D, Chunk> _chunks = new();

        internal ChunkLibrary(GraphicsDevice graphicsDevice, ContentLibrary contentLibrary)
        {
            _graphicsDevice = graphicsDevice;
            _contentLibrary = contentLibrary;
            _chunkFactory = new ChunkFactory(graphicsDevice, contentLibrary);
        }

        internal void UpdateLibrary(Point3D relevantWorldCoord)
        {
            // Locations are in WorldCoordinates
            // A typical world coordinate would be the player position
            // This also means that all other locations are way less relevant
            List<Point3D> chunksToRemove = new();
            foreach (var chunkIndex in _chunks)
            {
                Chunk chunk = chunkIndex.Value;
                if (chunk == null) continue;
                Point3D mapLoc = chunk.Map.Location;
                float dist = CoordinateHelper.GetDistanceInMapsForWorldCoords(relevantWorldCoord, mapLoc, _contentLibrary);

                if (dist <= s_fullResThreshold)
                {
                    if (chunk.FullResolution == null)
                    {
                        //chunk.FullResolution = _chunkFactory.ChunkDataToTexture(chunk, 1f);
                        Parallel.Invoke(() => SetFullResolutionOnChunk(chunk));
                    }
                }
                else if (dist <  s_quarterResThreshold)
                {
                    chunk.FullResolution = null;
                    if (chunk.QuarterResolution == null)
                    {
                        chunk.QuarterResolution = _chunkFactory.ChunkDataToTexture(chunk, 0.25f);
                    }
                }
                else if (dist > s_purgeThreshold)
                {
                    chunk.FullResolution = null;
                    chunk.QuarterResolution = null;
                    chunksToRemove.Add(mapLoc);
                    //_chunks[mapLoc] = null;
                }
            }

            foreach (var point in chunksToRemove)
            {
                _chunks[point] = null;
            }
        }

        private void SetFullResolutionOnChunk(Chunk chunk)
        {
            chunk.FullResolution = _chunkFactory.ChunkDataToTexture(chunk, 1f);
        }

        internal void SetChunk(Point3D key, Chunk chunk)
        {
            _chunks[key] = chunk;
        }

        internal Chunk GetChunk(Point3D key)
        {
            if (_chunks.ContainsKey(key))
            {
                /*Chunk chunk = _chunks[key];
                if (chunk.Image == null)
                {
                    _chunkFactory.CreateTextureForChunk(chunk);
                }*/
                return _chunks[key];
            }
            /*if (hasMap(key))
            {
                SKBitmap map = GetMap(CreateMapName(key.X, key.Y, key.Z, 0));
                if (map == null)
                {
                    return null;
                }
                Chunk chunk = _chunkFactory.CreateChunkFromMap(map);
                if (chunk == null)
                {
                    return null;
                }
                SetChunk(key, chunk);
                return chunk;
            }*/
            //return waterChunk;
            return null;
        }

        internal void NewMap(Map map)
        {

            Chunk chunk = _chunkFactory.CreateChunkFromMap(map, 0.25f);
            if (chunk != null)
            {
                SetChunk(map.Location, chunk);
            }
        }
    }
}