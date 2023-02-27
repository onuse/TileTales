using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Net.Tiletales.Network.Proto.App;
using Net.Tiletales.Network.Proto.Game;
using Net.Tiletales.Network.Proto.Paint;
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

        int paintStartX = 0;
        int paintStartY = 0;

        public ArtistState() : base()
        {
        }
        
        public override void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {
            base.Update(gameTime, ks, ms);

            // TODO: Check if mouse is over application and not over UI

            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (paintStartX == 0 || paintStartY == 0) { 
                    game.GameWorld.ScreenToWorldX(ms.X, ms.Y, out int worldX, out int worldY);
                    paintStartX = worldX;
                    paintStartY = worldY;
                }
            }
            else if (ms.LeftButton == ButtonState.Released)
            {
                if (paintStartX != 0 || paintStartY != 0)
                {
                    game.GameWorld.ScreenToWorldX(ms.X, ms.Y, out int worldX, out int worldY);
                    sendDrawLineRequest(paintStartX, paintStartY, worldX, worldY, 0);
                    paintStartX = 0;
                    paintStartY = 0;
                }
            }


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

        private void sendDrawLineRequest(int paintStartX, int paintStartY, int paintEndX, int paintEndY, int z)
        {
            Tile selectedTile = ui.getSelectedTile();
            if (selectedTile == null)
            {
                return;
            }
            DrawLineRequest drawLineRequest = rf.createDrawLineRequest(paintStartX, paintStartY, paintEndX, paintEndY, z);
            drawLineRequest.TileId = (uint)selectedTile.LegacyColor;
            serverConnector.SendMessage(drawLineRequest);
        }

        private void sendTeleportRequest(int teleportX, int teleportY, int z)
        {
            TeleportRequest teleportRequest = rf.createTeleportRequest(teleportX, teleportY, z);
            serverConnector.SendMessage(teleportRequest);
        }
    }
}
