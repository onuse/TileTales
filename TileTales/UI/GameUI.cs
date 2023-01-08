using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.UI
{
    internal class GameUI
    {
        private Grid _gameActionsPanel;

        public GameUI()
        {
            _gameActionsPanel = new Grid
            {
                ShowGridLines = true,
                ColumnSpacing = 8,
                RowSpacing = 8,
            };

        }



        public Widget GetWidget()
        {
            return _gameActionsPanel;
        }
    }
}