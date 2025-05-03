using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticHelpers.Util.Utils;
using StaticHelpers.PathFinder;

public class GridPath
{
    [SerializeField] private uint path_ID;
    public uint Path_ID => path_ID;

    [SerializeField] private Vector2Int[] path;
    public Vector2Int[] Path => path;

    [SerializeField] private HashSet<Vector2Int> pathTiles;//<=check this(something happend on a tile?) also its cannot be serialized!!
    public HashSet<Vector2Int> PathTiles => pathTiles;

    [SerializeField] private PathStatus pathStatus;
    public PathStatus PathStatus => pathStatus;

    [SerializeField] private Vector2Int pathStartGridPos;
    public Vector2Int PathStartGridPos => pathStartGridPos;

    [SerializeField] private Vector2Int pathEndGridPos;
    public Vector2Int PathEndGridPos => pathEndGridPos;

    private bool isStartable;
    public bool IsStartable => isStartable;
    private bool needsCalculation;
    public bool NeedsCalculation => needsCalculation;
    public GridPath(PathData pathData)
    {
        path_ID = pathData.Path_ID;
        path = pathData.Path;
        //pathTiles = new HashSet<Vector2Int>(path.Length);
        //AddPathPositionsToTheHashSet();
        pathStatus = pathData.PathStatus;
        pathStartGridPos = pathData.PathStartGridPos;
        pathEndGridPos = pathData.PathEndGridPos;
        isStartable = pathData.IsStartable;
    }
    public void RecalculatePath(Dictionary<Vector2Int, GroundTile> tileMap)//ask here?
    {
        //path kullanilacagi zaman tekrar hesaplat
        if (!tileMap[path[0]].IsBlocked)
        {
            isStartable = true;
        }
        else
        {
            isStartable = false;
            return;
        }
        path = PathFinder.CalculateUntillFindClosestAvailablePath(tileMap, pathStartGridPos, pathEndGridPos);
        //pathTiles.Clear();
        //AddPathPositionsToTheHashSet();
        if (path != null && path.Length > 0 && Vector2Int.Distance(path[path.Length - 1], pathEndGridPos) < 3f)
        {
            pathStatus = PathStatus.Reachable;
        }
        else
        {
            pathStatus = PathStatus.Blocked;
        }

    }
    public bool PathContainsTile(Vector2Int tilePos)
    {
        return pathTiles.Contains(tilePos);
    }
    public void SetNeedsRecalculation(bool needs)
    {
        needsCalculation = needs;
    }
}
