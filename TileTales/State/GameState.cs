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
        private static GameState _instance;

        public static GameState Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameState();
                }
                return _instance;
            }
        }

        private object moveRequestThrottle;

        public GameState() : base()
        {
        }
        
        public override void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {
            base.Update(gameTime, ks, ms);
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
        }

        private void sendTeleportRequest(int teleportX, int teleportY, int z)
        {
            TeleportRequest teleportRequest = rf.createTeleportRequest(teleportX, teleportY, z);
            serverConnector.SendMessage(teleportRequest);
            moveRequestThrottle = null;
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
    }
}
