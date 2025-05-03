using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticHelpers.Util
{
    public static class Utils
    {
        public enum GoblinType
        {
            Casual,
            Breach

        }
        public enum PathStatus
        {
            Blocked,
            Reachable

        }
        [System.Serializable]
        public struct PathData
        {
            [SerializeField] private uint path_ID;
            public uint Path_ID => path_ID;
            [SerializeField] private Vector2Int[] path;
            public Vector2Int[] Path => path;
            [SerializeField] private bool isStartable;
            public bool IsStartable => isStartable;
            [SerializeField] private PathStatus pathStatus;
            public PathStatus PathStatus => pathStatus;
            [SerializeField] private Vector2Int pathStartGridPos;
            public Vector2Int PathStartGridPos => pathStartGridPos;
            [SerializeField] private Vector2Int pathEndGridPos;
            public Vector2Int PathEndGridPos => pathEndGridPos;
            public PathData(uint path_ID, Vector2Int[] path, Vector2Int pathStartGridPos, Vector2Int pathEndGridPos, bool isStartable)
            {
                this.path_ID = path_ID;
                this.path = path;
                this.pathEndGridPos = pathEndGridPos;
                this.pathStartGridPos = pathStartGridPos;
                this.isStartable = isStartable;
                if (path != null && path.Length > 0 && Vector2Int.Distance(path[path.Length - 1], pathEndGridPos) < 3f)
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

