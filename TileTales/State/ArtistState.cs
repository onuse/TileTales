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
    internal class ArtistState : RunningState
    {
        private static ArtistState _instance;

        public static ArtistState Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ArtistState();
                }
                return _instance;
            }
        }
        
        int teleportX = 0;
        int teleportY = 0;

        public ArtistState() : base()
        {
        }
        
        public override void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {
            base.Update(gameTime, ks, ms);
            if (ms.RightButton == ButtonState.Pressed)
            {
                game.GameWorld.ScreenToWorldX(ms.X, ms.Y, out int worldX, out int worldY);
                teleportX = worldX;
                teleportY = worldY;
            }
            else if (ms.RightButton == ButtonState.Released)
            {
                if (teleportX != 0 || teleportY != 0)
                {
                    sendTeleportRequest(teleportX, teleportY, 0);
                    teleportX = 0;
                    teleportY = 0;
                }
            }
        }

        private void sendTeleportRequest(int teleportX, int teleportY, int z)
        {
            TeleportRequest teleportRequest = rf.createTeleportRequest(teleportX, teleportY, z);
            serverConnector.SendMessage(teleportRequest);
        }
    }
}
