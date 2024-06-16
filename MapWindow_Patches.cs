using HarmonyLib;

namespace PersistentMarkers.Patches
{
    [HarmonyPatch(typeof(MapWindow), "Show")]
    public static class MapWindowShowPatch
    {
        public static void Postfix(MapWindow __instance) => GameManager.Instance.UI.SpyglassUI.RemoveStamps();
    }

    [HarmonyPatch(typeof(MapWindow), "Hide")]
    public static class MapWindowHidePatch
    {
        public static void Postfix(MapWindow __instance) => GameManager.Instance.UI.SpyglassUI.AddStamps();
    }
}
