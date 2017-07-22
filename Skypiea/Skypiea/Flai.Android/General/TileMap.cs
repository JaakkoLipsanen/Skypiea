using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Flai.General
{
#if WINDOWS_PHONE
    public interface ITileMap<T> : IEnumerable<T>
#else
    public interface ITileMap<out T> : IEnumerable<T>
#endif
    {
        int Width { get; }
        int Height { get; }

        bool IsInsideBounds(int x, int y);

        T this[int x, int y] { get; }
        T this[Vector2i index] { get; }
    }

    public interface IEditableTileMap<T> : ITileMap<T>
    {
        new T this[int x, int y] { get; set; }
        new T this[Vector2i index] { get; set; }
    }

    #region TileMap<T>

    public class TileMap<T> : ITileMap<T>
    {
        private readonly T _defaultValue;
        protected readonly T[] _tiles;

        protected readonly int _width;
        protected readonly int _height;

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public TileMap(T[] tiles, int width, int height)
            : this(tiles, width, height, default(T))
        {

        }

        public TileMap(T[] tiles, int width, int height, T defaultValue)
        {
            Ensure.True(tiles.Length != 0);
            Ensure.True(tiles.Length == width * height);

            _tiles = tiles;
            _width = width;
            _height = height;
            _defaultValue = defaultValue;
        }

        public bool IsInsideBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _width && y < _height;
        }

        public T this[Vector2i index]
        {
            get { return this[index.X, index.Y]; }
        }

        public T this[int x, int y]
        {
            get { return this.IsInsideBounds(x, y) ? _tiles[x + y * _width] : _defaultValue; }
        }

        #region Implementation of IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)_tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tiles.GetEnumerator();
        }

        #endregion
    }

    #endregion

    #region ReadOnlyTileMap<T>

    public class ReadOnlyTileMap<T> : ITileMap<T>
    {
        private readonly ITileMap<T> _tileMap;

        public int Width
        {
            get { return _tileMap.Width; }
        }

        public int Height
        {
            get { return _tileMap.Height; }
        }

        public ReadOnlyTileMap(ITileMap<T> tileLayer)
        {
            _tileMap = tileLayer;
        }

        public T this[int x, int y]
        {
            get { return _tileMap[x, y]; }
        }

        public T this[Vector2i index]
        {
            get { return _tileMap[index]; }
        }

        public bool IsInsideBounds(int x, int y)
        {
            return _tileMap.IsInsideBounds(x, y);
        }

        #region Implementation of IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return _tileMap.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tileMap.GetEnumerator();
        }

        #endregion
    }

    #endregion

    #region EditableTileMap

    public class EditableTileMap<T> : IEditableTileMap<T>
    {
        protected readonly T _defaultValue;
        protected T[] _tiles;

        protected int _width;
        protected int _height;

        public int Width
        {
            get { return _width; }
        }

        public int Height
        {
            get { return _height; }
        }

        public EditableTileMap(T[] tiles, int width, int height)
            : this(tiles, width, height, default(T))
        {
        }

        public EditableTileMap(T[] tiles, int width, int height, T defaultValue)
        {
            Ensure.True(tiles.Length != 0);
            Ensure.True(tiles.Length == width * height);

            _tiles = tiles;
            _width = width;
            _height = height;
            _defaultValue = defaultValue;
        }

        public bool IsInsideBounds(int x, int y)
        {
            return x >= 0 && y >= 0 && x < _width && y < _height;
        }

        public bool IsInsideBounds(Vector2i v)
        {
            return this.IsInsideBounds(v.X, v.Y);
        }

        public T this[Vector2i index]
        {
            get { return this[index.X, index.Y]; }
            set { this[index.X, index.Y] = value; }
        }

        public T this[int x, int y]
        {
            get
            {
                return this.IsInsideBounds(x, y) ? _tiles[x + y * _width] : _defaultValue;
            }
            set
            {
                Ensure.True(this.IsInsideBounds(x, y));
                _tiles[x + y * _width] = value;
            }
        }

        public void MoveTiles(Rectangle tileArea, Vector2i offset)
        {
            Ensure.True(tileArea.Left >= 0 && tileArea.Top >= 0 && tileArea.Right <= _width && tileArea.Bottom <= _height);
            Rectangle newArea = new Rectangle(tileArea.X + offset.X, tileArea.Y + offset.Y, tileArea.Width, tileArea.Height);

            // Find startX and endX
            int xDir = (offset.X > 0) ? -1 : 1;
            int startX = FlaiMath.Min(this.Width - 1, newArea.Right);
            int endX = FlaiMath.Max(0, newArea.Left);
            if (offset.X <= 0)
            {
                Common.SwapReferences(ref startX, ref endX);
            }

            // Find startY and endY
            int yDir = (offset.Y > 0) ? -1 : 1;
            int startY = FlaiMath.Min(this.Height - 1, newArea.Bottom);
            int endY = FlaiMath.Max(0, newArea.Top);
            if (offset.Y <= 0)
            {
                Common.SwapReferences(ref startY, ref endY);
            }

            // Okay.. this should check if the values are valid
            if (((xDir >= 0) != (startX <= endX)) ||
                ((yDir >= 0) != (startY <= endY)))
            {
                return;
            }

            for (int y = startY; y != (endY + yDir); y += yDir)
            {
                for (int x = startX; x != (endX + xDir); x += xDir)
                {
                    Vector2i sourceIndex = new Vector2i(x, y) - offset;

                    _tiles[x + y * _width] = this.IsInsideBounds(sourceIndex) ? this[sourceIndex] : default(T);
                    if (this.IsInsideBounds(sourceIndex) && !newArea.Contains(sourceIndex))
                    {
                        _tiles[sourceIndex.X + sourceIndex.Y * _width] = default(T);
                    }
                }
            }
        }

        #region Implementation of IEnumerable<T>

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)_tiles.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _tiles.GetEnumerator();
        }

        #endregion

        public void Clear()
        {
            // to _defaultValue?!?
            Array.Clear(_tiles, 0, _tiles.Length);
        }
    }

    #endregion

    #region ResizableTileMap<T>

    public class ResizableTileMap<T> : EditableTileMap<T>
    {
        public ResizableTileMap(T[] tiles, int width, int height)
            : base(tiles, width, height)
        {
        }

        public ResizableTileMap(T[] tiles, int width, int height, T defaultValue)
            : base(tiles, width, height, defaultValue)
        {
        }

        // TODO: _defaultValue is not implemented in these
        // TODO: Not very fast, could be done better (for example have only one array that expands, never gets smaller)
        public void Resize(int newWidth, int newHeight)
        {
            Ensure.True(newWidth > 0 && newHeight > 0);

            T[] newTiles = new T[newWidth * newHeight];
            int yEnd = Math.Min(_height, newHeight);
            int xEnd = Math.Min(_width, newWidth);
            for (int y = 0; y < yEnd; y++)
            {
                for (int x = 0; x < xEnd; x++)
                {
                    newTiles[x + y * newWidth] = _tiles[x + y * _width];
                }
            }

            _width = newWidth;
            _height = newHeight;
            _tiles = newTiles;
        }

    }

    #endregion
}
