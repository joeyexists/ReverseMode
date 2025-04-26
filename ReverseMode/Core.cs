using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(ReverseMode.Core), "ReverseMode", "1.1.0", "joeyexists", null)]
[assembly: MelonGame("Little Flag Software, LLC", "Neon White")]

namespace ReverseMode
{
    public class Core : MelonMod
    {
        private static readonly Dictionary<string, int> spawnAngles = new()
        {
            { "TUT_MOVEMENT", 187 },
            { "TUT_SHOOTINGRANGE", 171 },
            { "SLUGGER", 28 },
            { "TUT_FROG", 168 },
            { "TUT_JUMP", 325 },
            { "GRID_TUT_BALLOON", 275 },
            { "TUT_BOMB2", 270 },
            { "TUT_BOMBJUMP", 310 },
            { "TUT_FASTTRACK", 352 },
            { "GRID_PORT", 60 },
            { "GRID_PAGODA", 118 },
            { "TUT_RIFLE", 330 },
            { "TUT_RIFLEJOCK", 359 },
            { "TUT_DASHENEMY", 225 },
            { "GRID_JUMPDASH", 200 },
            { "GRID_SMACKDOWN", 253 },
            { "GRID_MEATY_BALLOONS", 165 },
            { "GRID_FAST_BALLOONS", 180 },
            { "GRID_DRAGON2", 149 },
            { "GRID_DASHDANCE", 68 },
            { "TUT_GUARDIAN", 32 },
            { "TUT_UZI", 90 },
            { "TUT_JUMPER", 270 },
            { "TUT_BOMB", 60 },
            { "GRID_DESCEND", 25 },
            { "GRID_STAMPEROUT", 35 },
            { "GRID_CRUISE", 270 },
            { "GRID_SPRINT", 60 },
            { "GRID_MOUNTAIN", 275 },
            { "GRID_SUPERKINETIC", 290 },
            { "GRID_ARRIVAL", 175 },
            { "FLOATING", 44 },
            { "GRID_HOPHOP", 150 },
            { "GRID_RINGER_TUTORIAL", 265 },
            { "GRID_RINGER_EXPLORATION", 90 },
            { "GRID_HOPSCOTCH", 180 },
            { "GRID_BOOM", 210 },
            { "GRID_SNAKE_IN_MY_BOOT", 5 },
            { "GRID_FLOCK", 180 },
            { "GRID_BOMBS_AHOY", 0 },
            { "GRID_ARCS", 325 },
            { "GRID_APARTMENT", 180 },
            { "TUT_TRIPWIRE", 300 },
            { "GRID_TANGLED", 120 },
            { "GRID_HUNT", 270 },
            { "GRID_CANNONS", 20 },
            { "GRID_FALLING", 90 },
            { "TUT_SHOCKER2", 270 },
            { "TUT_SHOCKER", 50 },
            { "GRID_PREPARE", 100 },
            { "GRID_TRIPMAZE", 215 },
            { "GRID_RACE", 59 },
            { "TUT_FORCEFIELD2", 0 },
            { "GRID_SHIELD", 225 },
            { "SA L VAGE2", 260 },
            { "GRID_VERTICAL", 225 },
            { "GRID_MINEFIELD", 155 },
            { "TUT_MIMIC", 210 },
            { "GRID_MIMICPOP", 80 },
            { "GRID_SWARM", 100 },
            { "GRID_SWITCH", 100 },
            { "GRID_TRAPS2", 165 },
            { "TUT_ROCKETJUMP", 265 },
            { "TUT_ZIPLINE", 215 },
            { "GRID_CLIMBANG", 347 },
            { "GRID_ROCKETUZI", 255 },
            { "GRID_CRASHLAND", 295 },
            { "GRID_ESCALATE", 197 },
            { "GRID_SPIDERCLAUS", 180 },
            { "GRID_FIRECRACKER_2", 135 },
            { "GRID_SPIDERMAN", 280 },
            { "GRID_DESTRUCTION", 5 },
            { "GRID_HEAT", 75 },
            { "GRID_BOLT", 160 },
            { "GRID_PON", 25 },
            { "GRID_CHARGE", 350 },
            { "GRID_MIMICFINALE", 359 },
            { "GRID_BARRAGE", 85 },
            { "GRID_1GUN", 45 },
            { "GRID_HECK", 105 },
            { "GRID_ANTFARM", 125 },
            { "GRID_FORTRESS", 115 },
            { "GRID_GODTEMPLE_ENTRY", 240 },
            { "GRID_EXTERMINATOR", 325 },
            { "GRID_FEVER", 205 },
            { "GRID_SKIPSLIDE", 196 },
            { "GRID_CLOSER", 0 },
            { "GRID_HIKE", 85 },
            { "GRID_SKIP", 314 },
            { "GRID_CEILING", 230 },
            { "GRID_BOOP", 255 },
            { "GRID_TRIPRAP", 222 },
            { "GRID_ZIPRAP", 46 },
            { "SIDEQUEST_OBSTACLE_PISTOL", 180 },
            { "SIDEQUEST_OBSTACLE_PISTOL_SHOOT", 50 },
            { "SIDEQUEST_OBSTACLE_MACHINEGUN", 345 },
            { "SIDEQUEST_OBSTACLE_RIFLE_2", 320 },
            { "SIDEQUEST_OBSTACLE_UZI2", 150 },
            { "SIDEQUEST_OBSTACLE_SHOTGUN", 159 },
            { "SIDEQUEST_OBSTACLE_ROCKETLAUNCHER", 130 },
            { "SIDEQUEST_RAPTURE_QUEST", 180 },
            { "SIDEQUEST_DODGER", 190 },
            { "GRID_GLASSPATH", 254 },
            { "GRID_GLASSPATH2", 236 },
            { "GRID_HELLVATOR", 0 },
            { "GRID_GLASSPATH3", 200 },
            { "SIDEQUEST_ALL_SEEING_EYE", 156 },
            { "SIDEQUEST_RESIDENTSAWB", 180 },
            { "SIDEQUEST_RESIDENTSAW", 214 },
            { "SIDEQUEST_SUNSET_FLIP_POWERBOMB", 180 },
            { "GRID_BALLOONLAIR", 94 },
            { "SIDEQUEST_BARREL_CLIMB", 300 },
            { "SIDEQUEST_FISHERMAN_SUPLEX", 150 },
            { "SIDEQUEST_STF", 240 },
            { "SIDEQUEST_ARENASIXNINE", 90 },
            { "SIDEQUEST_ATTITUDE_ADJUSTMENT", 150 },
            { "SIDEQUEST_ROCKETGODZ", 115 }
        };

        private static Game Game;
        public override void OnLateInitializeMelon()
        {
            Game = Singleton<Game>.Instance;
            Settings.Register();
            UpdateAntiCheat();
        }

        private static void ReverseLevel()
        {
            // Ensure quick restart is disabled
            var quickRestart = MelonPreferences.GetEntry<bool>("NeonLite/Optimization", "superRestart");
            if (quickRestart == null || quickRestart.Value == true)
            {
                MelonLogger.Warning("Cannot reverse level: Quick Restart must be disabled.");
                return;
            }

            // Ignore boss levels & sacrifice
            var currentLevel = Game.GetCurrentLevel();
            if (currentLevel == null || currentLevel.isBossFight || currentLevel.levelID == "TUT_ORIGIN")
                return;

            GameObject levelGoal = null;
            GameObject levelStart = null;

            // Get all game objects
            var gameObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

            foreach (var obj in gameObjects)
            {
                // Get level start object
                if (levelStart == null && obj.name == "Teleport_START")
                    levelStart = obj;

                // Get level goal object
                if (levelGoal == null)
                {
                    if (currentLevel.isSidequest && obj.name.StartsWith("LoreCollectible"))
                        levelGoal = obj;
                    else if (currentLevel.useBookOfLifeLevelGoal && obj.name == "BookOfLife_Ending")
                        levelGoal = obj;
                    else if (!currentLevel.isSidequest && !currentLevel.useBookOfLifeLevelGoal && obj.name == "Level Goal")
                        levelGoal = obj;
                }

                // De-activate fail states
                if (obj.name.StartsWith("FailState") && obj.activeInHierarchy)
                    obj.SetActive(false);
            }

            if (levelStart == null || levelGoal == null)
                return;

            // Swap positions of the spawn point and the goal
            Vector3 spawnPos = levelStart.transform.position;
            Vector3 goalPos = levelGoal.transform.position;

            levelGoal.transform.position = spawnPos;
            levelStart.transform.position = goalPos;

            // Set custom spawn angle
            int spawnAngle = spawnAngles.TryGetValue(currentLevel.levelID, out var angle) ? angle : 0;
            levelStart.transform.rotation = Quaternion.Euler(0, spawnAngle, 0);

            if (currentLevel.isSidequest)
            {
                levelGoal.transform.position += Vector3.up * 3f;
                levelStart.transform.position += Vector3.down * 3f;
            }
        }

        public static class Settings
        {
            public static MelonPreferences_Category category;
            public static MelonPreferences_Entry<bool> reverseModeEnabled;

            public static void Register()
            {
                category = MelonPreferences.CreateCategory("Reverse Mode");
                reverseModeEnabled = category.CreateEntry("Reverse Mode", true,
                    description: "Reverses the level by swapping the player spawn with the goal, letting you play levels backwards." +
                    "\nRequires \"NeonLite/Optimization/Quick Restart\" to be DISABLED." +
                    "\n\nNote: This will trigger anti-cheat. To reset it, return to the hub.");
            }
        }
        private void UpdateAntiCheat(bool canDisable = false)
        {
            bool triggerAntiCheat = Settings.reverseModeEnabled.Value;

            if (triggerAntiCheat)
                NeonLite.Modules.Anticheat.Register(MelonAssembly);
            else if (canDisable)
                NeonLite.Modules.Anticheat.Unregister(MelonAssembly);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            base.OnSceneWasLoaded(buildIndex, sceneName);

            switch (sceneName)
            {
                case "HUB_HEAVEN":
                    UpdateAntiCheat(true);
                    break;

                case "Player":
                    if (Settings.reverseModeEnabled.Value && NeonLite.Modules.Anticheat.Active)
                        ReverseLevel();
                    break;
            }
        }

        public override void OnPreferencesSaved()
        {
            UpdateAntiCheat();
        }
    }
}