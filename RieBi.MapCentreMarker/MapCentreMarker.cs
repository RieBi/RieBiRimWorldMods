using System;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace RieBi.MapCentreMarker
{
    [StaticConstructorOnStartup]
    public static class MapCentreMarker
    {
        public static bool ShowCentreMarker = false;
        private static Material _material;
        private static bool _logged;

        static MapCentreMarker()
        {
            Load();
        }

        private static BoolDrawerApproach _boolDrawerApproach = new BoolDrawerApproach();

        public static void Draw()
        {
            _boolDrawerApproach.Draw();
            
            var size = Find.CurrentMap.Size;
            var cameraRect = Find.CameraDriver.CurrentViewRect;

            var midX = (float)size.x / 2;
            DrawVertical(midX, cameraRect, size);
            DrawHorizontal(midX, cameraRect, size);
        }

        private static void DrawVertical(float x, CellRect cameraRect, IntVec3 mapSize)
        {
            if (cameraRect.minX <= x && x <= cameraRect.maxX)
            {
                var start = Math.Max(cameraRect.minZ, 0);
                var end = Math.Min(cameraRect.maxZ, mapSize.z - 1);
                for (var i = start; i <= end; i++)
                    Draw(new Vector3(x, 0, i + 0.5f));
            }
        }

        private static void DrawHorizontal(float y, CellRect cameraRect, IntVec3 mapSize)
        {
            if (cameraRect.minZ <= y && y <= cameraRect.maxZ)
            {
                var start = Math.Max(cameraRect.minX, 0);
                var end = Math.Min(cameraRect.maxX, mapSize.x - 1);
                for (var i = start; i <= end; i++)
                    Draw(new Vector3(i + 0.5f, 0, y));
            }
        }

        private static void Draw(Vector3 pos)
        {
            Graphics.DrawMesh(MeshPool.plane10, pos, Quaternion.identity, _material, 0);
        }

        private static void Load()
        {
            _material = MaterialPool.MatFrom("Designations/Plan", ShaderDatabase.MetaOverlay);
        }
    }

    [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.ExposeData))]
    public class Patch_PlaySettings_ExposeData
    {
        public static void Prefix()
        {
            Scribe_Values.Look<bool>(ref MapCentreMarker.ShowCentreMarker, "showCentreMarker");
        }
    }

    [HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
    public class Patch_PlaySettings_DoPlaySettingsGlobalControls
    {
        public static void Postfix(WidgetRow row)
        {
            row.ToggleableIcon(ref MapCentreMarker.ShowCentreMarker, TexButton.ShowZones, "ShowCentreMarkerToggleButton".Translate(), SoundDefOf.Mouseover_ButtonToggle);
        }
    }

    [HarmonyPatch(typeof(MapInterface), nameof(MapInterface.MapInterfaceUpdate))]
    public class Patch_MapInterface_MapInterfaceUpdate
    {
        public static void Postfix()
        {
            if (MapCentreMarker.ShowCentreMarker)
                MapCentreMarker.Draw();
        }
    }
}