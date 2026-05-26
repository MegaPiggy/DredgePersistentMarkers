using System.Collections;
using UnityEngine;

namespace PersistentMarkers.Patches
{
    internal static class MarkerStampRefresh
    {
        private static bool mapOpen;
        private static bool spyglassActive;
        private static bool internalSpyglassClose;
        private static bool refreshInProgress;
        private static bool refreshQueued;
        private static MarkerStampRefreshRunner runner;

        public static bool IsMapOpen => mapOpen;
        public static bool IsInternalSpyglassClose => internalSpyglassClose;

        public static void SetMapOpen(bool open)
        {
            mapOpen = open;
        }

        public static void SetSpyglassActive(bool active)
        {
            if (!internalSpyglassClose)
                spyglassActive = active;
        }

        public static void EnsureRunner(SpyglassUI spyglassUI)
        {
            if (runner != null || spyglassUI == null)
                return;

            var gameObject = new GameObject("PersistentMarkers.RefreshRunner");
            Object.DontDestroyOnLoad(gameObject);
            runner = gameObject.AddComponent<MarkerStampRefreshRunner>();
        }

        public static void RequestRebuild()
        {
            if (refreshQueued || runner == null)
                return;

            runner.StartCoroutine(RebuildSoon());
        }

        private static IEnumerator RebuildSoon()
        {
            refreshQueued = true;

            // Let the game finish map UI callbacks and marker deletion first.
            yield return null;
            yield return null;

            ForceVanillaCleanupAndRebuild();
            refreshQueued = false;
        }

        public static void ForceVanillaCleanupAndRebuild()
        {
            if (refreshInProgress || mapOpen || spyglassActive)
                return;

            var spyglassUI = GameManager.Instance?.UI?.SpyglassUI;
            if (spyglassUI == null)
                return;

            refreshInProgress = true;
            try
            {
                internalSpyglassClose = true;
                try
                {
                    spyglassUI.OnPlayerAbilityToggled(spyglassUI.spyglassAbilityData, false);
                }
                finally
                {
                    internalSpyglassClose = false;
                }

                spyglassUI.AddStamps();
            }
            finally
            {
                refreshInProgress = false;
            }
        }
    }

    internal class MarkerStampRefreshRunner : MonoBehaviour
    {
    }
}
