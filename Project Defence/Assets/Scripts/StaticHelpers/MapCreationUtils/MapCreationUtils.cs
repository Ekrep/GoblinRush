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
        /// It's created for use for map creation otherwise don't use it!! 
        /// </summary>
        [System.Serializable]
        public struct BoundedBoundableData
        {
            private BoundableProbe probe;
            public BoundableProbe Probe => probe;
            private Vector3 boundedWorldPosition;
            public Vector3 BoundedWorldPosition=>boundedWorldPosition;
            private Vector2Int[] boundedTilePositions;
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

