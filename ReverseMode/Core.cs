using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(ReverseMode.Core), "ReverseMode", "1.0.0", "joeyexists", null)]
[assembly: MelonGame("Little Flag Software, LLC", "Neon White")]

namespace ReverseMode
{
    public class Core : MelonMod
    {
        private static Game Game;
        private static bool modEnabled = true; 
        public override void OnLateInitializeMelon()
        {
            // Make sure NeonLite is loaded
            var neonLiteAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name.Equals("NeonLite", StringComparison.OrdinalIgnoreCase));
            if (neonLiteAssembly == null)
            {
                MelonLogger.Error("NeonLite not found!");
                modEnabled = false;
                return;
            }

            Game = Singleton<Game>.Instance;
            Settings.Register();
            UpdateAntiCheat();
        }

        private static void ReverseLevel()
        {
            // Ensure quick restart is disabled
            var quickRestart = MelonPreferences.GetEntry<bool>("NeonLite/Optimization", "superRestart");
            if (quickRestart?.Value == true)
            {
                MelonLogger.Warning("Cannot reverse level: Quick Restart must be disabled.");
                return;
            }

            var currentLevel = Game.GetCurrentLevel();
            if (currentLevel == null)
                return;

            // Ignore boss levels & sacrifice
            if (currentLevel.isBossFight || currentLevel.GetLevelDisplayName() == "Interface/LEVELNAME_TUT_ORIGIN")
                return;

            GameObject levelGoal;

            // Get level goal depending on level type
            if (currentLevel.isSidequest)
                levelGoal = UnityEngine.Object.FindObjectsOfType<GameObject>().FirstOrDefault(obj => obj.name.StartsWith("LoreCollectible"));
            else if (currentLevel.useBookOfLifeLevelGoal)
                levelGoal = GameObject.Find("BookOfLife_Ending");
            else
                levelGoal = GameObject.Find("Level Goal");

            GameObject levelStart = GameObject.Find("Teleport_START");

            if (levelStart == null || levelGoal == null)
                return;

            // De-activate fail states
            var failStates = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(obj => obj.name.StartsWith("FailState") && obj.activeInHierarchy);
            foreach (var failState in failStates)
                failState.SetActive(false);

            // Swap positions of the spawn point and the goal
            Vector3 spawnPos = levelStart.transform.position;
            Vector3 goalPos = levelGoal.transform.position;

            levelStart.transform.position = goalPos;
            levelStart.transform.Rotate(0, 180, 0);

            levelGoal.transform.position = spawnPos;

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

            if (!modEnabled)
                return;

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
            base.OnPreferencesSaved();
            if (modEnabled)
                UpdateAntiCheat();
        }
    }
}