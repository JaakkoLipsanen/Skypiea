using Flai.CBES;
using Skypiea.Components;

namespace Skypiea.Messages
{
    public class PlayerKilledMessage : Message
    {
        public CPlayerInfo CPlayerInfo { get; private set; }
        public PlayerKilledMessage(CPlayerInfo cPlayerInfo)
        {
            this.CPlayerInfo = cPlayerInfo;
        }
    }
}
