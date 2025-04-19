using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using StaticHelpers.PathFinder;
using StaticHelpers.Random;
using Scriptables.MapCreation.MapData;
using System;
using static StaticHelpers.MapCreationUtils.MapCreationUtils;
using static StaticHelpers.Util.Utils;

public partial class GridMap : SerializedMonoBehaviour
{
    private static GridMap instance;
    public static GridMap Instance => instance;
    public MapData data;
    [SerializeField] private PlacedTileData[] placedTileDatas;
    private GroundTile[,] currentTileMap;
    public GroundTile[,] CurrentTileMap => currentTileMap;
    [SerializeField] private Vector3 tileScale;
    public Vector3 TileScale => tileScale;
    [SerializeField] private float cellXOffset;
    [SerializeField] private float cellZOffset;
    private Plane ground;
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
    void Start()
    {
        InitializeMap(data);
        ground = new Plane(Vector3.up, Vector3.zero);
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputController.Instance.mousePos);
        ground.Raycast(ray, out float enter);
        //Debug.Log(ray.GetPoint(enter));
        Vector2Int tilePos = WorldPositionToTilePosition(ray.GetPoint(enter));
        if (tilePos.x > 0 && tilePos.y > 0 && tilePos.x < currentTileMap.GetLength(0) && tilePos.y < currentTileMap.GetLength(1) && currentTileMap[tilePos.x, tilePos.y] != null)
        {
            currentTileMap[tilePos.x, tilePos.y].tileRenderer.material.color = Color.red;
        }

    }

    private void InitializeMap(MapData mapData)
    {
        GameObject mapParentObj = new GameObject("MapParent");
        currentTileMap = new GroundTile[mapData.placedTiles.Length, mapData.placedTiles.Length];
        cellXOffset = mapData.cellXOffset;
        cellZOffset = mapData.cellZOffset;
        tileScale = mapData.tileScale;
        placedTileDatas = mapData.placedTiles;
        mapParentObj.transform.position = mapData.placedTiles[mapData.placedTiles.Length / 2].PlacedWorldPosition;//temporary fix!!
        for (int i = 0; i < mapData.placedTiles.Length; i++)
        {
            GroundTile groundTile = Instantiate(mapData.placedTiles[i].GroundTile);
            currentTileMap[mapData.placedTiles[i].GridPos.x, mapData.placedTiles[i].GridPos.y] = groundTile;
            groundTile.SetTileScale(tileScale);
            groundTile.SetGridPosition(mapData.placedTiles[i].GridPos);
            groundTile.SetWorldPosition(new Vector3(mapData.placedTiles[i].PlacedWorldPosition.x * cellXOffset, mapData.placedTiles[i].PlacedWorldPosition.y, mapData.placedTiles[i].PlacedWorldPosition.z * cellZOffset));
            groundTile.transform.SetParent(mapParentObj.transform);//it may cause bug!
        }
        for (int i = 0; i < mapData.boundedBoundableDatas.Length; i++)
        {
            BoundableProbe probe = Instantiate(mapData.boundedBoundableDatas[i].Probe);
            probe.SetScale(tileScale);
            probe.transform.position = new Vector3(mapData.boundedBoundableDatas[i].BoundedWorldPosition.x * cellXOffset, mapData.boundedBoundableDatas[i].BoundedWorldPosition.y * tileScale.y, mapData.boundedBoundableDatas[i].BoundedWorldPosition.z * cellZOffset);
            probe.Initialize(mapData.boundedBoundableDatas[i].BoundedTilePositions);
            probe.transform.SetParent(mapParentObj.transform);
        }
        OnGridMapInitialized(mapParentObj.transform);
    }
    private void InitializePaths(MapData mapData)
    {
        PathData[] paths=new PathData[mapData.possiblePathStartPoints.Length];


    }
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
        Vector2Int gridPos = new Vector2Int((int)(tileWorldPos.x / cellXOffset), (int)(tileWorldPos.z / cellZOffset));
        return currentTileMap[gridPos.x, gridPos.y];

    }
    public GroundTile GetTileByGridPos(Vector2Int tileGridPos)
    {
        return currentTileMap[tileGridPos.x, tileGridPos.y];
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
        return tilePos.x > 0 && tilePos.y > 0 && tilePos.x < currentTileMap.GetLength(0) && tilePos.y < currentTileMap.GetLength(1) && currentTileMap[tilePos.x, tilePos.y] != null;
    }
    public bool IsPositionValidOnTilemap(Vector3 worldPos)
    {
        Vector2Int tilePos = WorldPositionToTilePosition(worldPos);
        return tilePos.x > 0 && tilePos.y > 0 && tilePos.x < currentTileMap.GetLength(0) && tilePos.y < currentTileMap.GetLength(1) && currentTileMap[tilePos.x, tilePos.y] != null;
    }
    private void DebugPath(Vector2Int startPos, Vector2Int endPos)
    {
        Vector2Int[] path = PathFinder.CalculateUntillFindClosestAvailablePath(currentTileMap, startPos, endPos);
        for (int i = 0; i < path.Length; i++)
        {
            if (path[i] != null)
            {
                GetTileByGridPos(path[i]).tileRenderer.material.color = Color.red;
            }
            else
            {
                Debug.LogWarning("Path doesnt exists");
                break;
            }

        }
    }
    #endregion

}
#region Events
public partial class GridMap
{
    public static event Action<Transform> GridMapInitialized;

    public void OnGridMapInitialized(Transform mapParent)
    {
        if (GridMapInitialized != null)
        {
            GridMapInitialized(mapParent);
        }

    }


}
#endregion