using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

namespace StaticHelpers.PathFinder
{
    public static class PathFinder
    {
        #region CasualA*
        public static Vector2Int[] CalculatePath(Dictionary<Vector2Int, GroundTile> tileMap, Vector2Int startPoint, Vector2Int endPoint)
        {
            if (!tileMap.ContainsKey(startPoint) || !tileMap.ContainsKey(endPoint))
            {
                Debug.Log("Key issue");
                return null;
            }
            foreach (var tile in tileMap.Values)
            {
                tile.g = 0;
                tile.h = 0;
                tile.parent = null;
            }
            //can be more optimized!!
            List<GroundTile> openList = new List<GroundTile> { tileMap[startPoint] };
            List<GroundTile> closedList = new List<GroundTile>();
            while (openList.Count > 0)
            {
                openList.Sort((a, b) => a.f.CompareTo(b.f));
                GroundTile current = openList[0];
                if (current.GridPosition == endPoint)
                {
                    List<Vector2Int> path = new List<Vector2Int>();
                    while (current != null)
                    {
                        path.Add(current.GridPosition);
                        current = current.parent;
                    }
                    path.Reverse();
                    return path.ToArray();
                }
                openList.Remove(current);
                closedList.Add(current);
                foreach (var neighbor in GetFourAxisNeighbors(current, tileMap, 1))
                {
                    if (closedList.Contains(neighbor) || neighbor == null || IsTilemapPositionBlocked(neighbor.GridPosition, tileMap))
                        continue;

                    int tentativeG = current.g + 1;
                    bool isInOpenList = openList.Contains(neighbor);
                    if (!isInOpenList || tentativeG < neighbor.g)
                    {
                        neighbor.g = tentativeG;
                        neighbor.h = CalculateH(neighbor, tileMap[endPoint]);
                        neighbor.parent = current;
                        if (!isInOpenList)
                            openList.Add(neighbor);
                    }
                }
            }
            return null;
        }
        #endregion
        #region BlockIgnoredA*
        public static Vector2Int[] IgnoreBlockedTilesAndCalculatePath(Dictionary<Vector2Int, GroundTile> tileMap, Vector2Int startPoint, Vector2Int endPoint)
        {
            if (!tileMap.ContainsKey(startPoint) || !tileMap.ContainsKey(endPoint))
            {
                Debug.Log("Key issue");
                return null;
            }
            foreach (var tile in tileMap.Values)
            {
                tile.g = 0;
                tile.h = 0;
                tile.parent = null;
            }
            //can be more optimized!!
            List<GroundTile> openList = new List<GroundTile> { tileMap[startPoint] };
            List<GroundTile> closedList = new List<GroundTile>();
            while (openList.Count > 0)
            {
                openList.Sort((a, b) => a.f.CompareTo(b.f));
                GroundTile current = openList[0];
                if (current.GridPosition == endPoint)
                {
                    List<Vector2Int> path = new List<Vector2Int>();
                    while (current != null)
                    {
                        path.Add(current.GridPosition);
                        current = current.parent;
                    }
                    path.Reverse();
                    return path.ToArray();
                }
                openList.Remove(current);
                closedList.Add(current);
                foreach (var neighbor in GetFourAxisNeighbors(current, tileMap, 1))
                {
                    if (closedList.Contains(neighbor) || neighbor == null)
                        continue;

                    int tentativeG = current.g + 1;
                    bool isInOpenList = openList.Contains(neighbor);
                    if (!isInOpenList || tentativeG < neighbor.g)
                    {
                        neighbor.g = tentativeG;
                        neighbor.h = CalculateH(neighbor, tileMap[endPoint]);
                        neighbor.parent = current;
                        if (!isInOpenList)
                            openList.Add(neighbor);
                    }
                }
            }
            return null;
        }
        #endregion
        #region FindPathUntillBlockedA*
        public static Vector2Int[] CalculateUntillFindClosestAvailablePath(Dictionary<Vector2Int, GroundTile> tileMap, Vector2Int startPoint, Vector2Int endPoint)
        {
            if (!tileMap.ContainsKey(startPoint) || !tileMap.ContainsKey(endPoint))
            {
                Debug.Log("Key issue");
                return null;
            }
            if (startPoint == endPoint)
            {
                return new Vector2Int[] { startPoint };
            }

            foreach (var tile in tileMap.Values)
            {
                tile.g = 0;
                tile.h = 0;
                tile.parent = null;
            }

            List<GroundTile> openList = new List<GroundTile> { tileMap[startPoint] };
            HashSet<GroundTile> closedSet = new HashSet<GroundTile>();

            GroundTile bestReachedTile = tileMap[startPoint];
            GroundTile current;
            int tentativeG;
            bool isInOpenList;
            while (openList.Count > 0)
            {
                current = openList[0];
                float minF = current.f;
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].f < minF)
                    {
                        current = openList[i];
                        minF = current.f;
                    }
                }
                if (current.GridPosition == endPoint)
                {
                    List<Vector2Int> path = new List<Vector2Int>();
                    while (current != null)
                    {
                        path.Add(current.GridPosition);
                        current = current.parent;
                    }
                    path.Reverse();
                    return path.ToArray();
                }

                openList.Remove(current);
                closedSet.Add(current);
                if (current.h < bestReachedTile.h || bestReachedTile == tileMap[startPoint])
                {
                    bestReachedTile = current;
                }

                foreach (var neighbor in GetFourAxisNeighbors(current, tileMap, 1))
                {
                    if (neighbor == null || closedSet.Contains(neighbor) || IsTilemapPositionBlocked(neighbor.GridPosition, tileMap))
                        continue;

                    tentativeG = current.g + 1;
                    isInOpenList = openList.Contains(neighbor);

                    if (!isInOpenList || tentativeG < neighbor.g)
                    {
                        neighbor.g = tentativeG;
                        neighbor.h = CalculateH(neighbor, tileMap[endPoint]);
                        neighbor.parent = current;

                        if (!isInOpenList)
                            openList.Add(neighbor);
                    }
                }
            }

            List<Vector2Int> fallbackPath = new List<Vector2Int>();
            GroundTile fallbackCurrent = bestReachedTile;
            while (fallbackCurrent != null)
            {
                fallbackPath.Add(fallbackCurrent.GridPosition);
                fallbackCurrent = fallbackCurrent.parent;
            }
            fallbackPath.Reverse();
            return fallbackPath.ToArray();
        }
        #endregion


        public static GroundTile[] GetFourAxisNeighbors(GroundTile tile, Dictionary<Vector2Int, GroundTile> tileMap, int layer)
        {
            GroundTile[] neighbors = new GroundTile[4];
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { 1, -1, 0, 0 };// i mean z axis because 3d represent.
            for (int i = 0; i < 4; i++)
            {
                Vector2Int newPos = new Vector2Int(tile.GridPosition.x + dx[i] * layer, tile.GridPosition.y + dy[i] * layer);
                if (IsPositionInTileMapBounds(new Vector2Int(newPos.x, newPos.y), tileMap))//check is groundTileValid
                {
                    neighbors[i] = tileMap[newPos];
                }
            }
            neighbors = DeleteNullVarsAndTrimNeighborArray(neighbors);
            return neighbors;

        }
        public static GroundTile[] GetEightAxisNeighbors(GroundTile tile, Dictionary<Vector2Int, GroundTile> tileMap, int layer)
        {
            List<GroundTile> neighbors = new List<GroundTile>();

            for (int dx = -layer; dx <= layer; dx++)
            {
                for (int dy = -layer; dy <= layer; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    Vector2Int newPos = new Vector2Int(tile.GridPosition.x + dx, tile.GridPosition.y + dy);

                    if (IsPositionInTileMapBounds(newPos, tileMap))
                    {
                        neighbors.Add(tileMap[newPos]);
                    }
                }
            }
            return neighbors.ToArray();

        }
        public static Vector2Int[] GetFourAxisNeighborPositions(Vector2Int tilePosition)
        {
            Vector2Int[] neighborPositions = new Vector2Int[4];
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { 1, -1, 0, 0 };
            for (int i = 0; i < 4; i++)
            {
                Vector2Int newPos = new Vector2Int(tilePosition.x + dx[i], tilePosition.y + dy[i]);
                neighborPositions[i] = newPos;
            }
            return neighborPositions;

        }
        //fix it
        public static Vector2Int[] GetEightAxisNeighborPositions(Vector2Int tilePosition)
        {
            Vector2Int[] neighborPositions = new Vector2Int[8];
            int[] dx = { 0, 0, 0, -1, 1, 1, 1, -1, -1 };
            int[] dy = { 0, 1, -1, 0, 0, 1, -1, -1, 1 };
            for (int i = 0; i < 8; i++)
            {
                Vector2Int newPos = new Vector2Int(tilePosition.x + dx[i], tilePosition.y + dy[i]);
                neighborPositions[i] = newPos;
            }
            return neighborPositions;
        }

        private static GroundTile[] DeleteNullVarsAndTrimNeighborArray(GroundTile[] array)
        {
            List<GroundTile> tileList = new List<GroundTile>();
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != null)
                {
                    tileList.Add(array[i]);
                }
            }
            return tileList.ToArray();

        }
        private static bool IsPositionInTileMapBounds(Vector2Int position, Dictionary<Vector2Int, GroundTile> tileMap)
        {
            return tileMap.ContainsKey(position);
        }
        private static bool IsTilemapPositionBlocked(Vector2Int position, Dictionary<Vector2Int, GroundTile> tileMap)
        {
            return tileMap[position].IsBlocked;
        }
        private static int CalculateH(GroundTile firstTile, GroundTile secondTile)
        {
            return Mathf.Abs(firstTile.GridPosition.x - secondTile.GridPosition.x) + Mathf.Abs(firstTile.GridPosition.y - secondTile.GridPosition.y);
        }
        private static Vector2Int FindAvailableClosestTilePos(Vector2Int startPoint, Vector2Int endPoint, Dictionary<Vector2Int, GroundTile> tileMap)
        {
            var keys = tileMap.Keys;
            int maxX = keys.Max(pos => pos.x);
            int maxY = keys.Max(pos => pos.y);
            int minX = keys.Min(pos => pos.x);
            int minY = keys.Min(pos => pos.y);
            int tileMapMaxRadius = Mathf.Max(maxX - minX, maxY - minY);
            GroundTile availableClosestNeighbor = null;
            GroundTile[] neighborCalculationResult;
            int multiplier = 1;
            while (availableClosestNeighbor == null && multiplier < tileMapMaxRadius)
            {
                //sort them
                neighborCalculationResult = GetEightAxisNeighbors(tileMap[endPoint], tileMap, multiplier);
                availableClosestNeighbor = GetClosestAvailableNeighborTileToStartPoint(startPoint, neighborCalculationResult);
                if (availableClosestNeighbor != null)
                {
                    return availableClosestNeighbor.GridPosition;
                }
                multiplier++;
            }
            return startPoint;

        }
        private static GroundTile GetClosestAvailableNeighborTileToStartPoint(Vector2Int startPoint, GroundTile[] neighborTiles)
        {
            float smallestDistance = float.MaxValue;
            float calculatedDistance;
            int closestTileIndex = -1;//check count and return null
            for (int i = 0; i < neighborTiles.Length; i++)
            {
                if (neighborTiles[i] == null || neighborTiles[i].IsBlocked)//check blockedStatus
                    continue;
                calculatedDistance = Vector2Int.Distance(startPoint, neighborTiles[i].GridPosition);
                if (calculatedDistance < smallestDistance)
                {
                    smallestDistance = calculatedDistance;
                    closestTileIndex = i;
                }
            }
            if (closestTileIndex == -1)
            {
                return null;
            }
            else
            {
                return neighborTiles[closestTileIndex];
            }
        }

    }


}

