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
        private MultipleItemsContainerBase panel;

        internal TextButton paddedCenteredButton;
        internal TextButton fixedSizeButton;

        public GameUI()
        {

        }

        public Widget GetWidget()
        {
            return panel;
        }

        internal void Load()
        {
            panel = new Panel();
            var positionedText = new Label();
            positionedText.Text = "Positioned Text";
            positionedText.Left = 50;
            positionedText.Top = 100;
            panel.Widgets.Add(positionedText);

            paddedCenteredButton = new TextButton();
            paddedCenteredButton.Text = "Padded Centered Button";
            paddedCenteredButton.HorizontalAlignment = HorizontalAlignment.Center;
            paddedCenteredButton.VerticalAlignment = VerticalAlignment.Center;
            panel.Widgets.Add(paddedCenteredButton);

            var rightBottomText = new Label();
            rightBottomText.Text = "Right Bottom Text";
            rightBottomText.Left = -30;
            rightBottomText.Top = -20;
            rightBottomText.HorizontalAlignment = HorizontalAlignment.Right;
            rightBottomText.VerticalAlignment = VerticalAlignment.Bottom;
            panel.Widgets.Add(rightBottomText);

            fixedSizeButton = new TextButton();
            fixedSizeButton.Text = "Fixed Size Button";
            fixedSizeButton.Width = 110;
            fixedSizeButton.Height = 80;
            panel.Widgets.Add(fixedSizeButton);
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