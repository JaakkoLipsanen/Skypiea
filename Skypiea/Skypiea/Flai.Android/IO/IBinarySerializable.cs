using System.IO;

namespace Flai.IO
{
    public interface IBinarySerializable
    {
        void Write(BinaryWriter writer);
        void Read(BinaryReader reader);
    }
}
