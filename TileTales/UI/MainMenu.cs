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
        private readonly VerticalStackPanel panel;

        internal readonly MenuItem menuStartNewGame;
        internal readonly MenuItem menuWorldGen;
        internal readonly MenuItem menuOptions;
        internal readonly MenuItem menuQuit;

        public MainMenu()
        {
            var textBlock1 = new TextBox
            {
                Text = "TileTales",
                TextColor = Color.Orange,
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center
            };

            menuStartNewGame = new MenuItem
            {
                Id = "_menuStartGame",
                Text = "Connect to server"
            };

            menuWorldGen = new MenuItem
            {
                Id = "_menuWorldGeneration",
                Text = "World Generation"
            };

            menuOptions = new MenuItem
            {
                Id = "_menuOptions",
                Text = "Options"
            };

            menuQuit = new MenuItem
            {
                Id = "_menuQuit",
                Text = "Quit"
            };

            var verticalMenu1 = new VerticalMenu();
            verticalMenu1.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
            verticalMenu1.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center;
            verticalMenu1.Items.Add(menuStartNewGame);
            verticalMenu1.Items.Add(menuWorldGen);
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
