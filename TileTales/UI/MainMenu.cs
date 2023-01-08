using Myra;
using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileTales.UI
{
    partial class MainMenu
    {
        private void BuildUI()
        {
            var textBlock1 = new TextBox();
            textBlock1.Text = "My Game";
            textBlock1.TextColor = Color.Orange;
            textBlock1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;

            _menuStartNewGame = new MenuItem();
            _menuStartNewGame.Id = "_menuStartNewGame";
            _menuStartNewGame.Text = "Start New Game";

            _menuOptions = new MenuItem();
            _menuOptions.Id = "_menuOptions";
            _menuOptions.Text = "Options";

            _menuQuit = new MenuItem();
            _menuQuit.Id = "_menuQuit";
            _menuQuit.Text = "Quit";

            var verticalMenu1 = new VerticalMenu();
            verticalMenu1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
            verticalMenu1.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center;
            verticalMenu1.Items.Add(_menuStartNewGame);
            verticalMenu1.Items.Add(_menuOptions);
            verticalMenu1.Items.Add(_menuQuit);


            //Widgets.Add(textBlock1);
            //Widgets.Add(verticalMenu1);
        }


        public MenuItem _menuStartNewGame;
        public MenuItem _menuOptions;
        public MenuItem _menuQuit;
    }
}
