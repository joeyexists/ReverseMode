using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(ReverseMode.Core), "ReverseMode", "1.1.1", "joeyexists", null)]
[assembly: MelonGame("Little Flag Software, LLC", "Neon White")]

namespace ReverseMode
{
    public class Core : MelonMod
    {
        private static Game Game;
        private bool hasInitializedAntiCheat = false;

        public override void OnLateInitializeMelon()
        {
            Game = Singleton<Game>.Instance;
            Settings.Register(this);
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
            LevelData currentLevel = Game.GetCurrentLevel();
            if (currentLevel == null || currentLevel.isBossFight || currentLevel.levelID == "TUT_ORIGIN")
                return;

            GameObject levelGoal = null, levelStart = null;

            // Loop through each object
            foreach (var obj in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                // De-activate fail states
                if (obj.name.StartsWith("FailState") && obj.activeInHierarchy)
                {
                    obj.SetActive(false);
                    continue;
                }

                // Get level start object
                if (levelStart == null && obj.name == "Teleport_START")
                {
                    levelStart = obj;
                    continue;
                }

                // Get level goal object
                if (levelGoal == null)
                {
                    if (currentLevel.isSidequest && obj.name.StartsWith("LoreCollectible") ||
                        currentLevel.useBookOfLifeLevelGoal && obj.name == "BookOfLife_Ending" ||
                        !currentLevel.isSidequest && !currentLevel.useBookOfLifeLevelGoal && obj.name == "Level Goal")
                    {
                        levelGoal = obj;
                        continue;
                    }
                }
            }

            if (levelStart == null || levelGoal == null)
                return;

            // Swap positions of the spawn point and the goal
            Vector3 spawnPos = levelStart.transform.position;
            Vector3 goalPos = levelGoal.transform.position;

            levelGoal.transform.position = spawnPos;
            levelStart.transform.position = goalPos;

            // Set custom spawn angle
            int spawnAngle = SpawnAngles.Angles.TryGetValue(currentLevel.levelID, out var angle) ? angle : 0;
            levelStart.transform.rotation = Quaternion.Euler(0, spawnAngle, 0);

            if (currentLevel.isSidequest)
            {
                levelGoal.transform.position += Vector3.up * 3f;
                levelStart.transform.position += Vector3.down * 3f;
            }
        }

        private static class Settings
        {
            public static MelonPreferences_Category category;
            public static MelonPreferences_Entry<bool> reverseModeEntry;

            public static void Register(Core modInstance)
            {
                category = MelonPreferences.CreateCategory("Reverse Mode");
                reverseModeEntry = category.CreateEntry("Reverse Mode", true,
                    description: "Reverses the level by swapping the player spawn with the goal, letting you play levels backwards." +
                    "\nRequires \"NeonLite/Optimization/Quick Restart\" to be DISABLED." +
                    "\n\nNote: This will trigger anti-cheat. To reset it, return to the hub.");

                reverseModeEntry.OnEntryValueChanged.Subscribe((oldValue, newValue) =>
                {
                    modInstance.UpdateAntiCheat();
                });
            }
        }
        private void UpdateAntiCheat(bool canDisable = false)
        {
            if (Settings.reverseModeEntry.Value)
                NeonLite.Modules.Anticheat.Register(MelonAssembly);
            else if (canDisable)
                NeonLite.Modules.Anticheat.Unregister(MelonAssembly);
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (!hasInitializedAntiCheat && sceneName == "Menu")
            {
                // Update anti-cheat after the game loads
                hasInitializedAntiCheat = true;
                UpdateAntiCheat();
            }

            else if (sceneName == "HUB_HEAVEN")
                UpdateAntiCheat(true);

            else if (sceneName == "Player" && 
                     Settings.reverseModeEntry.Value &&
                     NeonLite.Modules.Anticheat.Active)
            {
                // Reverse level when the player scene is loaded
                ReverseLevel();
            }
        }
    }
}