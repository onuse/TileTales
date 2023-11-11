using Microsoft.Xna.Framework.Graphics;
using Myra.Graphics2D.TextureAtlases;
using Myra.Graphics2D.UI;
using System.Collections.Generic;
using TileTales.GameContent;
using TileTales.Graphics;
using TileTales.Utils;

namespace TileTales.UI {
    /**
     * Things in this UI is a left panel with different paint tools, and a tile picker at the bottom picker.
     */
    internal class ArtistUI {
        private readonly ContentLibrary _contentLibrary;
        private Panel _panel;
        private Grid _paintToolsPanel;
        private Grid _tileSelectionPanel;

        public Button btnPenTool;
        public Button btnLineTool;
        public Button btnFillTool;
        public Button btnBrushTool;

        public ArtistUI(ContentLibrary contentLibrary) {
            _contentLibrary = contentLibrary;
        }

        public Tile ActiveTile { get; set; }

        public void Load() {
            _panel = new Panel();

            _panel.Widgets.Add(CreateToolPanel());
            EventBus.Singleton.Subscribe(EventType.AllTilesSaved, (data) => {
                Widget tilePanel = CreateTilePanel();
                _panel.Widgets.Add(tilePanel);
            });
        }

        private Widget CreateToolPanel() {
            _paintToolsPanel = new Grid {
                ShowGridLines = true,
                ColumnSpacing = 8,
                RowSpacing = 8,
                Top = 5,
                Width = 80,
                Height = 80
            };

            // Set partitioning configuration
            _paintToolsPanel.ColumnsProportions.Add(new Proportion());
            _paintToolsPanel.ColumnsProportions.Add(new Proportion());
            _paintToolsPanel.RowsProportions.Add(new Proportion());
            _paintToolsPanel.RowsProportions.Add(new Proportion());

            // Add buttons
            btnPenTool = CreateButton("Pen", 1, 1);
            _paintToolsPanel.Widgets.Add(btnPenTool);

            btnLineTool = CreateButton("Line", 2, 1);
            _paintToolsPanel.Widgets.Add(btnLineTool);

            btnFillTool = CreateButton("Fill", 1, 2);
            _paintToolsPanel.Widgets.Add(btnFillTool);

            btnBrushTool = CreateButton("Brush", 2, 2);
            _paintToolsPanel.Widgets.Add(btnBrushTool);

            return _paintToolsPanel;
        }

        private static Button CreateButton(string text, int col, int row) {
            Label label = new() {
                Text = text
            };
            Button button = new() {
                Content = label
            };
            Grid.SetColumn(button, col);
            Grid.SetRow(button, row);
            return button;
        }

        private Widget CreateTilePanel() {
            //System.Diagnostics.Debug.WriteLine("ArtistUI - Creating tile selection panel");
            float scale = 2f;// _contentLibrary.GetScale();
            List<Tile> allTiles = _contentLibrary.GetAllTiles();
            _tileSelectionPanel = new Grid {
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
            foreach (Tile tile in allTiles) {
                Texture2D texture = TextureUtils.scaleTexture(tile.Image, iconSize, iconSize);
                TextureRegion reg = new(texture);
                Image image = new() {
                    Renderable = reg
                };
                Button imgButton = new() {
                    Content = image,
                    Width = iconSize + 2,
                    Height = iconSize + 2
                };
                Grid.SetColumn(imgButton, col);
                Grid.SetRow(imgButton, row);
                imgButton.Click += (s, a) => {
                    ActiveTile = tile;
                };

                //System.Diagnostics.Debug.WriteLine("ArtistUI - Adding tile " + texture.Width + " to tile selection panel");

                // Add the widget to the grid
                _tileSelectionPanel.Widgets.Add(imgButton);

                // Increment the row or column index based on whether we've added the maximum number of columns
                row++;
                if (row == 2) {
                    row = 0;
                    col++;
                }
            }

            return _tileSelectionPanel;
        }

        public Widget GetWidget() {
            return _panel;
        }

        internal void SetWidth(int value) {
            _panel.Width = value;
        }

        internal void SetHeight(int value) {
            _panel.Height = value;
        }
    }

}
