using Net.Tiletales.Network.Proto.Game;
using Net.Tiletales.Network.Proto.Paint;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.GameContent;

namespace TileTales.Network
{
    internal class RequestFactory
    {
        public static RequestFactory Instance { get; } = new RequestFactory();

        private RequestFactory()
        {

        }

        public CenterMapsRequest CreateZoneMapsRequest(int x, int y, int z, int zoomLevel, int size)
        {
            CenterMapsRequest zoneMapsRequest = new()
            {
                CenterX = x,
                CenterY = y,
                Z = z,
                Width = size,
                Height = size,
                ZoomLevel = zoomLevel
            };
            return zoneMapsRequest;
        }

        internal CenterMapsRequest CreateZoneMapsRequest(Point3D mapIndex, int zoomLevel, int size)
        {
            return CreateZoneMapsRequest(mapIndex.X, mapIndex.Y, mapIndex.Z, zoomLevel, size);
        }

        public MapRequest createZoneMapRequest(int x, int y, int z, int zoomLevel)
        {
            MapRequest zoneMapRequest = new()
            {
                X = x,
                Y = y,
                Z = z,
                ZoomLevel = zoomLevel,
                MyVersion = 0
            };
            return zoneMapRequest;
        }

        public MoveRequest createMoveRequest(int deltaX, int deltaY, int deltaZ)
        {
            MoveRequest moveRequest = new()
            {
                DeltaX = deltaX,
                DeltaY = deltaY,
                DeltaZ = deltaZ
            };
            return moveRequest;
        }

        internal AllTilesRequest CreateAllTilesRequest()
        {
            AllTilesRequest request = new()
            {
                MyVersion = 0
            };
            return request;
        }

        internal TeleportRequest createTeleportRequest(int teleportX, int teleportY, int z)
        {
            TeleportRequest teleportRequest = new()
            {
                X = teleportX,
                Y = teleportY,
                Z = z
            };
            return teleportRequest;
        }

        internal DrawLineRequest createDrawLineRequest(int paintStartX, int paintStartY, int paintEndX, int paintEndY, int z)
        {
            DrawLineRequest drawLineRequest = new()
            {
                StartX = paintStartX,
                StartY = paintStartY,
                EndX = paintEndX,
                EndY = paintEndY,
                Z = z
            };
            return drawLineRequest;
        }

        internal DrawMultiTileRequest createDrawMultiTileRequest(HashSet<Point> paintPoints, int z, uint color)
        {
            DrawMultiTileRequest drawMultiTileRequest = new()
            {
            };
            foreach (Point point in paintPoints)
            {
                drawMultiTileRequest.Tiles.Add(new DrawTileRequest
                {
                    X = point.X,
                    Y = point.Y,
                    Z = z,
                    TileId = color
                });
            }
            return drawMultiTileRequest;
        }

        internal BucketFillRequest createBucketFillRequest(int worldX, int worldY, int v, uint legacyColor)
        {
            BucketFillRequest bucketFillRequest = new()
            {
                X = worldX,
                Y = worldY,
                Z = v,
                TileId = legacyColor
            };
            return bucketFillRequest;
        }
    }
 }
