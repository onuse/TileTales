using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Net.Tiletales.Network.Proto.App;
using Net.Tiletales.Network.Proto.Game;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TileTales.GameContent;
using TileTales.Graphics;
using TileTales.Network;
using TileTales.UI;
using TileTales.Utils;

namespace TileTales.State
{
    internal class GameState : BaseState
    {
        private object moveRequestThrottle;

        public GameState(TileTalesGame game) : base(game)
        {
            Settings settings = game.GameSettings;
            eventBus.Subscribe(ObjectLocationUpdate.Descriptor, (o) => {
                System.Diagnostics.Debug.WriteLine("GameState(ObjectLocationUpdate)");
                ObjectLocationUpdate response = ObjectLocationUpdate.Parser.ParseFrom((o as Any).Value);
                if (response.ObjectId == Player.ObjectId)
                {
                    Player.X = response.X;
                    Player.Y = response.Y;
                    Player.Z = response.Z;
                }
            });

            eventBus.Subscribe(MapInfo.Descriptor, (o) => {
                System.Diagnostics.Debug.WriteLine("GameState(MultiMapInfo)");
                MapInfo response = MapInfo.Parser.ParseFrom((o as Any).Value);
                Task.Run(() => LoadMap(response));
            });

            eventBus.Subscribe(MultiMapInfo.Descriptor, (o) => {
                System.Diagnostics.Debug.WriteLine("GameState(MultiMapInfo)");
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
            game.ContentLibrary.AddMap(mapName, mapBytes, false, true);
        }

        internal override void OnClientSizeChanged(int newWindowWidth, int newWindowHeight)
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            settings.WindowWidth = newWindowWidth;
            settings.WindowHeight = newWindowHeight;
        }
        
        public override void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            int deltaX = 0;
            int deltaY = 0;
            int deltaZ = 0;
            if (ks.IsKeyDown(Keys.Up) || ks.IsKeyDown(Keys.W))
                deltaY = -16;
            if (ks.IsKeyDown(Keys.Down) || ks.IsKeyDown(Keys.S))
                deltaY = 16;
            if (ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.A))
                deltaX = -16;
            if (ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.D))
                deltaX = +16;

            if (deltaX != 0 || deltaY != 0 || deltaZ != 0)
            {
                sendMoveRequestThrottled(deltaX, deltaY, deltaZ);
            }

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

        private void sendMoveRequestThrottled(int deltaX, int deltaY, int deltaZ)
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            if (moveRequestThrottle == null)
            {
                moveRequestThrottle = new Timer((o) =>
                {
                    MoveRequest moveRequest = rf.createMoveRequest(deltaX, deltaY, deltaZ);
                    serverConnector.SendMessage(moveRequest);
                    moveRequestThrottle = null;
                }, null, 150, Timeout.Infinite);
            }
        }

        private void ZoomIn()
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            if (--settings.ZoomLevel <= -1)
                settings.ZoomLevel = 0;
            
            System.Diagnostics.Debug.WriteLine("GameState settings.ZoomLevel " + settings.ZoomLevel);
        }
        private void ZoomOut()
        {
            Settings settings = game.GameSettings;
            if (settings == null) return;
            if (++settings.ZoomLevel >= Settings.SCALE_VALUES.Count)
                settings.ZoomLevel = Settings.SCALE_VALUES.Count - 1;

            System.Diagnostics.Debug.WriteLine("GameState settings.ZoomLevel " + settings.ZoomLevel);
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
