using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.UI
{
    /**
     * Things in this UI is a left panel with different paint tools, and a tile picker at the bottom picker.
     */
    internal class ArtistUI
    {
        private Grid _paintToolsPanel;

        public ArtistUI()
        {
            _paintToolsPanel = new Grid
            {
                ShowGridLines = true,
                ColumnSpacing = 8,
                RowSpacing = 8,
            };

            // Set partitioning configuration
            _paintToolsPanel.ColumnsProportions.Add(new Proportion());
            _paintToolsPanel.ColumnsProportions.Add(new Proportion());
            _paintToolsPanel.RowsProportions.Add(new Proportion());
            _paintToolsPanel.RowsProportions.Add(new Proportion());

            // Add widgets
            var btnPenTool = new TextButton();
            btnPenTool.Text = "Pen";
            btnPenTool.GridColumn = 1;
            btnPenTool.GridRow = 1;
            _paintToolsPanel.Widgets.Add(btnPenTool);

            var btnLineTool = new TextButton();
            btnLineTool.Text = "Line";
            btnLineTool.GridColumn = 2;
            btnLineTool.GridRow = 1;
            _paintToolsPanel.Widgets.Add(btnLineTool);

            var btnFillTool = new TextButton();
            btnFillTool.Text = "Fill";
            btnFillTool.GridColumn = 1;
            btnFillTool.GridRow = 2;
            _paintToolsPanel.Widgets.Add(btnFillTool);
        }

        public Widget GetWidget()
        {
            return _paintToolsPanel;
        }
    }
        
}
