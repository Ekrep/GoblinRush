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
            private Vector2Int[] boundedPositions;
            public Vector2Int[] BoundedPositions => boundedPositions;
            public BoundedBoundableData(BoundableProbe probe, Vector2Int[] boundedPositions)
            {
                this.probe = probe;
                this.boundedPositions = boundedPositions;
            }

        }
    }

}

