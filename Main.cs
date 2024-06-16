using HarmonyLib;
using System.Reflection;

namespace PersistentMarkers
{
    public static class Main
    {
        public static void Initialize()
        {
            new Harmony("megapiggy.persistentmarkers").PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
