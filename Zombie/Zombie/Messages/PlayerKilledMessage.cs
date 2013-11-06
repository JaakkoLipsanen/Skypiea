using Flai.CBES;
using Zombie.Components;

namespace Zombie.Messages
{
    public class PlayerKilledMessage : Message
    {
        public PlayerInfoComponent PlayerInfo { get; private set; }
        public PlayerKilledMessage(PlayerInfoComponent playerInfo)
        {
            this.PlayerInfo = playerInfo;
        }

    }
}
