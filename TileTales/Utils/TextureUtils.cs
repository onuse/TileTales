using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TileTales.Utils {
    internal class TextureUtils {
        private static TextureUtils _instance;

        public static TextureUtils Singleton {
            get {
                if (_instance == null) {
                    _instance = new TextureUtils();
                }
                return _instance;
            }
        }

        public static Texture2D CreateTexture(GraphicsDevice graphicsDevice, int width, int height, Color color) {
            var texture = new Texture2D(graphicsDevice, width, height);
            var data = new Color[width * height];
            for (var i = 0; i < data.Length; ++i) {
                data[i] = color;
            }
            texture.SetData(data);
            return texture;
        }

        public static Texture2D scaleTexture(Texture2D texture, int newWidth, int newHeight) {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData(data);
            Color[] newData = new Color[newWidth * newHeight];
            int sourceWidth = texture.Width;
            int sourceHeight = texture.Height;
            float xRatio = (float)sourceWidth / newWidth;
            float yRatio = (float)sourceHeight / newHeight;
            float xPosition, yPosition;
            int sourcePosition, targetPosition;
            for (int y = 0; y < newHeight; y++) {
                for (int x = 0; x < newWidth; x++) {
                    xPosition = x * xRatio;
                    yPosition = y * yRatio;
                    sourcePosition = (int)(Math.Floor(yPosition) * sourceWidth + Math.Floor(xPosition));
                    targetPosition = y * newWidth + x;
                    newData[targetPosition] = data[sourcePosition];
                }
            }
            Texture2D newTexture = new Texture2D(texture.GraphicsDevice, newWidth, newHeight);
            newTexture.SetData(newData);
            return newTexture;
        }

        public static Texture2D scaleTexture(Texture2D texture, float scale) {
            return scaleTexture(texture, (int)(texture.Width * scale), (int)(texture.Height * scale));
        }
    }
}
