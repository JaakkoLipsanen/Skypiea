using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems.Player
{
    public class PlayerDropPickupSystem : ComponentProcessingSystem<CDrop>
    {
        private Entity _player;
        private CWeapon _playerWeapon;
        private CPlayerInfo _playerInfo;
        private IPlayerPassiveStats _passiveStats;

        public PlayerDropPickupSystem()
            : base(Aspect.Any<CWeaponDrop, CLifeDrop>())
        {          
        }

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityNames.Player);
            _playerWeapon = _player.Get<CWeapon>();
            _playerInfo = _player.Get<CPlayerInfo>();
            _passiveStats = this.EntityWorld.Services.Get<IPlayerPassiveStats>();
        }

        public override void Process(UpdateContext updateContext, Entity entity, CDrop velocity)
        {
            const float MinDistance = SkypieaConstants.PixelsPerMeter;
            if (Vector2.Distance(_player.Transform.Position, entity.Transform.Position) < MinDistance)
            {
                if (velocity.DropType == DropType.Weapon)
                {
                    this.OnWeaponDropPicked(entity);
                }
                else
                {
                    this.OnLifeDropPicked(entity);
                }
            }
        }

        private void OnWeaponDropPicked(Entity entity)
        {
            CWeaponDrop weaponDrop = entity.Get<CWeaponDrop>();
            if (weaponDrop.Type == _playerWeapon.Weapon.Type)
            {
                _playerWeapon.Weapon.OnNewInstancePickedUp(); // awful name
            }
            else
            {
                WeaponChangedMessage message = this.EntityWorld.FetchMessage<WeaponChangedMessage>();
                message.Initialize(_playerWeapon.Weapon, weaponDrop.BuildWeapon(_passiveStats.AmmoMultiplier));
                _playerWeapon.Weapon = message.NewWeapon;

                this.EntityWorld.BroadcastMessage(message);
            }

            entity.Delete();
        }

        private void OnLifeDropPicked(Entity entity)
        {
            _playerInfo.AddLife();
            if (Global.Random.NextFromOdds(_passiveStats.ChanceToGetTwoLivesOnDrop))
            {
                _playerInfo.AddLife();
            }

            entity.Delete();
        }
    }
}
