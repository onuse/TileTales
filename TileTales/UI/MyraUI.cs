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
    internal class MyraUI
    {

        private Desktop _desktop;
        private GraphicsDeviceManager _graphics;
        private Game _game;
        private Grid _uiContainer;
        private ArtistUI _artistUI;
        private GameUI _gameUI;
        public MyraUI(Game game, GraphicsDeviceManager graphics)
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
                Background = new Myra.Graphics2D.Brushes.SolidBrush(Color.Aqua)
                //Width = WINDOW_WIDTH,
                //Height = WINDOW_HEIGHT
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

            //createTestWindow();
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

        private void createTestGrid()
        {
            Grid g = new Grid();
            var panel = new Panel();
            panel.Widgets.Add(new TextBox() { Text = "this.X=W.w/4;this.Y=W.h/2", Id = "Expression", Top = 25, Width = 100, Height = 100 });
            var btnA = new TextButton();
            var btnB = new TextButton() { Text = "Button", Left = 500, Top = 500 };
            btnB.Click += (sender, e) => { Console.WriteLine(btnB.Layout2d.Expresion); };

            btnA.Text = "Calc";
            btnA.Click += (sender, e) => { btnB.Layout2d.Expresion = (_desktop.GetWidgetByID("Expression") as TextBox).Text; _desktop.InvalidateLayout(); _desktop.UpdateLayout(); };
            panel.Widgets.Add(btnA);

            g.Widgets.Add(panel);
            g.Widgets.Add(btnB);

            _desktop.Root = g;
        }

        private void createTestWindow()
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
    }
}
