using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Scriptables.MapCreation.MapData
{
    [CreateAssetMenu(menuName = "Data/MapData")]
    public class MapData : SerializedScriptableObject
    {
        [Header("Tile Values")]
        public Vector2Int[,] tilePositions;
        public float cellXOffset;
        public float cellZOffset;
        public Vector3 tileScale;
        [Header("Boundable Object Values")]
        public Dictionary<Vector2Int, BoundableProbe> boundables;
    }
}
