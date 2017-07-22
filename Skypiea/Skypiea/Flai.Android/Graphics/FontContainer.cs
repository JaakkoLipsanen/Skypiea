
using System.Collections;
using System.Collections.Generic;
using Flai.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public class FontContainer : FlaiService, IFontContainer, IEnumerable<SpriteFont>
    {
        private readonly Dictionary<string, SpriteFont> _fonts = new Dictionary<string, SpriteFont>();
        private SpriteFont _defaultFont;

        public SpriteFont DefaultFont
        {
            get { return _defaultFont ?? (_defaultFont = this.InnerGetFont("Default")); }
            set
            {
                Ensure.NotNull(value);
                _defaultFont = value;
            }
        }

        internal FontContainer(FlaiServiceContainer services)
            : base(services)
        {
            _services.Add<IFontContainer>(this);
        }

        public void AddFont(string fontName)
        {
            IContentProvider contentProvider = _services.Get<IContentProvider>();
            if (contentProvider != null)
            {
                this.AddFont(fontName, contentProvider["Fonts"].LoadFont(fontName));
            }
        }

        public void AddFont(string fontName, SpriteFont font)
        {
            _fonts.Add(fontName, font);
        }

        public bool RemoveFont(string fontName)
        {
            return _fonts.Remove(fontName);
        }

        public bool ContainsFont(string fontName)
        {
            return _fonts.ContainsKey(fontName);
        }

        public SpriteFont GetFont(string fontName)
        {
            return this[fontName];
        }

        public SpriteFont this[string fontName]
        {
            get
            {
                if (fontName == "Default")
                {
                    return this.DefaultFont;
                }

                return this.InnerGetFont(fontName);
            }
            set
            {
                _fonts.AddOrSetValue(fontName, value);
            }
        }

        private SpriteFont InnerGetFont(string fontName)
        {
            SpriteFont font;
            if (!_fonts.TryGetValue(fontName, out font))
            {
                this.AddFont(fontName);
                font = _fonts[fontName];
            }

            return font;
        }

        #region IEnumerable<SpriteFont> Members

        public IEnumerator<SpriteFont> GetEnumerator()
        {
            return _fonts.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _fonts.Values.GetEnumerator();
        }

        #endregion
    }
}
