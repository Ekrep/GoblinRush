using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using static StaticHelpers.MapCreationUtils.MapCreationUtils;
using UnityEditor;
using static StaticHelpers.Util.Utils;

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
        public Vector2Int[] nexusClosestNeighborTiles;

        [Header("Tile Values")]
        public PlacedTileData[] placedTiles;

        [Header("Boundable Object Values")]
        public BoundedBoundableData[] boundedBoundableDatas;

        public void Save(PlacedTileData[] placedTiles, float cellXOffset, float cellZOffset, Vector3 tileScale, BoundedBoundableData[] boundables, Vector2Int minBounds, Vector2Int maxBounds, Vector2Int[] possiblePathStartPoints)
        {
            this.placedTiles = placedTiles;
            this.cellXOffset = cellXOffset;
            this.cellZOffset = cellZOffset;
            this.tileScale = tileScale;
            this.minBounds = minBounds;
            this.maxBounds = maxBounds;
            this.possiblePathStartPoints = possiblePathStartPoints;
            boundedBoundableDatas = boundables;
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssetIfDirty(this);
        }
        private void InitializePaths()
        {
            PathData[] availablePaths = new PathData[possiblePathStartPoints.Length];

        }
        // private Vector2Int GetClosestPathStartPointToNexus(Vector2Int pathStartPoint, Vector2Int[] nexusClosestNeighbors)
        // {


        // }
    }
}
