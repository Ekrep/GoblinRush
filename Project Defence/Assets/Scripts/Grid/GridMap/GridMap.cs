using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using StaticHelpers.PathFinder;
using StaticHelpers.Random;
using Scriptables.MapCreation.MapData;
using System;
using static StaticHelpers.Util.Utils;
using System.Linq;
using PoolSystem;

public partial class GridMap : SerializedMonoBehaviour
{
    private static GridMap instance;
    public static GridMap Instance => instance;
    public MapData data;
    private Dictionary<Vector2Int, GroundTile> currentTileMap = new Dictionary<Vector2Int, GroundTile>();//It must be a dictionary because the maps are created dynamically!
    public Dictionary<Vector2Int, GroundTile> CurrentTileMap => currentTileMap;
    private List<BoundableProbe> currentBoundables = new List<BoundableProbe>();
    public List<BoundableProbe> CurrentBoundables => currentBoundables;
    [SerializeField] private Vector3 tileScale;
    public Vector3 TileScale => tileScale;
    [SerializeField] private float cellXOffset;
    public float CellXOffset => cellXOffset;
    [SerializeField] private float cellZOffset;
    public float CellZOffset => cellZOffset;
    public GridPath[] paths;

    private GameObject mapParent;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void LoadMap(MapData mapData)
    {
        if (mapParent == null)
        {
            mapParent = new GameObject("MapParent");
        }
        for (int i = 0; i < mapData.placedTiles.Length; i++)
        {
            PoolManager.Instance.TryCreatePool(mapData.placedTiles[i].GroundTile.GetType().Name, mapData.placedTiles[i].GroundTile, 5, mapParent.transform);
        }
        for (int i = 0; i < mapData.boundedBoundableDatas.Length; i++)
        {
            PoolManager.Instance.TryCreatePool(mapData.boundedBoundableDatas[i].Probe.GetType().Name, mapData.boundedBoundableDatas[i].Probe, 3, mapParent.transform);
        }
        for (int i = 0; i < currentBoundables.Count; i++)
        {
            PoolManager.Instance.EnqueueItemToPool(currentBoundables[i].GetType().Name, currentBoundables[i]);
        }
        currentBoundables = null;
        for (int i = 0; i < currentTileMap.Count; i++)
        {
            PoolManager.Instance.EnqueueItemToPool(currentTileMap.ElementAt(i).Value.GetType().Name, currentTileMap.ElementAt(i).Value);
        }
        currentTileMap = null;
        paths = null;
        InitializeMap(mapData);
    }
    private void InitializeMap(MapData mapData)
    {
        currentTileMap = new Dictionary<Vector2Int, GroundTile>(mapData.placedTiles.Length);
        currentBoundables = new List<BoundableProbe>(mapData.boundedBoundableDatas.Length);
        cellXOffset = mapData.cellXOffset;
        cellZOffset = mapData.cellZOffset;
        tileScale = mapData.tileScale;
        Vector3 minPos = TilePositionToWorldPosition(mapData.minBounds);
        Vector3 maxPos = TilePositionToWorldPosition(mapData.maxBounds);
        Vector3 dist = maxPos - minPos;
        mapParent.transform.position = dist / 2f;
        for (int i = 0; i < mapData.placedTiles.Length; i++)
        {
            PoolManager.Instance.TryDequeueItemFromPool(mapData.placedTiles[i].GroundTile.GetType().Name, out GroundTile groundTile);
            currentTileMap.TryAdd(mapData.placedTiles[i].GridPos, groundTile);
            groundTile.SetTileScale(tileScale);
            groundTile.gameObject.name = mapData.placedTiles[i].GridPos.ToString();
            groundTile.SetGridPosition(mapData.placedTiles[i].GridPos);
            groundTile.SetWorldPosition(new Vector3(mapData.placedTiles[i].PlacedWorldPosition.x * cellXOffset, mapData.placedTiles[i].PlacedWorldPosition.y, mapData.placedTiles[i].PlacedWorldPosition.z * cellZOffset));
            groundTile.transform.SetParent(mapParent.transform);//it may cause bug!
            groundTile.Initialize();
        }
        for (int i = 0; i < mapData.boundedBoundableDatas.Length; i++)
        {
            PoolManager.Instance.TryDequeueItemFromPool(mapData.boundedBoundableDatas[i].Probe.GetType().Name, out BoundableProbe probe);
            probe.SetScale(tileScale);
            probe.transform.position = new Vector3(mapData.boundedBoundableDatas[i].BoundedWorldPosition.x * cellXOffset, mapData.boundedBoundableDatas[i].BoundedWorldPosition.y * tileScale.y, mapData.boundedBoundableDatas[i].BoundedWorldPosition.z * cellZOffset);
            probe.Initialize(mapData.boundedBoundableDatas[i].BoundedTilePositions, mapData.boundedBoundableDatas[i].BoundablePivotPoint);
            probe.transform.SetParent(mapParent.transform);
            currentBoundables.Add(probe);
        }
        //set path datas(reference type)
        InitializePaths(mapData);

    }
    private void InitializePaths(MapData mapData)
    {
        PathData[] pathDatas = mapData.pathDatas;
        paths = new GridPath[pathDatas.Length];
        for (int i = 0; i < pathDatas.Length; i++)
        {
            paths[i] = new GridPath(pathDatas[i]);
        }
        GridMapInitialized(mapParent.transform, mapData);
        LevelLoader.Instance.LevelLoaded();
    }
    public bool TryToGetPathWithStartPosition(Vector2Int pathStartPos, out GridPath path)
    {
        for (int i = 0; i < paths.Length; i++)
        {
            if (pathStartPos == paths[i].PathStartGridPos)
            {
                path = paths[i];
                if (path.NeedsCalculation)
                {
                    path.RecalculatePath(currentTileMap);
                }
                return true;
            }
        }
        path = null;
        return false;
    }

}
#region Helpers
public partial class GridMap
{

    #region  Map Helper Funcitons
    public Vector3 TilePositionToWorldPosition(Vector2Int tilePosition)
    {
        return new Vector3(tilePosition.x * cellXOffset, 0, tilePosition.y * cellZOffset);
    }
    public Vector2Int WorldPositionToTilePosition(Vector3 worldPosition)
    {
        //returns center pos of tile
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x / cellXOffset), Mathf.RoundToInt(worldPosition.z / cellZOffset));
    }
    public GroundTile GetTileByWorldPos(Vector3 tileWorldPos)
    {
        //check later
        Vector2Int gridPos = WorldPositionToTilePosition(tileWorldPos);
        if (currentTileMap.TryGetValue(gridPos, out GroundTile groundTile))
        {
            return groundTile;
        }
        else
        {
            return null;
        }


    }
    public GroundTile GetTileByGridPos(Vector2Int tileGridPos)
    {
        if (currentTileMap.TryGetValue(tileGridPos, out GroundTile groundTile))
        {
            return groundTile;
        }
        else
        {
            return null;
        }
    }
    public Vector3 GetRandomPointInsideTheTile(Vector3 tileWorldPos)
    {
        float tileXScaleRatio = tileScale.x * 0.5f;//based on unity's default cube.
        float tileZScaleRatio = tileScale.z * 0.5f;
        Vector3 position = new Vector3(tileWorldPos.x + RandomTable.GetRandomFloatBetweenMinusOneToOne() * tileXScaleRatio, 0, tileWorldPos.z +
        RandomTable.GetRandomFloatBetweenMinusOneToOne() * tileZScaleRatio);
        return position;
    }
    public Vector3 GetRandomPointInsideTheTile(Vector2Int tilePos)
    {
        Vector3 tileWorldPos = TilePositionToWorldPosition(tilePos);
        float tileXScaleRatio = tileScale.x * 0.5f;//based on unity's default cube.
        float tileZScaleRatio = tileScale.z * 0.5f;
        Vector3 position = new Vector3(tileWorldPos.x + RandomTable.GetRandomFloatBetweenMinusOneToOne() * tileXScaleRatio, 0, tileWorldPos.z +
        RandomTable.GetRandomFloatBetweenMinusOneToOne() * tileZScaleRatio);
        return position;

    }
    public bool IsPositionValidOnTilemap(Vector2Int tilePos)
    {
        return currentTileMap.ContainsKey(tilePos);
    }
    public bool IsPositionValidOnTilemap(Vector3 worldPos)
    {
        Vector2Int tilePos = WorldPositionToTilePosition(worldPos);
        return currentTileMap.ContainsKey(tilePos);
    }
    public int TileManhattanDistance(Vector2Int tile1, Vector2Int tile2)
    {
        return Mathf.Abs(tile1.x - tile2.x) + Mathf.Abs(tile1.y - tile2.y);
    }
    public Vector2Int[] GetFourAxisNeighbors(Vector2Int tilePos)
    {
        return PathFinder.GetFourAxisNeighborPositions(tilePos);
    }
    public Vector2Int[] GetEightAxisNeighbors(Vector2Int tilePos)
    {
        return PathFinder.GetFourAxisNeighborPositions(tilePos);

    }
    #endregion

}
#endregion

#region Events
public partial class GridMap
{
    public static event Action<Transform, MapData> OnGridMapInitialized;
    public static event Action OnTileModified;

    public void GridMapInitialized(Transform mapParent, MapData mapData)
    {
        if (OnGridMapInitialized != null)
        {
            OnGridMapInitialized(mapParent, mapData);
        }

    }
    public void TileModified()
    {
        if (OnTileModified != null)
        {
            OnTileModified();
        }
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i].SetNeedsRecalculation(true);
        }
    }


}
#endregion