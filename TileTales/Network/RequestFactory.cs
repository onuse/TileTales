using Net.Tiletales.Network.Proto.Game;
using Net.Tiletales.Network.Proto.Paint;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CenterMapsRequest zoneMapsRequest = new CenterMapsRequest
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

        public MapRequest createZoneMapRequest(int x, int y, int z, int zoomLevel)
        {
            MapRequest zoneMapRequest = new MapRequest
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
            MoveRequest moveRequest = new MoveRequest
            {
                DeltaX = deltaX,
                DeltaY = deltaY,
                DeltaZ = deltaZ
            };
            return moveRequest;
        }

        internal AllTilesRequest CreateAllTilesRequest()
        {
            AllTilesRequest request = new AllTilesRequest
            {
                MyVersion = 0
            };
            return request;
        }

        internal TeleportRequest createTeleportRequest(int teleportX, int teleportY, int z)
        {
            TeleportRequest teleportRequest = new TeleportRequest
            {
                X = teleportX,
                Y = teleportY,
                Z = z
            };
            return teleportRequest;
        }

        internal DrawLineRequest createDrawLineRequest(int paintStartX, int paintStartY, int paintEndX, int paintEndY, int z)
        {
            DrawLineRequest drawLineRequest = new DrawLineRequest
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
            DrawMultiTileRequest drawMultiTileRequest = new DrawMultiTileRequest
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
    }
 }
