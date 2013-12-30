using System.IO;
using Flai;
using Flai.Achievements;
using Flai.CBES;
using Flai.IO;
using Skypiea.Model;

namespace Skypiea.Achievements
{
    public delegate AchievementTracker CreateAchievementTrackerDelegate(AchievementManager achievementManager, EntityWorld entityWorld, Achievement achievement);
    public delegate void ProcessPassiveStatsDelegate(PlayerPassiveStats playerPassiveStats);

    public class AchievementInfo : IBinarySerializable
    {
        private readonly CreateAchievementTrackerDelegate _createTrackerFunction;
        public PassiveSkill RewardPassiveSkill { get; private set; }

        public bool HasTracker
        {
            get { return _createTrackerFunction != null; }
        }

        public AchievementInfo(CreateAchievementTrackerDelegate createTrackerFunction, PassiveSkill rewardPassiveSkill)
        {
            Ensure.NotNull(rewardPassiveSkill);

            _createTrackerFunction = createTrackerFunction;
            this.RewardPassiveSkill = rewardPassiveSkill;
        }

        public AchievementTracker CreateTracker(AchievementManager achievementManager, EntityWorld entityWorld, Achievement achievement)
        {
            return _createTrackerFunction(achievementManager, entityWorld, achievement);
        }

        void IBinarySerializable.Write(BinaryWriter writer)
        {
            if (this.RewardPassiveSkill is IBinarySerializable)
            {
                this.RewardPassiveSkill.Cast<IBinarySerializable>().Write(writer);
            }
        }

        void IBinarySerializable.Read(BinaryReader reader)
        {
            if (this.RewardPassiveSkill is IBinarySerializable)
            {
                this.RewardPassiveSkill.Cast<IBinarySerializable>().Read(reader);
            }
        }
    }

    public class PassiveSkill
    {
        private readonly ProcessPassiveStatsDelegate _processPassiveStatsAction;
        public string Description { get; private set; }

        public PassiveSkill(string description, ProcessPassiveStatsDelegate processPassiveStatsAction)
        {
            Ensure.NotNull(description);
            Ensure.NotNull(processPassiveStatsAction);

            this.Description = description;
            _processPassiveStatsAction = processPassiveStatsAction;
        }

        public virtual void ProcessPassives(PlayerPassiveStats passiveStats)
        {
            _processPassiveStatsAction(passiveStats);
        }
    }

    public class ToggleablePassiveSkill : PassiveSkill, IBinarySerializable
    {
        public bool IsEnabled { get; set; }
        public ToggleablePassiveSkill(string description, ProcessPassiveStatsDelegate processPassiveStatsAction) 
            : base(description, processPassiveStatsAction)
        {
            this.IsEnabled = true;
        }

        public override void ProcessPassives(PlayerPassiveStats passiveStats)
        {
            if (this.IsEnabled)
            {
                base.ProcessPassives(passiveStats);
            }
        }

        void IBinarySerializable.Write(BinaryWriter writer)
        {
            writer.Write(this.IsEnabled);
        }

        void IBinarySerializable.Read(BinaryReader reader)
        {
            this.IsEnabled = reader.ReadBoolean();
        }
    }
}
