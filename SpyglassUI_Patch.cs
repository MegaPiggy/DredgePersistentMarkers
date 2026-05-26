using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistentMarkers.Patches
{
	[HarmonyPatch(typeof(SpyglassUI), "Awake")]
	public static class SpyglassUI_Patch
	{
		public static void Postfix(SpyglassUI __instance)
        {
            __instance.AddStamps();
            MarkerStampRefresh.EnsureRunner(__instance);
        }
	}

	[HarmonyPatch(typeof(SpyglassUI), "OnPlayerAbilityToggled")]
    public static class SpyglassUI_OnPlayerAbilityToggled_Patch
    {
        public static void Postfix(
          SpyglassUI __instance,
          AbilityData abilityData,
          bool enabled)
        {
            if (__instance.spyglassAbilityData.name != abilityData.name)
                return;

            if (MarkerStampRefresh.IsInternalSpyglassClose)
                return;

            MarkerStampRefresh.SetSpyglassActive(enabled);

            if (!enabled && !MarkerStampRefresh.IsMapOpen)
                MarkerStampRefresh.RequestRebuild();
        }
	}
}
