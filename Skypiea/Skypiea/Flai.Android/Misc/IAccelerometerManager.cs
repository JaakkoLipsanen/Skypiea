#if WINDOWS_PHONE

using Microsoft.Devices.Sensors;

namespace Flai.Misc
{
    public interface IAccelerometerManager
    {
        AccelerometerReading CurrentValue { get; }
        SensorState State { get; }
        bool HasFailed { get; }
        bool IsActive { get; }

        void Start();       
        void Stop();
    }
}

#endif