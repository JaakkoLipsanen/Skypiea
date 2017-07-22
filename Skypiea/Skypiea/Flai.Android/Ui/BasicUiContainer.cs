
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Flai.Graphics;

namespace Flai.Ui
{
    public class BasicUiContainer : IUiContainer
    {
        private readonly List<UiObject> _uiObjects = new List<UiObject>();

        public int Count
        {
            get { return _uiObjects.Count; }
        }

        public T FindByID<T>(string id)
            where T : UiObject
        {
            for (int i = 0; i < _uiObjects.Count; i++)
            {
                if (_uiObjects[i].ID == id)
                {
                    return (T) _uiObjects[i];
                }
            }

            throw new ArgumentException("id");
        }

        public IEnumerable<UiObject> FindAllWithTag(object tag)
        {
            return this.FindAllWithTag<UiObject>(tag);
        }

        public IEnumerable<T> FindAllWithTag<T>(object tag)
           where T : UiObject
        {
            for (int i = 0; i < _uiObjects.Count; i++)
            {
                if (object.Equals(_uiObjects[i].Tag, tag))
                {
                    yield return (T)_uiObjects[i];
                }
            }
        }

        // Search child elements (cant be done currently since "child elements" are not implemented in UiObject)
        public IEnumerable<T> FindAll<T>()
            where T : UiObject
        {
            foreach (UiObject uiObject in _uiObjects)
            {
                T obj = uiObject as T;
                if (obj != null)
                {
                    yield return obj;
                }
            }
        }

        public IEnumerable<T> FindAll<T>(object tag)
           where T : UiObject
        {
            foreach (UiObject uiObject in _uiObjects)
            {
                T obj = uiObject as T;
                if (obj != null && obj.Tag == tag)
                {
                    yield return obj;
                }
            }
        }

        public T FindFirst<T>()
            where T : UiObject
        {
            foreach (UiObject uiObject in _uiObjects)
            {
                T obj = uiObject as T;
                if (obj != null)
                {
                    return obj;
                }
            }

            throw new ArgumentException(string.Format("No {0} found", typeof(T).Name));
        }

        public T FindFirst<T>(object tag)
            where T : UiObject
        {
            foreach (UiObject uiObject in _uiObjects)
            {
                T obj = uiObject as T;
                if (obj != null && obj.Tag == tag)
                {
                    return obj;
                }
            }

            throw new ArgumentException(string.Format("No {0} found with tag {1}", typeof(T).Name, tag));
        }

        public bool FindFirst<T>(out T foundUiObject)
            where T : UiObject
        {
            foreach (UiObject uiObject in _uiObjects)
            {
                T obj = uiObject as T;
                if (obj != null)
                {
                    foundUiObject = obj;
                    return true;
                }
            }

            foundUiObject = default(T);
            return false;
        }

        public bool FindFirst<T>(object tag, out T foundUiObject)
            where T : UiObject
        {
            foreach (UiObject uiObject in _uiObjects)
            {
                T obj = uiObject as T;
                if (obj != null && obj.Tag == tag)
                {
                    foundUiObject = obj;
                    return true;
                }
            }

            foundUiObject = default(T);
            return false;
        }

        public bool Remove(UiObject uiObject)
        {
            return _uiObjects.Remove(uiObject);
        }

        public void Clear()
        {
            _uiObjects.Clear();
        }

        #region IUIContainer Members

        public T Add<T>(T uiObject)
            where T : UiObject
        {
            _uiObjects.Add(uiObject);
            return uiObject;
        }

        public void Add(UiObject uiObject, params UiObject[] uiObjects)
        {
            _uiObjects.Add(uiObject);
            foreach (UiObject uiObj in uiObjects)
            {
                _uiObjects.Add(uiObj);
            }
        }

        public void Update(UpdateContext updateContext)
        {
            foreach (UiObject uiObject in _uiObjects.Where(uiObject => uiObject.Enabled))
            {
                uiObject.Update(updateContext);
            }
        }

        public void Draw(GraphicsContext graphicsContext)
        {
            this.Draw(graphicsContext, false);
        }

        public void Draw(GraphicsContext graphicsContext, bool spriteBatchHasBegun)
        {
            if (!spriteBatchHasBegun)
            {
                graphicsContext.SpriteBatch.Begin();
            }

            foreach (UiObject uiObject in _uiObjects.Where(uiObject => uiObject.Visible))
            {
                uiObject.Draw(graphicsContext);
            }

            if (!spriteBatchHasBegun)
            {
                graphicsContext.SpriteBatch.End();
            }
        }

        #endregion

        #region IEnumerable<UIObject> Members

        public IEnumerator<UiObject> GetEnumerator()
        {
            return _uiObjects.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _uiObjects.GetEnumerator();
        }

        #endregion
    }
}
