using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileTales.GameContent;
using TileTales.Graphics;
using TileTales.Utils;

namespace TileTales.UI
{
    /**
     * Things in this UI is a left panel with different paint tools, and a tile picker at the bottom picker.
     */
    internal class ArtistUI
    {
        private Panel _panel;
        private Grid _paintToolsPanel;
        private Grid _tileSelectionPanel;
        private ContentLibrary _contentLibrary;

        public TextButton btnPenTool;
        public TextButton btnLineTool;
        public TextButton btnFillTool;
        public TextButton btnBrushTool;

        public ArtistUI(ContentLibrary contentLibrary)
        {
            _contentLibrary = contentLibrary;
        }

        public Tile ActiveTile { get; set; }

        public void Load()
        {
            _panel = new Panel();

            _panel.AddChild(CreateToolPanel());
            EventBus.Instance.Subscribe(EventType.AllTilesSaved, (data) =>
            {
                Widget tilePanel = CreateTilePanel();
                _panel.AddChild(tilePanel);
            });
        }

        private Widget CreateToolPanel()
        {
            _paintToolsPanel = new Grid
            {
                ShowGridLines = true,
                ColumnSpacing = 8,
                RowSpacing = 8,
            };

            _paintToolsPanel.Top = 5;
            _paintToolsPanel.Width = 80;
            _paintToolsPanel.Height = 80;

            // Set partitioning configuration
            _paintToolsPanel.ColumnsProportions.Add(new Proportion());
            _paintToolsPanel.ColumnsProportions.Add(new Proportion());
            _paintToolsPanel.RowsProportions.Add(new Proportion());
            _paintToolsPanel.RowsProportions.Add(new Proportion());

            // Add widgets
            btnPenTool = new TextButton();
            btnPenTool.Text = "Pen";
            btnPenTool.GridColumn = 1;
            btnPenTool.GridRow = 1;
            _paintToolsPanel.Widgets.Add(btnPenTool);

            btnLineTool = new TextButton();
            btnLineTool.Text = "Line";
            btnLineTool.GridColumn = 2;
            btnLineTool.GridRow = 1;
            _paintToolsPanel.Widgets.Add(btnLineTool);

            btnFillTool = new TextButton();
            btnFillTool.Text = "Fill";
            btnFillTool.GridColumn = 1;
            btnFillTool.GridRow = 2;
            _paintToolsPanel.Widgets.Add(btnFillTool);

            btnBrushTool = new TextButton();
            btnBrushTool.Text = "Brush";
            btnBrushTool.GridColumn = 2;
            btnBrushTool.GridRow = 2;
            _paintToolsPanel.Widgets.Add(btnBrushTool);

            return _paintToolsPanel;
        }

        private Widget CreateTilePanel()
        {
            //System.Diagnostics.Debug.WriteLine("ArtistUI - Creating tile selection panel");
            float scale = 2f;// _contentLibrary.GetScale();
            List<Tile> allTiles = _contentLibrary.GetAllTiles();
            _tileSelectionPanel = new Grid
            {
                ShowGridLines = true,
                ColumnSpacing = 8,
                RowSpacing = 8,
            };
            int iconSize = ((int)(16 * scale));
            _tileSelectionPanel.Width = allTiles.Count / 2 * (iconSize + 16) + 18;
            // Set partitioning configuration
            _tileSelectionPanel.ColumnsProportions.Add(new Proportion());
            _tileSelectionPanel.ColumnsProportions.Add(new Proportion());
            _tileSelectionPanel.RowsProportions.Add(new Proportion());
            _tileSelectionPanel.RowsProportions.Add(new Proportion());

            _tileSelectionPanel.VerticalAlignment = VerticalAlignment.Bottom;
            _tileSelectionPanel.HorizontalAlignment = HorizontalAlignment.Center;

            int row = 0;
            int col = 0;
            //System.Diagnostics.Debug.WriteLine("Nbr of tiles: " + allTiles.Count);
            foreach (Tile tile in allTiles)
            {
                Texture2D texture = TextureUtils.scaleTexture(tile.Image, iconSize, iconSize);
                TextureRegion reg = new TextureRegion(texture);
                ImageButton imgButton = new ImageButton();
                imgButton.Image = reg;
                imgButton.GridColumn = col;
                imgButton.GridRow = row;
                imgButton.Width = iconSize + 2;
                imgButton.Height = iconSize + 2;
                imgButton.Click += (s, a) =>
                {
                    ActiveTile = tile;
                };

                //System.Diagnostics.Debug.WriteLine("ArtistUI - Adding tile " + texture.Width + " to tile selection panel");

                // Add the widget to the grid
                _tileSelectionPanel.Widgets.Add(imgButton);
                
                // Increment the row or column index based on whether we've added the maximum number of columns
                row++;
                if (row == 2)
                {
                    row = 0;
                    col++;
                }
            }

            return _tileSelectionPanel;
        }

        public Widget GetWidget()
        {
            return _panel;
        }

        internal void SetWidth(int value)
        {
            _panel.Width = value;
        }

        internal void SetHeight(int value)
        {
            _panel.Height = value;
        }
    }
        
}
