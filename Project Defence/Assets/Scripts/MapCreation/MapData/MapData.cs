using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using static StaticHelpers.MapCreationUtils.MapCreationUtils;
using UnityEditor;

namespace Scriptables.MapCreation.MapData
{
    [CreateAssetMenu(menuName = "Data/MapData")]
    public class MapData : SerializedScriptableObject
    {
        [Header("Grid Values")]
        public float cellXOffset;
        public float cellZOffset;
        public Vector3 tileScale;

        [Header("Tile Values")]
        public PlacedTileData[] placedTiles;
        
        [Header("Boundable Object Values")]
        public BoundedBoundableData[] boundedBoundableDatas;

        public void Save(PlacedTileData[] placedTiles, float cellXOffset, float cellZOffset, Vector3 tileScale, BoundedBoundableData[] boundables)
        {
            this.placedTiles = placedTiles;
            this.cellXOffset = cellXOffset;
            this.cellZOffset = cellZOffset;
            this.tileScale = tileScale;
            boundedBoundableDatas = boundables;
            if (!EditorUtility.IsDirty(this))
            {
                EditorUtility.SetDirty(this);
            }
            AssetDatabase.SaveAssetIfDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}
