using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TileTales.UI
{
    internal class AppUI
    {

        private Desktop _desktop;
        private GraphicsDeviceManager _graphics;
        private Game _game;
        private Grid _uiContainer;
        private ArtistUI _artistUI;
        private GameUI _gameUI;
        public AppUI(Game game, GraphicsDeviceManager graphics)
        {
            _graphics = graphics;
            _game = game;
            // Initialize Myra UI
            MyraEnvironment.Game = _game;
        }
        public void LoadContent()
        {
            _game.IsMouseVisible = true;
            _game.Window.AllowUserResizing = true;
            _desktop = new Desktop
            {
                HasExternalTextInput = true
            };

            // Provide that text input
            _game.Window.TextInput += (s, a) =>
            {
                _desktop.OnChar(a.Character);
            };

            _uiContainer = new Grid
            {
                //Background = new Myra.Graphics2D.Brushes.SolidBrush(Color.Beige)
            };
            _uiContainer.ColumnsProportions.Add(new Proportion());
            _uiContainer.RowsProportions.Add(new Proportion());
            _uiContainer.RowsProportions.Add(new Proportion());

            StackPanel tabPanel = createTabPanel();
            tabPanel.GridRow = 1;
            _uiContainer.Widgets.Add(tabPanel);
            
            _artistUI = new ArtistUI();
            Widget artistUiWidget = _artistUI.GetWidget();
            artistUiWidget.GridRow = 2;
            _uiContainer.Widgets.Add(artistUiWidget);

            _gameUI = new GameUI();
            Widget gameUiWidget = _gameUI.GetWidget();
            gameUiWidget.GridRow = 2;
            _uiContainer.Widgets.Add(gameUiWidget);

            _desktop.Root = _uiContainer;

            //createLoginWindow();
        }

        public void Draw()
        {
            // Drawing UI
            _desktop.Render();
        }
        
        public void Update()
        {

            //Console.WriteLine((_desktop.Widgets.ToList().Find((Widget w) => { return w.Id == "Expression"; }) as TextBox)?.Text);
        }

        private StackPanel createTabPanel()
        {
            var tabPanel = new HorizontalStackPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                //Height = 60,
            };
            tabPanel.ShowGridLines = true;
            tabPanel.Spacing = 8;
            tabPanel.Background = new Myra.Graphics2D.Brushes.SolidBrush(Color.Red);

            var btnArtist = new TextButton();
            btnArtist.Text = "Artist mode";
            btnArtist.Height = 40;
            btnArtist.Click += (s, a) =>
            {
                _artistUI.GetWidget().Visible = true;
                _gameUI.GetWidget().Visible = false;
            };
            tabPanel.Widgets.Add(btnArtist);

            var btnGame = new TextButton();
            btnGame.Text = "Game mode";
            btnGame.Height = 40;
            btnGame.Click += (s, a) =>
            {
                _artistUI.GetWidget().Visible = false;
                _gameUI.GetWidget().Visible = true;
            };
            tabPanel.Widgets.Add(btnGame);

            return tabPanel;
        }

        private void createLoginWindow()
        {
            Window window = new Window
            {
                Title = "Simple Window"
            };

            TextButton button = new TextButton
            {
                Text = "Push Me!",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            window.Content = button;

            window.Closed += (s, a) =>
            {
                // Called when window is closed
            };

            window.ShowModal(_desktop);
        }

        internal void popConnectErrorUI(String technicalInfo)
        {
            Window window = new Window
            {
                Title = "Connection error"
            };

            VerticalStackPanel verticalStackPanel = new VerticalStackPanel
            {
                ShowGridLines = true,
                Spacing = 8
            };

            var textBlock1 = new Label();
            textBlock1.Text = technicalInfo;
            textBlock1.Wrap = true;
            verticalStackPanel.Widgets.Add(textBlock1);

            TextButton button = new TextButton
            {
                Text = "wtf!",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            verticalStackPanel.Widgets.Add(button);

            window.Content = verticalStackPanel;

            window.Closed += (s, a) =>
            {
                // Called when window is closed
            };

            window.ShowModal(_desktop);
        }
    }
}
