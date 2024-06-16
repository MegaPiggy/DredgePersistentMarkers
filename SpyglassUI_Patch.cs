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
		public static void Postfix(SpyglassUI __instance) => __instance.AddStamps();
	}

	[HarmonyPatch(typeof(SpyglassUI), "OnPlayerAbilityToggled")]
    public static class SpyglassUI_OnPlayerAbilityToggled_Patch
    {
        public static bool Prefix(
          SpyglassUI __instance,
          AbilityData abilityData,
          bool enabled)
        {
            if (__instance.spyglassAbilityData.name != abilityData.name) return false;

            if (!enabled)
            {
                __instance.focusedHarvestPOI = null;
                __instance.container.SetActive(false);
                __instance.animator.SetBool("showing", false);
            }
            __instance.crosshairContainer.SetActive(enabled);
            return false;
        }
	}
}
