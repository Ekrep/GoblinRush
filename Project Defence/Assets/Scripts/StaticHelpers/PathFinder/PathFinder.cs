using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StaticHelpers.PathFinder
{
    public static class PathFinder
    {
        #region CasualA*
        public static Vector2Int[] CalculatePath(GroundTile[,] tileMap, Vector2Int startPoint, Vector2Int endPoint)
        {
            //can be more optimized!!
            List<GroundTile> openList = new List<GroundTile> { tileMap[startPoint.x, startPoint.y] };
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
                        neighbor.h = CalculateH(neighbor, tileMap[endPoint.x, endPoint.y]);
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
        public static Vector2Int[] IgnoreBlockedTilesAndCalculatePath(GroundTile[,] tileMap, Vector2Int startPoint, Vector2Int endPoint)
        {
            //can be more optimized!!
            List<GroundTile> openList = new List<GroundTile> { tileMap[startPoint.x, startPoint.y] };
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
                        neighbor.h = CalculateH(neighbor, tileMap[endPoint.x, endPoint.y]);
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
        public static Vector2Int[] CalculateUntillFindClosestAvailablePath(GroundTile[,] tileMap, Vector2Int startPoint, Vector2Int endPoint)
        {
            //can be more optimized!!
            List<GroundTile> openList = new List<GroundTile> { tileMap[startPoint.x, startPoint.y] };
            List<GroundTile> closedList = new List<GroundTile>();
            if (tileMap[endPoint.x, endPoint.y].IsBlocked)
            {
                endPoint = FindAvailableClosestTilePos(startPoint, endPoint, tileMap);
            }
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
                        neighbor.h = CalculateH(neighbor, tileMap[endPoint.x, endPoint.y]);
                        neighbor.parent = current;
                        if (!isInOpenList)
                            openList.Add(neighbor);
                    }
                }
            }
            return null;
        }

        #endregion
        public static GroundTile[] GetFourAxisNeighbors(GroundTile tile, GroundTile[,] tileMap, int layer)
        {
            GroundTile[] neighbors = new GroundTile[4];
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { 1, -1, 0, 0 };// i mean z axis because 3d represent.
            for (int i = 0; i < 4; i++)
            {
                Vector2Int newPos = new Vector2Int(tile.GridPosition.x + dx[i] * layer, tile.GridPosition.y + dy[i] * layer);
                if (IsPositionInTileMapBounds(new Vector2Int(newPos.x, newPos.y), tileMap))//check is groundTileValid
                {
                    neighbors[i] = tileMap[newPos.x, newPos.y];
                }
            }
            neighbors = DeleteNullVarsAndTrimNeighborArray(neighbors);
            return neighbors;

        }
        public static Vector2Int[] GetFourAxisNeighborPositions(Vector2Int tilePosition, int layer)
        {
            Vector2Int[] neighborPositions = new Vector2Int[4];
            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { 1, -1, 0, 0 };
            for (int i = 0; i < 4; i++)
            {
                Vector2Int newPos = new Vector2Int(tilePosition.x + dx[i] * layer, tilePosition.y + dy[i] * layer);
                neighborPositions[i] = newPos;
            }
            return neighborPositions;

        }
        public static Vector2Int[] GetEightAxisNeighborPositions(Vector2Int tilePosition, int layer)
        {
            Vector2Int[] neighborPositions = new Vector2Int[8];
            int[] dx = { 0, 0, -1, 1, 1, 1, -1, -1 };
            int[] dy = { 1, -1, 0, 0, 1, -1, -1, 1 };
            for (int i = 0; i < 8; i++)
            {
                Vector2Int newPos = new Vector2Int(tilePosition.x + dx[i] * layer, tilePosition.y + dy[i] * layer);
                neighborPositions[i] = newPos;
            }
            return neighborPositions;
        }
        public static GroundTile[] GetEightAxisNeighbors(GroundTile tile, GroundTile[,] tileMap, int layer)
        {
            GroundTile[] neighbors = new GroundTile[8];
            int[] dx = { 0, 0, -1, 1, 1, 1, -1, -1 };
            int[] dy = { 1, -1, 0, 0, 1, -1, -1, 1 };// i mean z axis because 3d represent.
            for (int i = 0; i < 8; i++)
            {
                Vector2Int newPos = new Vector2Int(tile.GridPosition.x + dx[i] * layer, tile.GridPosition.y + dy[i] * layer);
                if (IsPositionInTileMapBounds(new Vector2Int(newPos.x, newPos.y), tileMap))//check is groundTileValid
                {
                    neighbors[i] = tileMap[newPos.x, newPos.y];
                }
            }
            neighbors = DeleteNullVarsAndTrimNeighborArray(neighbors);
            return neighbors;

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
        private static bool IsPositionInTileMapBounds(Vector2Int position, GroundTile[,] tileMap)
        {
            return position.x >= 0 && position.x < tileMap.GetLength(0) && position.y >= 0 && position.y < tileMap.GetLength(1);
        }
        private static bool IsTilemapPositionBlocked(Vector2Int position, GroundTile[,] tileMap)
        {
            return tileMap[position.x, position.y].IsBlocked;
        }
        private static int CalculateH(GroundTile firstTile, GroundTile secondTile)
        {
            return Mathf.Abs(firstTile.GridPosition.x - secondTile.GridPosition.x) + Mathf.Abs(firstTile.GridPosition.y - secondTile.GridPosition.y);
        }
        private static Vector2Int FindAvailableClosestTilePos(Vector2Int startPoint, Vector2Int endPoint, GroundTile[,] tileMap)
        {
            int tileMapMaxRadius = Mathf.Max(tileMap.GetLength(0), tileMap.GetLength(1));
            GroundTile availableClosestNeighbor = null;
            GroundTile[] neighborCalculationResult;
            int multiplier = 1;
            while (availableClosestNeighbor == null && multiplier < tileMapMaxRadius)
            {
                //sort them
                neighborCalculationResult = GetFourAxisNeighbors(tileMap[endPoint.x, endPoint.y], tileMap, multiplier);
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

