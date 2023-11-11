using Myra.Graphics2D.UI;

namespace TileTales.UI {
    internal class GameUI {
        private Panel panel;

        internal Button paddedCenteredButton;
        private Label paddedCenteredButtonLabel;

        internal Button fixedSizeButton;
        private Label fixedSizeButtonLabel;

        public GameUI() {
        }

        public Widget GetWidget() {
            return panel;
        }

        internal void Load() {
            panel = new Panel();

            paddedCenteredButtonLabel = new Label {
                Text = "Padded Centered Button"
            };
            paddedCenteredButton = new Button {
                Content = paddedCenteredButtonLabel,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            panel.Widgets.Add(paddedCenteredButton);

            var rightBottomText = new Label {
                Text = "Right Bottom Text",
                Left = -30,
                Top = -20,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            panel.Widgets.Add(rightBottomText);

            fixedSizeButtonLabel = new Label {
                Text = "Fixed Size Button"
            };
            fixedSizeButton = new Button {
                Content = fixedSizeButtonLabel,
                Width = 110,
                Height = 80
            };
            panel.Widgets.Add(fixedSizeButton);
        }

        internal void SetWidth(int value) {
            panel.Width = value;
        }

        internal void SetHeight(int value) {
            panel.Height = value;
        }
    }
}