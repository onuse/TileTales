using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Net.Tiletales.Network.Proto.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TileTales.GameContent;
using TileTales.Graphics;
using TileTales.Utils;
using TileTales.Network;
using MonoGame.Extended.Timers;

namespace TileTales.State
{
    internal class RunningState : BaseState
    {
        private static RunningState _instance;
        public static RunningState Singleton
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RunningState();
                }
                return _instance;
            }
        }

        private RunningState() : base()
        {
            Settings settings = game.GameSettings;
            eventBus.Subscribe(ObjectLocationUpdate.Descriptor, (o) => {
                Log.Debug("(ObjectLocationUpdate)");
                ObjectLocationUpdate response = ObjectLocationUpdate.Parser.ParseFrom((o as Any).Value);
                if (response.ObjectId == Player.ObjectId)
                {
                    Point3D newLocation = new Point3D(response.X, response.Y, response.Z);
                    Player.Teleport(newLocation);
                    GameWorld world = game.GameWorld;
                    float mapDistances = CoordinateHelper.GetDistanceInMapsForWorldCoords(world.LastMapFetchLocation, newLocation, content);
                    if (mapDistances > 1)
                    {
                        SendMapsRequest();
                    }
                }
            });

            eventBus.Subscribe(MapInfo.Descriptor, (o) => {
                Log.Debug("(MapInfo)");
                MapInfo response = MapInfo.Parser.ParseFrom((o as Any).Value);
                Task.Run(() => LoadMap(response));
                //Parallel.Invoke(() => LoadMap(response));
                //LoadMap(response);
                //new Thread(sendDelayedMapsRequest).Start();
                /*Task.Run(() =>
                {
                    LoadMap(response);
                });*/
            });

            eventBus.Subscribe(MultiMapInfo.Descriptor, (o) => {
                Log.Debug("(MultiMapInfo)");
                MultiMapInfo response = MultiMapInfo.Parser.ParseFrom((o as Any).Value);
                // Start new thread to load maps
                Task.Run(() => LoadMaps(response));
            });
        }

        private void  LoadMaps(MultiMapInfo response)
        {
            response.Maps.ToList().ForEach(response => LoadMap(response));
        }

        private void LoadMap(MapInfo response)
        {
            Map map = new(response.X, response.Y, response.Z, response.Map);
            //String mapName = ContentLibrary.CreateMapName(response.X, response.Y, response.Z, response.ZoomLevel);
            Log.Debug("map: " + map.Location + ", Thread.CurrentThread.ManagedThreadId: " + Thread.CurrentThread.ManagedThreadId);
            /*if (game.ContentLibrary.HasMap(mapName))
            {
                System.Diagnostics.Debug.WriteLine("RunningState(MultiMapInfo) - Map already loaded: " + mapName);
                return;
            }*/
            //System.Diagnostics.Debug.WriteLine("RunningState.LoadMap ManagedThreadId: " + Thread.CurrentThread.ManagedThreadId);
            //System.Diagnostics.Debug.WriteLine("RunningState.LoadMap calling Thread.sleep() 60 seconds: ");
            //await Task.Run(() => Thread.Sleep(60000));
            game.ContentLibrary.AddMap(map, false, true, 0.25f);
            game.ContentLibrary.UpdateCaches(game.GameWorld.GetPlayer());
        }
        
        internal void SendDelayedMapsRequest()
        {
            Thread.Sleep(1000);
            SendMapsRequest();
        }

        private void SendMapsRequest()
        {
            Player p = game.GameWorld.GetPlayer();
            Point3D mapIndex = CoordinateHelper.WorldCoordsToMapIndex(p.Location, content);
            CenterMapsRequest zoneMapsRequest = RequestFactory.CreateZoneMapsRequest(mapIndex, 0, 6);
            serverConnector.SendMessage(zoneMapsRequest);
            GameWorld world = game.GameWorld;
            world.LastMapFetchLocation = p.Location;
        }

        internal override void OnClientSizeChanged(int newWindowWidth, int newWindowHeight)
        {

        }

        public override void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {
            ///System.Diagnostics.Debug.WriteLine("RunningState.LoadMap Update");
            Settings settings = game.GameSettings;
            if (settings == null) return;
            int dir = ms.ScrollWheelValue - settings.LastScrollWheelValue;
            if (dir != 0)
            {
                if (dir > 0)
                {
                    ZoomIn();
                }
                else
                {
                    ZoomOut();
                }
            }
            settings.LastScrollWheelValue = ms.ScrollWheelValue;
        }

        protected void ZoomIn()
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            if (--settings.ZoomLevel <= -1)
                settings.ZoomLevel = 0;

            //System.Diagnostics.Debug.WriteLine("RunningState settings.ZoomLevel " + settings.ZoomLevel);
        }
        protected void ZoomOut()
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            if (++settings.ZoomLevel >= Settings.SCALE_VALUES.Count)
                settings.ZoomLevel = Settings.SCALE_VALUES.Count - 1;

            //System.Diagnostics.Debug.WriteLine("RunningState settings.ZoomLevel " + settings.ZoomLevel);
        }

        public override void Draw(GameTime gameTime)
        {
            Settings settings = game.GameSettings;
            if (settings != null)
            {
                game.renderer.Draw(game.GameWorld, gameTime);
            }
        }
    }
}
