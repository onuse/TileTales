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

namespace TileTales.State
{
    internal class RunningState : BaseState
    {
        public RunningState() : base()
        {
            Settings settings = game.GameSettings;
            eventBus.Subscribe(ObjectLocationUpdate.Descriptor, (o) => {
                System.Diagnostics.Debug.WriteLine("RunningState(ObjectLocationUpdate)");
                ObjectLocationUpdate response = ObjectLocationUpdate.Parser.ParseFrom((o as Any).Value);
                if (response.ObjectId == Player.ObjectId)
                {
                    Player.X = response.X;
                    Player.Y = response.Y;
                    Player.Z = response.Z;
                }
            });

            eventBus.Subscribe(MapInfo.Descriptor, (o) => {
                System.Diagnostics.Debug.WriteLine("RunningState(MultiMapInfo)");
                MapInfo response = MapInfo.Parser.ParseFrom((o as Any).Value);
                Task.Run(() => LoadMap(response));
            });

            eventBus.Subscribe(MultiMapInfo.Descriptor, (o) => {
                System.Diagnostics.Debug.WriteLine("RunningState(MultiMapInfo)");
                MultiMapInfo response = MultiMapInfo.Parser.ParseFrom((o as Any).Value);
                // Start new thread to load maps
                Task.Run(() => LoadMaps(response));
            });
        }

        private void LoadMaps(MultiMapInfo response)
        {
            response.Maps.ToList().ForEach(response => LoadMap(response));
        }

        private void LoadMap(MapInfo response)
        {
            ByteString mapBytes = response.Map;
            String mapName = ContentLibrary.CreateMapName(response.X, response.Y, response.Z, response.ZoomLevel);
            if (game.ContentLibrary.HasMap(mapName))
            {
                System.Diagnostics.Debug.WriteLine("RunningState(MultiMapInfo) - Map already loaded: " + mapName);
                return;
            }
            game.ContentLibrary.AddMap(mapName, mapBytes, false, true);
        }

        internal override void OnClientSizeChanged(int newWindowWidth, int newWindowHeight)
        {

        }

        public override void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {
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
