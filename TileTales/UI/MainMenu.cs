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
    internal partial class MainMenu
    {
        private VerticalStackPanel panel;

        internal readonly MenuItem menuStartNewGame;
        internal readonly MenuItem menuOptions;
        internal readonly MenuItem menuQuit;

        public MainMenu()
        {
            var textBlock1 = new TextBox();
            textBlock1.Text = "TileTales";
            textBlock1.TextColor = Color.Orange;
            textBlock1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;

            menuStartNewGame = new MenuItem();
            menuStartNewGame.Id = "_menuStartGame";
            menuStartNewGame.Text = "Start Game";

            menuOptions = new MenuItem();
            menuOptions.Id = "_menuOptions";
            menuOptions.Text = "Options";

            menuQuit = new MenuItem();
            menuQuit.Id = "_menuQuit";
            menuQuit.Text = "Quit"; 

            var verticalMenu1 = new VerticalMenu();
            verticalMenu1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
            verticalMenu1.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center;
            verticalMenu1.Items.Add(menuStartNewGame);
            verticalMenu1.Items.Add(menuOptions);
            verticalMenu1.Items.Add(menuQuit);

            panel = new VerticalStackPanel();

            panel.AddChild(textBlock1);
            panel.AddChild(verticalMenu1);

            panel.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
            panel.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center;
        }

        public Widget GetWidget()
        {
            return panel;
        }

        internal void SetWidth(int value)
        {
            panel.Width = value;
        }

        internal void SetHeight(int value)
        {
            panel.Height = value;
        }
    }
}
