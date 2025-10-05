using HarmonyLib;
using Verse;

namespace RieBi.ShowFactionRelations
{
    [StaticConstructorOnStartup]
    public class MyModMain
    {
        static MyModMain()
        {
            var harmony = new Harmony("RieBi.RieBi.ShowFactionRelations");
            
            harmony.PatchAll();
        }

        private static void Test()
        {
        }
    }
}