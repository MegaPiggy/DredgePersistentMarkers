using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PersistentMarkers.Patches
{
    [HarmonyPatch(typeof(SpyglassMapStamp), "LateUpdate")]
    public static class SpyglassMapStamp_Patch
	{
		public static float dropOff = 0.55f; //2f;

		public static bool Prefix(SpyglassMapStamp __instance)
        {
			__instance.rectTransform.position = Camera.main.WorldToScreenPoint(__instance.worldPosition);
            __instance.isOnScreen = __instance.rectTransform.position.z > 0;
            __instance.stampImage.enabled = __instance.isOnScreen;
            __instance.distanceTextField.enabled = __instance.isOnScreen;
            if (!__instance.isOnScreen)
                return false;
            __instance.magnitude = __instance.rectTransform.anchoredPosition.magnitude;
            __instance.rectTransform.anchoredPosition = Vector3.ClampMagnitude(__instance.rectTransform.anchoredPosition, __instance.radius);
            __instance.canvasGroup.alpha = Mathf.Lerp(1, 0, Mathf.InverseLerp(0, __instance.radius * dropOff, __instance.magnitude - __instance.radius));
            __instance.timeUntilDistanceUpdate -= Time.deltaTime;
            if (__instance.timeUntilDistanceUpdate < 0)
            {
                __instance.timeUntilDistanceUpdate = __instance.timeBetweenDistanceUpdates;
                __instance.UpdatePlayerDistance();
            }
            return false;
        }
    }
}
