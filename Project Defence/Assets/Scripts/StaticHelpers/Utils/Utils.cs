using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticHelpers.Util
{
    public static class Utils
    {
        public enum GoblinType
        {
            Troop,
            SoftBreach,
            HardBreach
        }
        public enum PathStatus
        {
            Blocked,
            Reachable

        }
        public struct PathData
        {
            private uint path_ID;
            public uint Path_ID => path_ID;
            private GroundTile[] path;
            public GroundTile[] Path => path;
            private Dictionary<Vector2Int, GroundTile> pathTiles;
            public Dictionary<Vector2Int, GroundTile> PathTiles => pathTiles;//bad choice
            private PathStatus pathStatus;
            public PathStatus PathStatus => pathStatus;
            private Vector2Int pathStartGridPos;
            public Vector2Int PathStartGridPos => pathStartGridPos;
            private Vector2Int pathEndGridPos;
            public Vector2Int PathEndGridPos => pathEndGridPos;
            public PathData(uint path_ID, GroundTile[] path, Vector2Int pathStartGridPos, Vector2Int pathEndGridPos)
            {
                this.path_ID = path_ID;
                this.path = path;
                pathTiles = new Dictionary<Vector2Int, GroundTile>(path.Length);
                for (int i = 0; i < path.Length; i++)
                {
                    pathTiles.TryAdd(path[i].GridPosition, path[i]);
                }
                this.pathEndGridPos = pathEndGridPos;
                this.pathStartGridPos = pathStartGridPos;
                if (path[path.Length - 1].GridPosition == pathEndGridPos)
                {
                    pathStatus = PathStatus.Reachable;
                }
                else
                {
                    pathStatus = PathStatus.Blocked;
                }
            }

        }


    }

}

