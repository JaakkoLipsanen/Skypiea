using System.Collections;
using System.Collections.Generic;

namespace Flai.Graphics
{
    public interface ICameraManager2D : IEnumerable<KeyValuePair<string, ICamera2D>>
    {
        Dictionary<string, ICamera2D>.ValueCollection Cameras { get; }

        ICamera2D ActiveCamera { get; }
        string ActiveCameraName { get; }

        void AddCamera(string cameraName, ICamera2D camera);
        void AddCamera(string cameraName, ICamera2D camera, bool setAsActive);

        ICamera2D GetCamera(string cameraName);
        bool TryGetCamera(string cameraName, out ICamera2D camera);
        bool ContainsCamera(string cameraName);

        bool RemoveCamera(string cameraName);
        void RemoveAll();
        ICamera2D SetActiveCamera(string cameraName);
    }

    public class CameraManager2D : ICameraManager2D
    {
        private readonly Dictionary<string, ICamera2D> _cameraDictionary = new Dictionary<string, ICamera2D>();
        private string _activeCameraName = "";

        public Dictionary<string, ICamera2D>.ValueCollection Cameras
        {
            get { return _cameraDictionary.Values; }
        }

        public ICamera2D ActiveCamera
        {
            get { return string.IsNullOrEmpty(_activeCameraName) ? null : _cameraDictionary[_activeCameraName]; }
        }

        public string ActiveCameraName
        {
            get { return _activeCameraName == "" ? null : _activeCameraName; }
        }

        public CameraManager2D()
        {
        }

        public void AddCamera(string cameraName, ICamera2D camera)
        {
            this.AddCamera(cameraName, camera, true);
        }

        public void AddCamera(string cameraName, ICamera2D camera, bool setAsActive)
        {
            _cameraDictionary.Add(cameraName, camera);
            if (setAsActive)
            {
                this.SetActiveCamera(cameraName);
            }
        }

        public ICamera2D GetCamera(string cameraName)
        {
            return _cameraDictionary[cameraName];
        }

        public bool TryGetCamera(string cameraName, out ICamera2D camera)
        {
            return _cameraDictionary.TryGetValue(cameraName, out camera);
        }

        public bool ContainsCamera(string cameraName)
        {
            return _cameraDictionary.ContainsKey(cameraName);
        }

        public bool RemoveCamera(string cameraName)
        {
            return _cameraDictionary.Remove(cameraName);
        }

        public void RemoveAll()
        {
            _cameraDictionary.Clear();
        }

        public ICamera2D SetActiveCamera(string cameraName)
        {
            ICamera2D camera;
            if (!_cameraDictionary.TryGetValue(cameraName, out camera))
            {
                throw new KeyNotFoundException("cameraName");
            }

            _activeCameraName = cameraName;
            return camera;
        }

        #region IEnumerable<KeyValuePair<string,Camera3D>> Members

        public IEnumerator<KeyValuePair<string, ICamera2D>> GetEnumerator()
        {
            return _cameraDictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _cameraDictionary.GetEnumerator();
        }

        #endregion
    }
}
