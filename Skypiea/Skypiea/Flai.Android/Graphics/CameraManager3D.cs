using System.Collections;
using System.Collections.Generic;

namespace Flai.Graphics
{
    public interface ICameraManager3D : IEnumerable<KeyValuePair<string, ICamera3D>>
    {
        Dictionary<string, ICamera3D>.ValueCollection Cameras { get; }

        ICamera3D ActiveCamera { get; }
        string ActiveCameraName { get; }

        void AddCamera(string cameraName, ICamera3D camera);
        void AddCamera(string cameraName, ICamera3D camera, bool setAsActive);
        bool RemoveCamera(string cameraName);
        ICamera3D SetActiveCamera(string cameraName);
    }

    public class CameraManager3D : ICameraManager3D
    {
        private readonly Dictionary<string, ICamera3D> _cameraDictionary = new Dictionary<string, ICamera3D>();
        private string _activeCameraName = "";

        public Dictionary<string, ICamera3D>.ValueCollection Cameras
        {
            get { return _cameraDictionary.Values; }
        }

        public ICamera3D ActiveCamera
        {
            get { return string.IsNullOrEmpty(_activeCameraName) ? null : _cameraDictionary[_activeCameraName]; }
        }

        public string ActiveCameraName
        {
            get { return _activeCameraName == "" ? null : _activeCameraName; }
        }

        public void AddCamera(string cameraName, ICamera3D camera)
        {
            this.AddCamera(cameraName, camera, true);
        }

        public void AddCamera(string cameraName, ICamera3D camera, bool setAsActive)
        {
            _cameraDictionary.Add(cameraName, camera);
            if (setAsActive)
            {
                this.SetActiveCamera(cameraName);
            }
        }

        public bool RemoveCamera(string cameraName)
        {
            return _cameraDictionary.Remove(cameraName);
        }

        public ICamera3D SetActiveCamera(string cameraName)
        {
            ICamera3D camera;
            if (!_cameraDictionary.TryGetValue(cameraName, out camera))
            {
                throw new KeyNotFoundException("cameraName");
            }

            _activeCameraName = cameraName;
            return camera;
        }

        #region IEnumerable<KeyValuePair<string,Camera3D>> Members

        public IEnumerator<KeyValuePair<string, ICamera3D>> GetEnumerator()
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
