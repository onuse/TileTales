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

        public static ArtistState Singleton
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

        private readonly ArtistUI artistUI;

        private ArtistState() : base()
        {
            ActiveBrushType = BrushType.TilePen;
            artistUI = game.AppUI.GetArtistUI();
            artistUI.btnLineTool.Click += (s, a) => { ActiveBrushType = BrushType.TileLine; };
            artistUI.btnPenTool.Click += (s, a) => { ActiveBrushType = BrushType.TilePen; };
            artistUI.btnBrushTool.Click += (s, a) => { ActiveBrushType = BrushType.TileBrush; };
            artistUI.btnFillTool.Click += (s, a) => { ActiveBrushType = BrushType.TileFill; };
        }
        
        public override void Update(GameTime gameTime, KeyboardState ks, MouseState ms)
        {
            base.Update(gameTime, ks, ms);

            if (!IsMouseInsideWindow() || IsMouseOverUI())
            {
                return;
            }

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
                    if (ActiveBrushType == BrushType.TilePen || ActiveBrushType == BrushType.TileBrush)
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
                        if (ActiveBrushType == BrushType.TilePen || ActiveBrushType == BrushType.TileBrush)
                        {
                            if (ActiveBrushType == BrushType.TilePen)
                            {
                                Point latestPoint = paintPoints.Last();
                                List<Point> points = PointUtils.GetPointsBetween(latestPoint.X, latestPoint.Y, worldX, worldY);
                                foreach (Point point in points)
                                {
                                    paintPoints.Add(point);
                                }
                            }
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
                    if (ActiveBrushType == BrushType.TilePen || ActiveBrushType == BrushType.TileBrush)
                    {
                        SendDrawMultiTileRequest(paintPoints, 0);
                    }
                    else if (ActiveBrushType == BrushType.TileLine)
                    {
                        SendDrawLineRequest(paintStartX, paintStartY, worldX, worldY, 0);
                    }
                    else if (ActiveBrushType == BrushType.TileFill)
                    {
                        SendBucketFillRequest(worldX, worldY, 0);
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
                    SendTeleportRequest(teleportX, teleportY, 0);
                    teleportX = 0;
                    teleportY = 0;
                }
            }
        }

        private void SendBucketFillRequest(int worldX, int worldY, int v)
        {
            BucketFillRequest bucketFillRequest = rf.createBucketFillRequest(worldX, worldY, v, (uint)ui.getSelectedTile().LegacyColor);
            serverConnector.SendMessage(bucketFillRequest);
        }

        private void SendDrawMultiTileRequest(HashSet<Point> paintPoints, int z)
        {
            Tile selectedTile = ui.getSelectedTile();
            if (selectedTile == null)
            {
                return;
            }
            DrawMultiTileRequest drawMultiTileRequest = rf.createDrawMultiTileRequest(paintPoints, z, (uint)selectedTile.LegacyColor);
            serverConnector.SendMessage(drawMultiTileRequest);
        }

        private void SendDrawLineRequest(int paintStartX, int paintStartY, int paintEndX, int paintEndY, int z)
        {
            Log.Debug("ArtistState.sendDrawLineRequest(paintStartX: " + paintStartX + ", paintStartY: " + paintStartY + ", paintEndX: " + paintEndX + ", paintEndY: " + paintEndY + ", z: " + z);
            Tile selectedTile = ui.getSelectedTile();
            if (selectedTile == null)
            {
                return;
            }
            DrawLineRequest drawLineRequest = rf.createDrawLineRequest(paintStartX, paintStartY, paintEndX, paintEndY, z);
            drawLineRequest.TileId = (uint)selectedTile.LegacyColor;
            serverConnector.SendMessage(drawLineRequest);
        }

        private void SendTeleportRequest(int teleportX, int teleportY, int z)
        {
            Log.Debug("ArtistState.SendTeleportRequest(teleportX: " + teleportX + ", teleportY: " + teleportY + ", z: " + z);
            TeleportRequest teleportRequest = rf.createTeleportRequest(teleportX, teleportY, z);
            serverConnector.SendMessage(teleportRequest);
        }
    }
}
