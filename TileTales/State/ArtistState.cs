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
    internal class ArtistState : BaseState
    {
        public enum BrushType
        {
            TilePen,
            TileLine,
            TileBrush,
            TileFill
        }
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

        public BrushType ActiveBrushType  { get; set; }

        private int teleportX = 0;
        private int teleportY = 0;

        private int paintStartX = 0;
        private int paintStartY = 0;
        private int paintLastX = 0;
        private int paintLastY = 0;
        //private HashSet<Point> paintPoints;
        private String paintIndicator = null;
        private HashSet<Point> paintPoints = null;

        public ArtistState() : base()
        {
            ActiveBrushType = BrushType.TileLine;
        }
        
        public override void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {
            base.Update(gameTime, ks, ms);

            // TODO: Check if mouse is over application and not over UI

            if (ms.LeftButton == ButtonState.Pressed)
            {
                if (paintIndicator == null)
                {
                    paintIndicator = "";
                    game.GameWorld.ScreenToWorldX(ms.X, ms.Y, out int worldX, out int worldY);
                    paintStartX = worldX;
                    paintStartY = worldY;
                    paintLastX = worldX;
                    paintLastY = worldY;
                    if (ActiveBrushType == BrushType.TilePen)
                    {
                        paintPoints = new HashSet<Point>
                        {
                            new Point(worldX, worldY)
                        };
                    }
                    //paintPoints.Add(new Point(worldX, worldY));
                }
                else
                {
                    game.GameWorld.ScreenToWorldX(ms.X, ms.Y, out int worldX, out int worldY);
                    if (worldX != paintLastX || worldY != paintLastY) {
                        if (ActiveBrushType == BrushType.TilePen)
                        {
                            paintPoints.Add(new Point(worldX, worldY));
                        }
                        //sendDrawLineRequest(paintLastX, paintLastY, worldX, worldY, 0);
                        paintLastX = worldX;
                        paintLastY = worldY;
                    }
                }
            }
            else if (ms.LeftButton == ButtonState.Released)
            {
                if (paintIndicator != null)
                {
                    paintIndicator = null;
                    game.GameWorld.ScreenToWorldX(ms.X, ms.Y, out int worldX, out int worldY);
                    if (ActiveBrushType == BrushType.TilePen)
                    {
                        sendDrawLineRequest(paintPoints, 0);
                    }
                    else if (ActiveBrushType == BrushType.TileLine)
                    {
                        sendDrawLineRequest(paintStartX, paintStartY, worldX, worldY, 0);
                    }
                    paintStartX = 0;
                    paintStartY = 0;
                    paintLastX = 0;
                    paintLastY = 0;
                    paintPoints = null;
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

        private void sendDrawLineRequest(HashSet<Point> paintPoints, int z)
        {
            Tile selectedTile = ui.getSelectedTile();
            if (selectedTile == null)
            {
                return;
            }
            DrawMultiTileRequest drawMultiTileRequest = rf.createDrawMultiTileRequest(paintPoints, z, (uint)selectedTile.LegacyColor);
            serverConnector.SendMessage(drawMultiTileRequest);
        }

        private void sendDrawLineRequest(int paintStartX, int paintStartY, int paintEndX, int paintEndY, int z)
        {
            System.Diagnostics.Debug.WriteLine("ArtistState.sendDrawLineRequest(paintStartX: " + paintStartX + ", paintStartY: " + paintStartY + ", paintEndX: " + paintEndX + ", paintEndY: " + paintEndY + ", z: " + z);
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
