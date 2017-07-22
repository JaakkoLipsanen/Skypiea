using Flai.CBES;
using Skypiea.Components;

namespace Skypiea.Messages
{
    public class PlayerKilledMessage : PoolableMessage
    {
        public CPlayerInfo PlayerInfo { get; private set; }
        public PlayerKilledMessage Initialize(CPlayerInfo playerInfo)
        {
            this.PlayerInfo = playerInfo;
            return this;
        }

        protected internal override void Cleanup()
        {
            this.PlayerInfo = null;
        }
    }
}
