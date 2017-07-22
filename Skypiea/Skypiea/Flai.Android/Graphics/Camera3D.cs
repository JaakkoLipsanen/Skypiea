using Microsoft.Xna.Framework;

namespace Flai.Graphics
{
    public class Camera3D : ICamera3D
    {
        #region Fields

        private bool _isViewDirty = true;
        private bool _isProjectionDirty = true;

        private float _nearPlane;
        private float _farPlane;
        private float _aspectRatio;
        private float _fieldOfView;

        private readonly BoundingFrustum _boundingFrustum = new BoundingFrustum(Matrix.Identity);

        private Vector3 _position;
        private Vector3 _direction;
        private Vector3 _up;

        private Matrix _projection;
        private Matrix _view;

        #endregion

        #region Properties

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    _isViewDirty = true;
                }
            }
        }

        public Vector3 Direction
        {
            get { return _direction; }
            set
            {
                if (value != Vector3.Zero)
                {
                    Vector3 normalizedValue = Vector3.Normalize(value);
                    if (_direction != normalizedValue)
                    {
                        _direction = normalizedValue;
                        _isViewDirty = true;
                    }
                }
            }
        }

        public Vector3 Up
        {
            get { return _up; }
            set
            {
                if (value != Vector3.Zero)
                {
                    Vector3 normalizedValue = Vector3.Normalize(value);
                    if (_up != normalizedValue)
                    {
                        _up = normalizedValue;
                        _isViewDirty = true;
                    }
                }
            }
        }

        public Matrix Projection
        {
            get
            {
                if (_isProjectionDirty)
                {
                    this.CreateProjection();
                }

                return _projection;
            }
        }

        public Matrix View
        {
            get
            {
                if (_isViewDirty)
                {
                    this.CreateView();
                }

                return _view;
            }
        }

        public BoundingFrustum BoundingFrustum
        {
            get
            {
                if (_isViewDirty)
                {
                    this.CreateView();
                }

                if (_isProjectionDirty)
                {
                    this.CreateProjection();
                }

                return _boundingFrustum;
            }
        }

        public float NearPlane
        {
            get { return _nearPlane; }
            set
            {
                if(_nearPlane != value)
                {
                    _nearPlane = value;
                    _isProjectionDirty = true;
                }
            }
        }

        public float FarPlane
        {
            get { return _farPlane; }
            set
            {
                if(_farPlane != value)
                {
                    _farPlane = value;
                    _isProjectionDirty = true;
                }
            }
        }

        public float AspectRatio
        {
            get { return _aspectRatio; }
            set
            {
                if (_aspectRatio != value)
                {
                    _aspectRatio = value;
                    _isProjectionDirty = true;
                }
            }
        }

        public float FieldOfView
        {
            get { return _fieldOfView; }
            set
            {
                if (_fieldOfView != value)
                {
                    _fieldOfView = value;
                    _isProjectionDirty = true;
                }
            }
        }

        #endregion

        public Camera3D(Vector3 position, Vector3 direction, Vector3 up, float aspectRatio)
            : this(position, direction, up, aspectRatio, MathHelper.ToRadians(60), 0.1f, 1000f)
        {
        }

        public Camera3D(Vector3 position, Vector3 direction, Vector3 up, float aspectRatio, float fieldOfView, float nearPlane, float farPlane)
        {
            this.Position = position;
            this.Direction = direction;
            this.Up = up;

            this.AspectRatio = aspectRatio;
            this.FieldOfView = fieldOfView;
            this.NearPlane = nearPlane;
            this.FarPlane = farPlane;
        }

        public Matrix CreateBillboardMatrix(Vector3 objectPosition)
        {
            return Matrix.CreateBillboard(objectPosition, _position, _up, null);
        }

        private void CreateProjection()
        {
            _projection = Matrix.CreatePerspectiveFieldOfView(
                _fieldOfView,
                _aspectRatio,
                _nearPlane,
                _farPlane);

            _isProjectionDirty = false;
        }

        private void CreateView()
        {
            _view = Matrix.CreateLookAt(
                _position,
               _position + _direction,
                _up);

            _boundingFrustum.Matrix = _view * _projection;
            _isViewDirty = false;
        }
    }
}
