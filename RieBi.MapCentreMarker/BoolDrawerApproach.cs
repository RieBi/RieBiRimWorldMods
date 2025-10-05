using UnityEngine;
using Verse;

namespace RieBi.MapCentreMarker
{
    public class BoolDrawerApproach : ICellBoolGiver
    {
        private CellBoolDrawer _boolDrawer;

        private int _mapX = -1;
        private int _mapZ = -1;

        public void Draw()
        {
            var currentMap = Find.CurrentMap;
            
            if (currentMap.Size.x != _mapX || currentMap.Size.z != _mapZ)
            {
                ResetDrawer();
                
                _mapX = currentMap.Size.x;
                _mapZ = currentMap.Size.z;
                
                Log.Message($"Map size x: {Find.CurrentMap.Size.x}, y: {Find.CurrentMap.Size.y}, z: {Find.CurrentMap.Size.z}");
                Log.Message($"Map id: {Find.CurrentMap.uniqueID}");
                
                var x = Find.CurrentMap.Size.x;
                var z = Find.CurrentMap.Size.z;

                var x1 = x / 2;
                var x2 = x % 2 == 0 ? x1 - 1 : x1;
            
                var z1 = z / 2;
                var z2 = z % 2 == 0 ? z1 - 1 : z1;
                
                Log.Message($"x1: {x1}, x2: {x2}, z1: {z1}, z2: {z2}");
            }
            
            _boolDrawer.MarkForDraw();
            _boolDrawer.CellBoolDrawerUpdate();
        }

        public Color Color => Color.white;

        public bool GetCellBool(int index)
        {
            var x = Find.CurrentMap.Size.x;
            var z = Find.CurrentMap.Size.z;

            var indexX = index % x;
            var indexZ = index / x;

            var x1 = x / 2;
            var x2 = x % 2 == 0 ? x1 - 1 : x1;
            
            var z1 = z / 2;
            var z2 = z % 2 == 0 ? z1 - 1 : z1;
            
            return indexX == x1 || indexX == x2 || indexZ == z1 || indexZ == z2;

            // 11 - 5
            // 10 - 4, 5
        }

        public Color GetCellExtraColor(int index)
        {
            return Color.green;
        }

        private void ResetDrawer()
        {
            _boolDrawer = new CellBoolDrawer(this, Find.CurrentMap.Size.x, Find.CurrentMap.Size.z, opacity: 0.5f);
        }
    }
}