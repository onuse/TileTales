using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TileTales.Utils;

namespace TileTales.UI
{
    internal class AppUI
    {
        private Desktop _desktop;
        private GraphicsDeviceManager _graphics;
        private TileTalesGame _game;
        private VerticalStackPanel _uiContainer;
        private Widget _currentTab;

        internal MainMenu _mainMenu;
        // Tab contents
        internal ArtistUI _artistUI;
        internal GameUI _gameUI;
        public AppUI(TileTalesGame game, GraphicsDeviceManager graphics)
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

            _uiContainer = new VerticalStackPanel();
            StackPanel tabPanel = createTabPanel();
            _uiContainer.Widgets.Add(tabPanel);
            
            _artistUI = new ArtistUI();
            _gameUI = new GameUI();
            _mainMenu = new MainMenu();
            
            EventBus.Instance.Subscribe("loadgame", (data) =>
            {
                _desktop.Root = _uiContainer;
            });
            EventBus.Instance.Subscribe("connectfailed", (data) =>
            {
                popConnectErrorUI((string)data);
            });
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
                setTab(_artistUI.GetWidget());
            };
            tabPanel.Widgets.Add(btnArtist);

            var btnGame = new TextButton();
            btnGame.Text = "Game mode";
            btnGame.Height = 40;
            btnGame.Click += (s, a) =>
            {
                setTab(_gameUI.GetWidget());
            };
            tabPanel.Widgets.Add(btnGame);

            return tabPanel;
        }

        private void setTab(Widget tab)
        {
            if (_currentTab != null)
            {
                _uiContainer.RemoveChild(_currentTab);
            }
            _currentTab = tab;
            _uiContainer.AddChild(_currentTab);
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
                Text = "OK",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            button.Click += (s, a) =>
            {
                window.Close();
            };
            verticalStackPanel.Widgets.Add(button);

            window.Content = verticalStackPanel;

            window.Closed += (s, a) =>
            {
                // Called when window is closed
            };

            window.ShowModal(_desktop);
        }

        internal MainMenu ShowStartMenu()
        {
            _desktop.Root = _mainMenu.GetWidget();
            return _mainMenu;
        }
    }
}
