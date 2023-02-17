using Net.Tiletales.Network.Proto.Game;
using System;
using System.Collections.Generic;
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

        public MapsRequest CreateZoneMapsRequest(int x, int y, int z, int zoomLevel)
        {
            MapsRequest zoneMapsRequest = new MapsRequest
            {
                CenterX = x,
                CenterY = y,
                Z = z,
                Width = 3,
                Height = 3,
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
    }
 }
