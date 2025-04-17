using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using StaticHelpers.PathFinder;
using StaticHelpers.Random;
using Scriptables.MapCreation.MapData;

public class GridMap : SerializedMonoBehaviour
{
    private static GridMap instance;
    public static GridMap Instance => instance;
    public MapData data;
    private GridCell[,] cells;//deprecated remove it!!
    private GroundTile[,] currentTileMap;
    public GroundTile[,] CurrentTileMap => currentTileMap;
    public Dictionary<Vector2Int, GridCell> testDict;
    [SerializeField] private int gridSizeX;
    [SerializeField] private int gridSizeZ;
    [SerializeField] private Vector3 tileScale;
    public Vector3 TileScale => tileScale;
    [SerializeField] private float cellXOffset;
    [SerializeField] private float cellZOffset;
    [SerializeField] private GroundTile tilePrefab;
    public Vector2Int[] path;
    public GameObject cube;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        ground.Raycast(ray, out float enter);
        Debug.Log(ray.GetPoint(enter));
        Vector2Int tilePos = WorldPositionToTilePosition(ray.GetPoint(enter));
        if (tilePos.x > 0 && tilePos.y > 0 && tilePos.x < currentTileMap.GetLength(0) && tilePos.y < currentTileMap.GetLength(1) && currentTileMap[tilePos.x, tilePos.y] != null)
        {
            currentTileMap[tilePos.x, tilePos.y].tileRenderer.material.color = Color.red;
        }

    }
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
    private void InitializeMap(MapData mapData)
    {
        currentTileMap = new GroundTile[mapData.placedTiles.Length, mapData.placedTiles.Length];
        cellXOffset = mapData.cellXOffset;
        cellZOffset = mapData.cellZOffset;
        tileScale = mapData.tileScale;
        for (int i = 0; i < mapData.placedTiles.Length; i++)
        {
            GroundTile groundTile = Instantiate(mapData.placedTiles[i].GroundTile);
            currentTileMap[mapData.placedTiles[i].GridPos.x, mapData.placedTiles[i].GridPos.y] = groundTile;
            groundTile.SetGridPosition(mapData.placedTiles[i].GridPos);
            groundTile.SetWorldPosition(mapData.placedTiles[i].PlacedWorldPosition);
            groundTile.SetTileScale(tileScale);
        }
        for (int i = 0; i < mapData.boundedBoundableDatas.Length; i++)
        {
            BoundableProbe probe = Instantiate(mapData.boundedBoundableDatas[i].Probe);
            probe.transform.position = mapData.boundedBoundableDatas[i].BoundedWorldPosition;
            probe.OccupyTile(mapData.boundedBoundableDatas[i].BoundedTilePositions);
        }
    }
    public bool IsPositionValidOnTilemap(Vector2Int tilePos)
    {
        return tilePos.x > 0 && tilePos.y > 0 && tilePos.x < currentTileMap.GetLength(0) && tilePos.y < currentTileMap.GetLength(1) && currentTileMap[tilePos.x, tilePos.y] != null
    }
    public bool IsPositionValidOnTilemap(Vector3 worldPos)
    {
        Vector2Int tilePos = WorldPositionToTilePosition(worldPos);
        return tilePos.x > 0 && tilePos.y > 0 && tilePos.x < currentTileMap.GetLength(0) && tilePos.y < currentTileMap.GetLength(1) && currentTileMap[tilePos.x, tilePos.y] != null
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

}
