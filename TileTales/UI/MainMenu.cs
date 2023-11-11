using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;

namespace TileTales.UI {
    internal partial class MainMenu {
        private readonly VerticalStackPanel panel;

        internal readonly MenuItem menuStartNewGame;
        internal readonly MenuItem menuOptions;
        internal readonly MenuItem menuQuit;

        public MainMenu() {
            var textBlock1 = new TextBox {
                Text = "TileTales",
                TextColor = Color.Orange,
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center
            };

            menuStartNewGame = new MenuItem {
                Id = "_menuStartGame",
                Text = "Connect to server"
            };

            menuOptions = new MenuItem {
                Id = "_menuOptions",
                Text = "Options"
            };

            menuQuit = new MenuItem {
                Id = "_menuQuit",
                Text = "Quit"
            };

            var verticalMenu1 = new VerticalMenu {
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center,
                VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center
            };
            verticalMenu1.Items.Add(menuStartNewGame);
            verticalMenu1.Items.Add(menuOptions);
            verticalMenu1.Items.Add(menuQuit);

            panel = new VerticalStackPanel();

            panel.Widgets.Add(textBlock1);
            panel.Widgets.Add(verticalMenu1);

            panel.HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Center;
            panel.VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Center;
        }

        public Widget GetWidget() {
            return panel;
        }

        internal void SetWidth(int value) {
            panel.Width = value;
        }

        internal void SetHeight(int value) {
            panel.Height = value;
        }
    }
}
