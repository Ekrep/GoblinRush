using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GridMap : SerializedMonoBehaviour
{
    private static GridMap instance;
    public static GridMap Instance => instance;
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
        InitializeTileArray();
        CreateGridCells();
        CreateVisual();
        Debug.Log(TilePositionToWorldPosition(new Vector2Int(3, 3)));
        Debug.Log(WorldPositionToTilePosition(new Vector3(13.5f, 0, 24f)));
        #region TestStuff
        currentTileMap[3, 3].SetTileBlockStatus(true);
        currentTileMap[0, 1].SetTileBlockStatus(true);
        currentTileMap[1, 1].SetTileBlockStatus(true);
        currentTileMap[3, 2].SetTileBlockStatus(true);
        currentTileMap[2, 3].SetTileBlockStatus(true);
        currentTileMap[3, 4].SetTileBlockStatus(true);
        currentTileMap[4, 3].SetTileBlockStatus(true);
        path = PathFinder.CalculateUntillFindClosestAvailablePath(currentTileMap, new Vector2Int(0, 0), new Vector2Int(3, 3));
        for (int i = 0; i < path.Length; i++)
        {
            currentTileMap[path[i].x, path[i].y].tileRenderer.material.color = Color.red;
        }
        #endregion
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Vector3 pos = GetRandomPointInsideTheTile(TilePositionToWorldPosition(new Vector2Int(2, 2)));
            cube.transform.position = new Vector3(pos.x, 1.5f, pos.z);
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
    private void InitializeTileArray()
    {
        cells = new GridCell[gridSizeX, gridSizeZ];
        testDict = new Dictionary<Vector2Int, GridCell>();
        currentTileMap = new GroundTile[gridSizeX, gridSizeZ];
    }
    private void CreateVisual()
    {
        for (int z = 0; z < cells.GetLength(1); z++)
        {
            for (int x = 0; x < cells.GetLength(0); x++)
            {
                GroundTile tile = Instantiate(tilePrefab);
                tile.SetGridPosition(cells[x, z].GridPos);
                tile.SetWorldPosition(cells[x, z].WorldPos);
                tile.SetTileScale(tileScale);
                currentTileMap[x, z] = tile;
            }
        }

    }
    private void CreateGridCells()
    {
        Vector2Int gridPos;
        Vector3 worldPos;
        for (int z = 0; z < gridSizeZ; z++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                gridPos = new Vector2Int(x, z);
                worldPos = new Vector3(x * cellXOffset, 0f, z * cellZOffset);
                GridCell cell = new GridCell(gridPos, worldPos);
                cells[x, z] = cell;
                testDict.Add(gridPos, cell);
            }
        }

    }

}
