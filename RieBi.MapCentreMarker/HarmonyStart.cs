using HarmonyLib;
using Verse;

namespace RieBi.MapCentreMarker
{
    [StaticConstructorOnStartup]
    public class HarmonyStart
    {
        static HarmonyStart()
        {
            var harmony = new Harmony("RieBi.RieBi.MapCentreMarker");
            
            harmony.PatchAll();
        }
    }
}