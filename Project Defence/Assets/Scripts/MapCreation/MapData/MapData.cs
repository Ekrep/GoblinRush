using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using static StaticHelpers.MapCreationUtils.MapCreationUtils;
using UnityEditor;
using static StaticHelpers.Util.Utils;
using StaticHelpers.PathFinder;

namespace Scriptables.MapCreation.MapData
{
    [CreateAssetMenu(menuName = "Data/MapData")]
    public class MapData : SerializedScriptableObject
    {
        [Header("Grid Values")]
        public float cellXOffset;
        public float cellZOffset;
        public Vector3 tileScale;
        public Vector2Int minBounds;
        public Vector2Int maxBounds;
        public Vector2Int[] possiblePathStartPoints;
        public Vector2Int nexusCenter;

        [Header("Tile Values")]
        public PlacedTileData[] placedTiles;

        [Header("Boundable Object Values")]
        public BoundedBoundableData[] boundedBoundableDatas;
        [Header("Path Data")]
        public PathData[] pathDatas;
        [Header("Currency")]
        public int levelStartMoney;

        public void Save(PlacedTileData[] placedTiles, float cellXOffset, float cellZOffset, Vector3 tileScale, BoundedBoundableData[] boundables, Vector2Int minBounds, Vector2Int maxBounds, Vector2Int[] possiblePathStartPoints, Vector2Int nexusCenter, PathData[] pathDatas)
        {
            this.placedTiles = placedTiles;
            this.cellXOffset = cellXOffset;
            this.cellZOffset = cellZOffset;
            this.tileScale = tileScale;
            this.minBounds = minBounds;
            this.maxBounds = maxBounds;
            this.possiblePathStartPoints = possiblePathStartPoints;
            boundedBoundableDatas = boundables;
            this.nexusCenter = nexusCenter;
            this.pathDatas = pathDatas;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }

    }
}
