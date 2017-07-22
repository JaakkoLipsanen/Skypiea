using Microsoft.Xna.Framework;

namespace Flai.Graphics
{
    public interface ICamera3D
    {
        Matrix View { get; }
        Matrix Projection { get; }
    }
}
