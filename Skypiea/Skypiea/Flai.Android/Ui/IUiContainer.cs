
using System.Collections.Generic;
using Flai.Graphics;

namespace Flai.Ui
{
    public interface IUiContainer : IEnumerable<UiObject>
    {
        T Add<T>(T uiObject) where T : UiObject;

        void Update(UpdateContext updateContext);
        void Draw(GraphicsContext graphicsContext);
    }
}
