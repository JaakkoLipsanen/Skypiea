#if WINDOWS_PHONE

using System;
using Microsoft.Devices.Sensors;

namespace Flai.Misc
{
    public class AccelerometerManager : FlaiService, IAccelerometerManager
    {
        private readonly Accelerometer _accelerometer = new Accelerometer();
        private bool _hasFailed = false;

        public EventHandler<SensorReadingEventArgs<AccelerometerReading>> CurrentValueChanged;

        public SensorState State
        {
            get { return _accelerometer.State; }
        }

        /// <summary>
        /// Note! This is not a direct property getter. There is some logic behind
        /// </summary>
        public AccelerometerReading CurrentValue
        {
            get { return _accelerometer.CurrentValue; }
        }

        public bool HasFailed
        {
            get { return _hasFailed; }
        }

        public bool IsActive
        {
            get { return _accelerometer.State == SensorState.Ready; }
        }

        public AccelerometerManager(FlaiServiceContainer services)
            : base(services)
        {
            _services.Add<IAccelerometerManager>(this);
        }

        public void Start()
        {
            try
            {
                _accelerometer.Start();
                _accelerometer.CurrentValueChanged += (o, e) =>
                    {
                        var handler = this.CurrentValueChanged;
                        if (handler != null)
                        {
                            handler(o, e);
                        }
                    };
            }
            catch (AccelerometerFailedException)
            {
                // The accelerometer could not be started
                _hasFailed = true;
            }
            catch (UnauthorizedAccessException)
            {
                _hasFailed = true;
            }
        }

        public void Stop()
        {
            if (this.IsActive)
            {
                try
                {
                    _accelerometer.Stop();
                }
                catch (AccelerometerFailedException)
                {
                    // Accelerometer could not be stopped
                }
            }
        }
    }
}

#endif