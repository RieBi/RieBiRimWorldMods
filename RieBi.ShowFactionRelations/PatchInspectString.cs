using System.Text.RegularExpressions;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RieBi.ShowFactionRelations
{
    [StaticConstructorOnStartup]
    public static class PatchInspectString
    {
        private static Regex? _regex;
        private static string? _language;
        
        static PatchInspectString()
        {
            UpdateRegex();
        }
        
        public static string PostfixInternal(string result, WorldObject __instance)
        {
            if (__instance.Faction == null || !__instance.AppendFactionToInspectString)
                return result;
            
            var faction = __instance.Faction;
            if (faction == Faction.OfPlayer)
                return result;
            
            var originalFactionString = (string)("Faction".Translate() + ": " + __instance.Faction.Name);
            var relations = faction.PlayerRelationKind;

            var relationsGoodwill = faction.PlayerGoodwill;

            if (faction.Hidden)
            {
                relations = FactionRelationKind.Hostile;
                relationsGoodwill = -100;
            }

            var relationsText = relations.GetLabelCap();
            
            var relationsColor = ColorUtility.ToHtmlStringRGB(relations.GetColor());
            var relationsGoodwillText = relationsGoodwill.ToStringWithSign();

            var addedLine = $"<color=#{relationsColor}>{relationsText} ({relationsGoodwillText})</color>";
            var newFactionString = $"{originalFactionString}\n{addedLine}";
            
            UpdateRegex();

            return _regex!.Replace(result, newFactionString);
        }

        private static void UpdateRegex()
        {
            var currentLanguage = LanguageDatabase.activeLanguage.folderName;

            if (_language == currentLanguage)
                return;

            _language = currentLanguage;
            _regex = new Regex(@$"{"Faction".Translate()}: .*(?:\s+.+ \([+-]?\d+\))?", RegexOptions.Compiled | RegexOptions.Multiline);
        }
    }
    
    [HarmonyPatch(typeof(Site), nameof(WorldObject.GetInspectString))]
    public class PatchInspectStringSite
    {
        public static string Postfix(string result, WorldObject __instance)
        {
            return PatchInspectString.PostfixInternal(result, __instance);
        }
    }
    
    [HarmonyPatch(typeof(Settlement), nameof(WorldObject.GetInspectString))]
    public class PatchInspectStringSettlement
    {
        public static string Postfix(string result, WorldObject __instance)
        {
            return PatchInspectString.PostfixInternal(result, __instance);
        }
    }
}