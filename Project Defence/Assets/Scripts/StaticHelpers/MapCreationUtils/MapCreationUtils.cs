using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticHelpers.MapCreationUtils
{
    public static class MapCreationUtils
    {
        public enum MapCreatorMode
        {
            TileMode,
            BuildingMode

        }
        /// <summary>
        /// Only for map creation, do not use elsewhere!
        /// </summary>
        [System.Serializable]
        public struct PlacedTileData
        {
            [SerializeField] private GroundTile groundTile;
            public GroundTile GroundTile => groundTile;
            [SerializeField] private Vector2Int gridPosition;
            public Vector2Int GridPos => gridPosition;
            [SerializeField] private Vector3 placedWorldPosition;
            public Vector3 PlacedWorldPosition => placedWorldPosition;
            public PlacedTileData(GroundTile groundTile, Vector2Int gridPos, Vector3 worldPos)
            {
                this.groundTile = groundTile;
                gridPosition = gridPos;
                placedWorldPosition = worldPos;
            }
        }
        /// <summary>
        /// Only for map creation, do not use elsewhere!
        /// </summary>
        [System.Serializable]
        public struct BoundedBoundableData
        {
            [SerializeField] private BoundableProbe probe;
            public BoundableProbe Probe => probe;
            [SerializeField] private Vector3 boundedWorldPosition;
            public Vector3 BoundedWorldPosition => boundedWorldPosition;
            [SerializeField] private Vector2Int[] boundedTilePositions;
            public Vector2Int[] BoundedTilePositions => boundedTilePositions;
            public BoundedBoundableData(BoundableProbe probe, Vector2Int[] boundedTilePositions, Vector3 boundedWorldPosition)
            {
                this.probe = probe;
                this.boundedTilePositions = boundedTilePositions;
                this.boundedWorldPosition = boundedWorldPosition;
            }

        }
    }

}

