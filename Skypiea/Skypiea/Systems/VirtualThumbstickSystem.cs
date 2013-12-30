using Flai;
using Flai.CBES.Systems;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Prefabs;
using Skypiea.Settings;

namespace Skypiea.Systems
{
    public class VirtualThumbstickSystem : EntitySystem
    {
        private VirtualThumbstick _movementThumbstick;
        private VirtualThumbstick _rotationThumbstick;
        private bool _hasGameEnded = false;

        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PreUpdate; }
        }

        protected override void PreInitialize()
        {
            this.CreateThumbsticks(out _movementThumbstick, out _rotationThumbstick);

            this.EntityWorld.CreateEntityFromPrefab<VirtualThumbstickPrefab>(EntityNames.MovementThumbStick, _movementThumbstick);
            this.EntityWorld.CreateEntityFromPrefab<VirtualThumbstickPrefab>(EntityNames.RotationThumbStick, _rotationThumbstick);

            this.EntityWorld.SubscribeToMessage<GameOverMessage>(message => _hasGameEnded = true);
        }

        protected override void Update(UpdateContext updateContext)
        {
            if (!_hasGameEnded)
            {
                _movementThumbstick.Update(updateContext);
                _rotationThumbstick.Update(updateContext);
            }
        }

        private void CreateThumbsticks(out VirtualThumbstick movementThumbstick, out VirtualThumbstick rotationThumbstick)
        {
            SkypieaSettingsManager settingsManager = FlaiGame.Current.Services.Get<SkypieaSettingsManager>();
            Size screenSize = FlaiGame.Current.ScreenSize;
            if (settingsManager.Settings.ThumbstickStyle == ThumbstickStyle.Fixed)
            {
                movementThumbstick = VirtualThumbstick.CreateFixed(new Vector2(120f, screenSize.Height - 120f));
                rotationThumbstick = VirtualThumbstick.CreateFixed(new Vector2(screenSize.Width - 120f, screenSize.Height - 120f));
            }
            else
            {
                const float OffsetFromTop = 80;
                movementThumbstick = VirtualThumbstick.CreateRelative(new RectangleF(0, OffsetFromTop, screenSize.Width / 2f, screenSize.Height - OffsetFromTop));
                rotationThumbstick = VirtualThumbstick.CreateRelative(new RectangleF(screenSize.Width / 2f, OffsetFromTop, screenSize.Width / 2f, screenSize.Height - OffsetFromTop));
            }
        }
    }
}
