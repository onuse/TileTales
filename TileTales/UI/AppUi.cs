using Microsoft.Xna.Framework;
using Myra;
using Myra.Graphics2D.UI;
using System;
using TileTales.Graphics;
using TileTales.Utils;

namespace TileTales.UI {
    internal class AppUI {
        private Desktop _desktop;
        private readonly GraphicsDeviceManager _graphics;
        private readonly TileTalesGame _game;
        private VerticalStackPanel _uiContainer;
        private Widget _currentTab;

        internal MainMenu _mainMenu;
        // Tab contents
        internal ArtistUI _artistUI;
        internal GameUI _gameUI;
        internal Label fpsLabel;
        internal Label mpfLabel;

        public AppUI(TileTalesGame game, GraphicsDeviceManager graphics) {
            _graphics = graphics;
            _game = game;
            // Initialize Myra UI
            MyraEnvironment.Game = _game;
        }
        public void LoadMainMenu() {
            _game.IsMouseVisible = true;
            _game.Window.AllowUserResizing = true;
            _desktop = new Desktop {
                HasExternalTextInput = true
            };

            // Provide that text input
            _game.Window.TextInput += (s, a) => {
                _desktop.OnChar(a.Character);
            };

            _uiContainer = new VerticalStackPanel();

            _mainMenu = new MainMenu();

            EventBus.Singleton.Subscribe(EventType.LoadGameUI, (data) => {
                _artistUI = new ArtistUI(_game.ContentLibrary);
                _gameUI = new GameUI();
                StackPanel tabPanel = createTabPanel();
                _uiContainer.Widgets.Add(tabPanel);
                _artistUI.Load();
                _artistUI.SetWidth(_graphics.PreferredBackBufferWidth);
                _artistUI.SetHeight(_graphics.PreferredBackBufferHeight);
                _gameUI.Load();
                _gameUI.SetWidth(_graphics.PreferredBackBufferWidth);
                _gameUI.SetHeight(_graphics.PreferredBackBufferHeight);
                Panel overlayPanel = createOverlayPanel();
                _uiContainer.Widgets.Add(overlayPanel);
                _desktop.Root = _uiContainer;
                EventBus.Singleton.Publish(EventType.GameUILoaded, null);
            });
            EventBus.Singleton.Subscribe(EventType.LoadWorldGenUI, (data) => {
                _desktop.Root = _uiContainer;
                EventBus.Singleton.Publish(EventType.GameWorldGenUILoaded, null);
            });

            EventBus.Singleton.Subscribe(EventType.ConnectFailed, (data) => {
                popConnectErrorUI((string)data);
            });
        }

        public void Draw() {
            // Drawing UI
            _desktop.Render();
        }

        public void Update() {

            //Console.WriteLine((_desktop.Widgets.ToList().Find((Widget w) => { return w.Id == "Expression"; }) as TextBox)?.Text);
        }

        public ArtistUI GetArtistUI() {
            return _artistUI;
        }

        public GameUI GetGameUI() {
            return _gameUI;
        }

        private StackPanel createTabPanel() {
            var tabPanel = new HorizontalStackPanel {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                ShowGridLines = true,
                Spacing = 8,
                Background = new Myra.Graphics2D.Brushes.SolidBrush(Color.Red)
            };

            Button btnArtist = CreateModeButton("Artist mode", 40, (s, a) => {
                SetTab(_artistUI.GetWidget());
                _game.ActivateArtistState();
            });
            tabPanel.Widgets.Add(btnArtist);

            Button btnGame = CreateModeButton("Game mode", 40, (s, a) => {
                SetTab(_gameUI.GetWidget());
                _game.ActivateGameState();
            });
            tabPanel.Widgets.Add(btnGame);

            return tabPanel;
        }

        private static Button CreateModeButton(string text, int Height, EventHandler onClick) {
            Label lbl = new() {
                Text = text
            };
            var btn = new Button {
                Content = lbl,
                Height = Height
            };
            btn.Click += onClick;
            return btn;
        }

        private Panel createOverlayPanel() {
            var overlayPanel = new Panel();

            fpsLabel = new Label {
                Text = "FPS Text",
                HorizontalAlignment = HorizontalAlignment.Right,
                Left = -10,
                Top = 0
            };
            overlayPanel.Widgets.Add(fpsLabel);

            mpfLabel = new Label {
                Text = "MPF Text",
                HorizontalAlignment = HorizontalAlignment.Right,
                Left = -10,
                Top = 20
            };
            overlayPanel.Widgets.Add(mpfLabel);

            return overlayPanel;

        }

        private void SetTab(Widget tab) {
            if (_currentTab != null) {
                _uiContainer.Widgets.Remove(_currentTab);
            }
            _currentTab = tab;
            _uiContainer.Widgets.Add(_currentTab);
        }

        [Obsolete]
        private void createLoginWindow() {
            Window window = new() {
                Title = "Simple Window"
            };

            TextButton button = new() {
                Text = "Push Me!",
                HorizontalAlignment = HorizontalAlignment.Center
            };

            window.Content = button;

            window.Closed += (s, a) => {
                // Called when window is closed
            };

            window.ShowModal(_desktop);
        }

        internal void popConnectErrorUI(String technicalInfo) {
            ShowPopupMessage("Error", technicalInfo);
        }

        internal MainMenu ShowStartMenu() {
            _desktop.Root = _mainMenu.GetWidget();
            return _mainMenu;
        }

        internal void ShowPopupMessage(string title, string message) {

            Window window = new() {
                Title = title
            };

            VerticalStackPanel verticalStackPanel = new() {
                ShowGridLines = true,
                Spacing = 8
            };

            verticalStackPanel.Widgets.Add(new Label { Text = message, Wrap = true });

            Button okButton = new() {
                Content = new Label { Text = "OK" },
                HorizontalAlignment = HorizontalAlignment.Center
            };
            okButton.Click += (s, a) => {
                window.Close();
            };
            verticalStackPanel.Widgets.Add(okButton);

            window.Content = verticalStackPanel;

            window.Closed += (s, a) => {
                // Called when window is closed
            };

            window.ShowModal(_desktop);
        }

        public Tile GetSelectedTile() {
            return _artistUI.ActiveTile;
        }

        internal bool IsMouseOverGUI() {
            return _desktop.IsMouseOverGUI;
        }

        public int Width {
            get { return _graphics.PreferredBackBufferWidth; }
            set {
                //_mainMenu.SetWidth(value);
                if (_artistUI != null) _artistUI.SetWidth(value);
                if (_gameUI != null) _gameUI.SetWidth(value);
            }
        }

        public int Height {
            get { return _graphics.PreferredBackBufferHeight; }
            set {
                //_mainMenu.SetHeight(value);
                if (_artistUI != null) _artistUI.SetHeight(value);
                if (_gameUI != null) _gameUI.SetHeight(value);
            }
        }
    }
}
