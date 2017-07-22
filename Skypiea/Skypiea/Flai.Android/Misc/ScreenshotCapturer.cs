#if WINDOWS

using System;
using System.IO;
using Flai.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Flai.Misc
{
    public class ScreenshotCapturer : FlaiDrawableGameComponent
    {
        private string _folderPath = typeof(FlaiGame).Assembly.Location;
        private Keys _screenshotKey = Keys.F12;
        private bool _takeScreenshot = false;

        public Keys ScreenshotKey
        {
            get { return _screenshotKey; }
            set { _screenshotKey = value; }
        }

        public string FolderPath
        {
            get { return _folderPath; }
            set
            {
                _folderPath = (value ?? "").Trim();
                try
                {

                    if (!string.IsNullOrEmpty(_folderPath) && !Directory.Exists(_folderPath))
                    {
                        Directory.CreateDirectory(_folderPath);
                    }
                }
                catch
                {
                    _folderPath = "";
                }
            }
        }

        public ScreenshotCapturer(FlaiGame game)
            : base(game.Services)
        {
            game.Components.Add(this);
        }

        public override void Update(UpdateContext updateContext)
        {
            if (updateContext.InputState.IsNewKeyPress(_screenshotKey) && !string.IsNullOrEmpty(_folderPath) && Directory.Exists(_folderPath))
            {
                _takeScreenshot = true;
            }
        }

        public override void Draw(GraphicsContext graphicsContext)
        {
            if (_takeScreenshot)
            {
                this.TakeScreenShot();
                _takeScreenshot = false;
            }
        }

        private void TakeScreenShot()
        {
            Texture2D screenshotTexture = this.GetBackBufferTexture();
            string path = this.GetValidPath();

            screenshotTexture.SaveAsPng(path);
        }

        private Texture2D GetBackBufferTexture()
        {
            Texture2D screenshotTexture = new Texture2D(base.GraphicsDevice, base.GraphicsDevice.Viewport.Width, base.GraphicsDevice.Viewport.Height);
            int[] data = new int[screenshotTexture.Width * screenshotTexture.Height];
            base.GraphicsDevice.GetBackBufferData(data);

            screenshotTexture.SetData(data);

            return screenshotTexture;
        }

        // generates lot's of garbage but whatever
        private string GetValidPath()
        {
            string dateString = DateTime.Now.ToShortDateString().Replace('.', '-');
            int index = 1;

            string filePath = _folderPath + "\\" + dateString + "_";
            string fileName = "";
            while (File.Exists((fileName = filePath + index.ToString("000000") + ".png")))
            {
                index++;
            }

            return fileName;
        }
    }
}

#endif