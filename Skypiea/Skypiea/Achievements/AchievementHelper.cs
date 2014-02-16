
using Flai.Achievements;
using Flai.DataStructures;
using Skypiea.Achievements.Trackers;
using System.Collections.Generic;
using System.IO.IsolatedStorage;

namespace Skypiea.Achievements
{
    public static class AchievementHelper
    {
        private const string AchievementFilePath = "achievements.dat";
        public static AchievementManager CreateAchivementManager()
        {
            AchievementManager achievementManager = new AchievementManager(AchievementHelper.AchievementFilePath, AchievementHelper.CreateAchievements());
            AchievementHelper.CreateAchievementGroups(achievementManager);
            return achievementManager;
        }

        private static IEnumerable<Achievement> CreateAchievements()
        {
            CreateAchievementTrackerDelegate persistentKillZombieTrackerCreator = (am, ew, achievement) => new PersistentKillZombiesTracker(am, ew, achievement.Name);
            PassiveSkill zombieKillReward = new PassiveSkill("Player shoots faster", passiveStats => passiveStats.FireRateMultiplier += 0.1f);

            Bag<Achievement> achievements = new Bag<Achievement>();
            
            // "important"
            AchievementHelper.CreatePersistentKillingAchievements(achievements);
            AchievementHelper.CreatePersistentRunningAchievements(achievements);
            AchievementHelper.CreateScoreAchievements(achievements);

            // "kill XXX"
            AchievementHelper.CreateKillInTimeAchievements(achievements);
            AchievementHelper.CreateKillRushersWhileRushingAchievements(achievements);
            AchievementHelper.CreateKillWithInvulnerabilityAchievements(achievements);
            AchievementHelper.CreateKillWithSingleRocketAchievments(achievements);
            AchievementHelper.CreateKillGoldenGoblinsAchievements(achievements);

            // "survive"
            AchievementHelper.CreateSurviveWithoutKillingAchievements(achievements);
            AchievementHelper.CreateSurviveWithoutMovingAchievements(achievements);

            // misc
            AchievementHelper.CreateLivesAtAnyPointOfGameAchievements(achievements);
            AchievementHelper.CreateSpendLaserWithoutKillingAchievements(achievements);

            // golden goblin

            // not in atm
            AchievementHelper.CreateIAPAchievements(achievements);

            return achievements;
        }

        #region Survive Without Moving

        private static void CreateSurviveWithoutMovingAchievements(ICollection<Achievement> achievements)
        {
            const float TargetTime = 120;
            CreateAchievementTrackerDelegate trackerCreator = (am, ew, achievement) => new SurviveWithoutMovingTracker(am, ew, achievement.Name, TargetTime);
            PassiveSkill reward = PassiveHelper.CreateSpawnWithRandomWeaponPassive();

            achievements.Add(new Achievement("Immovable Object", "Survive for 120 seconds without moving", "Stay still for 120 seconds", new BooleanProgression(false)) { Tag = new AchievementInfo(trackerCreator, reward) });
        }

        #endregion

        #region Spend All Laser Ammo Without Killing

        private static void CreateSpendLaserWithoutKillingAchievements(ICollection<Achievement> achievements)
        {
            PassiveSkill reward = PassiveHelper.CreateZombieBirthdayPartyPassive();
            achievements.Add(new Achievement("Waste of the Year", "Spend all ammo in laser without killing anyone", "Use laser without killing anyone", new BooleanProgression(false))
            {
                Tag = new AchievementInfo((am, ew, a) => new SpendLaserAmmoWithoutHittingTracker(am, ew, a.Name), reward)
            });
        }

        #endregion

        #region Kill X With Single Rocket Bullet

        private static void CreateKillWithSingleRocketAchievments(ICollection<Achievement> achievements)
        {
            const int KillsNeeded = 8;
            PassiveSkill reward = PassiveHelper.CreateChanceForZombieExplodePassive(0.01f);
            achievements.Add(new Achievement("Pyromaniac", "Kill at least 8 enemies with a single rocket explosion", "Kill 8 zombies with a rocket", new BooleanProgression(false))
            {
                Tag = new AchievementInfo((am, ew, a) => new KillEnemiesWithSingleRocket(am, ew, a.Name, KillsNeeded), reward)
            });
        }

        #endregion

        #region IAP

        private static void CreateIAPAchievements(ICollection<Achievement> achievements)
        {
           // PassiveSkill reward = PassiveHelper.CreateWaterBlasterPassive();
           // achievements.Add(new Achievement("Elite Supporter", "Support the development of the game", new BooleanProgression(true)) { Tag = new AchievementInfo(null, reward) });
        }

        #endregion

        #region Score XXXX

        private static void CreateScoreAchievements(ICollection<Achievement> achievements)
        {
            PassiveSkill reward = PassiveHelper.CreateScoreMultiplierPassive(0.075f);
            achievements.Add(new Achievement("Novice", "Score 20000 in a single game", new BooleanProgression(false)) { Tag = new AchievementInfo((am, ew, a) => new ScoreTracker(am, ew, a.Name, 20000), PassiveHelper.CreateScoreMultiplierPassive(0.01f)) });
            achievements.Add(new Achievement("Master", "Score 50000 in a single game", new BooleanProgression(false)) { Tag = new AchievementInfo((am, ew, a) => new ScoreTracker(am, ew, a.Name, 50000), PassiveHelper.CreateScoreMultiplierPassive(0.02f)) });
            achievements.Add(new Achievement("Legend", "Score 100000 in a single game", new BooleanProgression(false)) { Tag = new AchievementInfo((am, ew, a) => new ScoreTracker(am, ew, a.Name, 100000), PassiveHelper.CreateScoreMultiplierPassive(0.03f)) });
            achievements.Add(new Achievement("Divine", "Score 200000 in a single game", new BooleanProgression(false)) { Tag = new AchievementInfo((am, ew, a) => new ScoreTracker(am, ew, a.Name, 200000), PassiveHelper.CreateScoreMultiplierPassive(0.05f)) });
        }

        #endregion

        #region Kill With Invulnerability Booster

        private static void CreateKillWithInvulnerabilityAchievements(ICollection<Achievement> achievements)
        {
            const int Kills = 150;
            CreateAchievementTrackerDelegate trackerCreator = (am, ew, a) => new KillWithInvulnerabilityBoosterTracker(am, ew, a.Name, Kills);
            PassiveSkill reward = PassiveHelper.CreateChanceToKillEveryoneOnDeathPassive(0.25f);

            // actually it is "kill 100 zombies with a single invulnerability booster", but since it's very unlikely that anyone kills 100 zombies while being on "death-invulnerability",
            // lets just use this wording. it's more clear
            achievements.Add(new Achievement("Invincible", "Kill 150 zombies while invulnerable", new BooleanProgression(false)) { Tag = new AchievementInfo(trackerCreator, reward) });
        }

        #endregion

        #region Survive 90s Without Killing

        private static void CreateSurviveWithoutKillingAchievements(ICollection<Achievement> achievements)
        {
            const float TargetTime = 120;
            CreateAchievementTrackerDelegate trackerCreator = (am, ew, achievement) => new SurviveWithoutKillingTracker(am, ew, achievement.Name, TargetTime);
            PassiveSkill reward = PassiveHelper.CreateSpawnWithThreeLivesPassive();

            achievements.Add(new Achievement("Pacifist", "Survive for 120 seconds without killing", "Don't kill for 120 seconds", new BooleanProgression(false)) { Tag = new AchievementInfo(trackerCreator, reward) });
        }

        #endregion

        #region Kill Rushers While Rushing

        private static void CreateKillRushersWhileRushingAchievements(ICollection<Achievement> achievements)
        {
            CreateAchievementTrackerDelegate trackerCreator = (am, ew, a) => new KillRushersWhileRushingTracker(am, ew, a.Name);
            PassiveSkill reward = PassiveHelper.CreateFireRatePassive(0.075f);

            achievements.Add(new Achievement("Peace Officer", "Kill 25 rushers while they are rushing", "Kill 25 rushers while rushing", new IntegerProgression(0, 25)) { Tag = new AchievementInfo(trackerCreator, reward) });
            achievements.Add(new Achievement("Punisher of Maniacs", "Kill 100 rushers while they are  rushing", "Kill 100 rushers while rushing", new IntegerProgression(0, 100)) { Tag = new AchievementInfo(trackerCreator, reward) });
            achievements.Add(new Achievement("Exterminator of Lunatics", "Kill 500 rushers while they are  rushing", "Kill 500 rushers while rushing", new IntegerProgression(0, 500)) { Tag = new AchievementInfo(trackerCreator, reward) });
        }

        #endregion

        #region Lives at Any Point of the Game

        private static void CreateLivesAtAnyPointOfGameAchievements(ICollection<Achievement> achievements)
        {
            const int LivesNeeded = 4;
            CreateAchievementTrackerDelegate trackerCreator = (am, ew, a) => new LivesAtAnyPointOfGameTracker(am, ew, a.Name, LivesNeeded);
            PassiveSkill reward = PassiveHelper.CreateChanceToGetTwoLivesOnDrop(0.25f);

            achievements.Add(new Achievement("The Life Harvester", "Have at least 4 lives at any point of a game", "Have 4 lives in a game", new BooleanProgression(false)) { Tag = new AchievementInfo(trackerCreator, reward) });
        }

        #endregion

        #region Killing in Time (10s)

        private static void CreateKillInTimeAchievements(ICollection<Achievement> achievements)
        {
            PassiveSkill reward = PassiveHelper.CreateDropIncreasePassive(0.15f);
            achievements.Add(AchievementHelper.CreateKillInTimeAchievement("Massacre", "Kill 40 zombies in 10 seconds", 40, reward));
            achievements.Add(AchievementHelper.CreateKillInTimeAchievement("Genocide", "Kill 75 zombies in 10 seconds", 75, reward));
            achievements.Add(AchievementHelper.CreateKillInTimeAchievement("Desolation", "Kill 125 zombies in 10 seconds", 125, reward));
        }

        private static Achievement CreateKillInTimeAchievement(string name, string description, int count, PassiveSkill reward)
        {
            const float Time = 10f;
            return new Achievement(name, description, new BooleanProgression(false))
            {
                Tag = new AchievementInfo((am, ew, a) => new KillZombiesInTimeTracker(am, ew, a.Name, count, Time), reward)
            };
        }

        #endregion

        #region Persistent Running

        private static void CreatePersistentRunningAchievements(ICollection<Achievement> achievements)
        {
            CreateAchievementTrackerDelegate trackerCreator = (am, ew, a) => new PersistentRunningTracker(am, ew, a.Name);
            PassiveSkill reward = PassiveHelper.CreateMovementSpeedPassive(0.075f);

            achievements.Add(new Achievement("Jogger", "Run 1500 meters", new FloatProgression(0, 1500)) { Tag = new AchievementInfo(trackerCreator, reward) });
            achievements.Add(new Achievement("Marathoner", "Run 5000 meters", new FloatProgression(0, 5000)) { Tag = new AchievementInfo(trackerCreator, reward) });
            achievements.Add(new Achievement("The Flash", "Run 20000 meters", new FloatProgression(0, 20000)) { Tag = new AchievementInfo(trackerCreator, reward) });
            achievements.Add(new Achievement("Duracell", "Run 50000 meters", new FloatProgression(0, 50000)) { Tag = new AchievementInfo(trackerCreator, reward) });
        }

        #endregion

        #region Persistent Killing

        private static void CreatePersistentKillingAchievements(ICollection<Achievement> achievements)
        {
            CreateAchievementTrackerDelegate trackerCreator = (am, ew, a) => new PersistentKillZombiesTracker(am, ew, a.Name);
            PassiveSkill reward = PassiveHelper.CreateAmmoPassive(0.25f);

            achievements.Add(new Achievement("Sunday Killer", "Kill 500 zombies", new IntegerProgression(0, 500)) { Tag = new AchievementInfo(trackerCreator, reward) });
            achievements.Add(new Achievement("Zombie-nator", "Kill 2500 zombies", new IntegerProgression(0, 2500)) { Tag = new AchievementInfo(trackerCreator, reward) });
            achievements.Add(new Achievement("Reaper of the Dead", "Kill 10000 zombies", new IntegerProgression(0, 10000)) { Tag = new AchievementInfo(trackerCreator, reward) });
            achievements.Add(new Achievement("Titan", "Kill 25000 zombies", new IntegerProgression(0, 25000)) { Tag = new AchievementInfo(trackerCreator, reward) });
            achievements.Add(new Achievement("God", "Kill 50000 zombies", new IntegerProgression(0, 50000)) { Tag = new AchievementInfo(trackerCreator, reward) });
        }

        #endregion

        #region Persistent Kill Golden Goblins

        private static void CreateKillGoldenGoblinsAchievements(ICollection<Achievement> achievements)
        {
            CreateAchievementTrackerDelegate trackerCreator = (am, ew, a) => new KillGoldenGoblinsTracker(am, ew, a.Name);
            PassiveSkill reward = PassiveHelper.CreateGoldenGoblinRadarPassive();

            achievements.Add(new Achievement("Treasure Hunter", "Kill 25 golden goblins", new IntegerProgression(0, 25)) { Tag = new AchievementInfo(trackerCreator, reward) });
        }

        #endregion

        #region Groups

        private static void CreateAchievementGroups(AchievementManager achievementManager)
        {
            achievementManager.AddGroup("ZombieKills", AchievementHelper.CreateAchievementGroup(achievementManager, "Sunday Killer", "Zombie-nator", "Reaper of the Dead", "Titan", "God"));
            achievementManager.AddGroup("Running", AchievementHelper.CreateAchievementGroup(achievementManager, "Jogger", "Marathoner", "The Flash", "Duracell"));
            achievementManager.AddGroup("KillsInTime", AchievementHelper.CreateAchievementGroup(achievementManager, "Massacre", "Genocide", "Desolation"));
            achievementManager.AddGroup("KillRushersWhileRushing", AchievementHelper.CreateAchievementGroup(achievementManager, "Peace Officer", "Punisher of Maniacs", "Exterminator of Lunatics"));
            achievementManager.AddGroup("Score", AchievementHelper.CreateAchievementGroup(achievementManager, "Novice", "Master", "Legend", "Divine"));
        }

        private static AchievementGroup CreateAchievementGroup(AchievementManager achievementManager, params string[] achievementNames)
        {
            Achievement[] achievements = new Achievement[achievementNames.Length];
            for (int i = 0; i < achievements.Length; i++)
            {
                achievements[i] = achievementManager[achievementNames[i]];
            }

            return new AchievementGroup(achievements);
        }

        #endregion

        public static void ResetAchievements()
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(AchievementHelper.AchievementFilePath))
                {
                    isolatedStorage.DeleteFile(AchievementHelper.AchievementFilePath);
                }
            }
        }
    }

    public static class PassiveHelper
    {
        public static PassiveSkill CreateFireRatePassive(float fireRateReward)
        {
            return new PassiveSkill("Player shoots faster", passiveStats => passiveStats.FireRateMultiplier += fireRateReward);
        }

        public static PassiveSkill CreateMovementSpeedPassive(float movementSpeedReward)
        {
            return new PassiveSkill("Player runs faster", passiveStats => passiveStats.MovementSpeedMultiplier += movementSpeedReward);
        }

        public static PassiveSkill CreateAmmoPassive(float ammoMultiplier)
        {
            return new PassiveSkill("Weapons have more ammo", passiveStats => passiveStats.AmmoMultiplier += ammoMultiplier);
        }

        public static PassiveSkill CreateDropIncreasePassive(float dropIncreaseMultiplier)
        {
            return new PassiveSkill("Increased drop rate", passiveStats => passiveStats.DropIncreaseMultiplier += dropIncreaseMultiplier);
        }

        public static PassiveSkill CreateChanceToKillEveryoneOnDeathPassive(float chance)
        {
            return new PassiveSkill("Chance to kill all zombies on death", passiveStats => passiveStats.ChanceToKillEveryoneOnDeath += chance);
        }

        public static PassiveSkill CreateChanceForZombieExplodePassive(float chance)
        {
            return new PassiveSkill("Chance for zombies to explode when killed", passiveStats => passiveStats.ChanceForZombieExplodeOnDeath += chance);
        }

        public static PassiveSkill CreateScoreMultiplierPassive(float increase)
        {
            return new PassiveSkill("Increased score per kill", passiveStats => passiveStats.ScoreMultiplier += increase);
        }

        public static PassiveSkill CreateSpawnWithThreeLivesPassive()
        {
            return new PassiveSkill("Spawn with 3 lives", passiveStats => passiveStats.SpawnWithThreeLives = true);
        }

        public static PassiveSkill CreateSpawnWithRandomWeaponPassive()
        {
            return new PassiveSkill("Spawn with random weapon", passiveStats => passiveStats.SpawnWithRandomWeapon = true);
        }

        public static PassiveSkill CreateZombieBirthdayPartyPassive()
        {
            return new ToggleablePassiveSkill("Zombie birthday party", passiveStats => passiveStats.ZombieBirthdayParty = true);
        }

        public static PassiveSkill CreateWaterBlasterPassive()
        {
            return new ToggleablePassiveSkill("New Weapon: Waterblaster", passiveStats => passiveStats.ChanceForWaterBlaster = true);
        }

        public static PassiveSkill CreateChanceToGetTwoLivesOnDrop(float chance)
        {
            return new PassiveSkill("Chance to get two lives on a single life drop", passiveStats => passiveStats.ChanceToGetTwoLivesOnDrop += chance);
        }

        public static PassiveSkill CreateGoldenGoblinRadarPassive()
        {
            return new PassiveSkill("See golden goblins in the radar", passiveStats => passiveStats.SeeGoldenGoblinsOnRadar = true);
        }
    }
}
