using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticHelpers.Convertor
{
    public static class PositionConvertor
    {
        public static Vector3 TilePositionToWorldPosition(Vector2Int tilePosition, Vector2 cellOffset)
        {
            //centerPos of tile
            return new Vector3(tilePosition.x * cellOffset.x, 0, tilePosition.y * cellOffset.y);
        }
        public static Vector2Int WorldPositionToTilePosition(Vector3 worldPosition, Vector2 cellOffset)
        {
            return new Vector2Int(Mathf.RoundToInt(worldPosition.x / cellOffset.x), Mathf.RoundToInt(worldPosition.z / cellOffset.y));
        }
    }

}

