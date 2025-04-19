using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Scriptables.MapCreation.MapData;
using static StaticHelpers.MapCreationUtils.MapCreationUtils;
using Sirenix.OdinInspector;
using System.Linq;
using StaticHelpers.Convertor;

[ExecuteInEditMode]
public class MapCreator : SerializedMonoBehaviour
{
    public bool enable;
    public MapData mapData;
    public MapProbesData mapProbesData;
    public float cellXOffset;
    public float cellZOffset;
    public Vector3 tileScale;
    public GroundTile currentGroundTilePrefab;
    public BoundableProbe currentBoundableProbePrefab;
    public MapCreatorMode creationMode;
    public Dictionary<Vector2Int, GroundTile> tileMap;
    public List<BoundedBoundableData> boundedBoundableDatas;
    public List<PlacedTileData> placedTileDatas;
    private Vector3 mouseWorldPos;
    private Plane ground;

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
        ground = new Plane(Vector3.up, Vector3.zero);
    }
    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
    void OnValidate()
    {
    }
    void Update()
    {
        Debug.Log(PositionConvertor.WorldPositionToTilePosition(mouseWorldPos, new Vector2(cellXOffset, cellZOffset)));
    }
    private void OnSceneGUI(SceneView sceneView)
    {
        if (!enable) return;
        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        ground.Raycast(ray, out float enterX);
        mouseWorldPos = ray.GetPoint(enterX);
        if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
        {
            //Plane ground = new Plane(Vector3.up, Vector3.zero);
            if (ground.Raycast(ray, out float enter))
            {
                Vector3 worldPos = ray.GetPoint(enter);
                Vector2Int tileMapPos = PositionConvertor.WorldPositionToTilePosition(worldPos, new Vector2(cellXOffset, cellZOffset));
                Vector3 snappedPos;
                switch (creationMode)
                {
                    case MapCreatorMode.TileMode:
                        snappedPos = PositionConvertor.TilePositionToWorldPosition(tileMapPos, new Vector2(cellXOffset, cellZOffset));
                        if (currentGroundTilePrefab != null && tileMapPos.x >= 0 && tileMapPos.y >= 0 && !tileMap.ContainsKey(tileMapPos))
                        {
                            GroundTile newTile = PrefabUtility.InstantiatePrefab(currentGroundTilePrefab) as GroundTile;
                            newTile.SetWorldPosition(snappedPos);
                            newTile.SetGridPosition(tileMapPos);
                            tileMap.Add(tileMapPos, newTile);
                            placedTileDatas.Add(new PlacedTileData(currentGroundTilePrefab, tileMapPos, snappedPos));
                        }
                        else
                        {
                            Debug.LogWarning("Tile prefab not found or position not valid");
                        }
                        break;
                    case MapCreatorMode.BuildingMode:
                        snappedPos = PositionConvertor.TilePositionToWorldPosition(tileMapPos, new Vector2(cellXOffset, cellZOffset));
                        if (currentBoundableProbePrefab != null && tileMap.ContainsKey(tileMapPos))
                        {
                            List<Vector2Int> boundPositions = CalculateBoundablesTilemapPos(tileMapPos, currentBoundableProbePrefab.boundableData);
                            if (boundPositions != null)
                            {
                                BoundableProbe boundableProbe = PrefabUtility.InstantiatePrefab(currentBoundableProbePrefab) as BoundableProbe;
                                Vector3 snappedWorldPos = new Vector3(snappedPos.x, currentBoundableProbePrefab.boundableData.height, snappedPos.z);
                                boundableProbe.transform.position = snappedWorldPos;
                                boundedBoundableDatas.Add(new BoundedBoundableData(currentBoundableProbePrefab, boundPositions.ToArray(), snappedWorldPos));
                                for (int i = 0; i < boundPositions.Count; i++)
                                {
                                    tileMap[boundPositions[i]].BoundTheBoundable(boundableProbe, currentBoundableProbePrefab.boundableData.canBlockTile);
                                }

                            }

                        }
                        else
                        {
                            Debug.LogWarning("Error on Boundable Placing");
                        }
                        break;
                    default:
                        break;
                }

            }
            e.Use();
        }
    }
    void OnDrawGizmos()
    {
        if (!enable) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(499f, 0, -1f), new Vector3(1000, 0, 0));
        Gizmos.DrawWireCube(new Vector3(-1f, 0, 499f), new Vector3(0, 0, 1000));
        switch (creationMode)
        {
            case MapCreatorMode.TileMode:
                Gizmos.color = Color.white;
                Vector2Int tileMapPos = PositionConvertor.WorldPositionToTilePosition(mouseWorldPos, new Vector2(cellXOffset, cellZOffset));
                Vector3 snappedPos = PositionConvertor.TilePositionToWorldPosition(tileMapPos, new Vector2(cellXOffset, cellZOffset));
                Gizmos.DrawWireMesh(currentGroundTilePrefab.tileFilter.sharedMesh, snappedPos, Quaternion.identity, tileScale);
                break;
            case MapCreatorMode.BuildingMode:
                Gizmos.color = Color.white;
                tileMapPos = PositionConvertor.WorldPositionToTilePosition(mouseWorldPos, new Vector2(cellXOffset, cellZOffset));
                snappedPos = PositionConvertor.TilePositionToWorldPosition(tileMapPos, new Vector2(cellXOffset, cellZOffset));
                List<Vector2Int> list = CalculateBoundablesTilemapPos(tileMapPos, currentBoundableProbePrefab.boundableData);
                if (list != null)
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }
                Gizmos.DrawWireMesh(currentBoundableProbePrefab.meshFilter.sharedMesh, new Vector3(snappedPos.x, currentBoundableProbePrefab.boundableData.height, snappedPos.z), Quaternion.identity, tileScale);
                break;
            default:
                break;
        }
    }
    [ContextMenu("Clear Map")]
    public void ClearMap()
    {
        for (int i = 0; i < boundedBoundableDatas.Count; i++)
        {
            Destroy(boundedBoundableDatas[i].Probe);
        }
        boundedBoundableDatas.Clear();
        for (int i = 0; i < placedTileDatas.Count; i++)
        {

        }
    }
    [ContextMenu("Clear All Tile Boundables")]
    public void ClearAllTileBoundables()
    {
        for (int i = 0; i < tileMap.Count; i++)
        {
            tileMap.ElementAt(i).Value.UnboundTheBoundable();
        }
        boundedBoundableDatas.Clear();

    }
    [ContextMenu("Save Map Data")]
    public void SaveMapData()
    {
        var keys = tileMap.Keys;
        Vector2Int min = new Vector2Int(keys.Min(p => p.x), keys.Min(p => p.y));
        Vector2Int max = new Vector2Int(keys.Max(p => p.x), keys.Max(p => p.y));
        //this system currently only works on square type maps!!
        Vector2Int[] possiblePathStartPoints = keys.Where(pos => pos.x == min.x || pos.x == max.x || pos.y == min.y || pos.y == max.y).ToArray();
        mapData.Save(placedTileDatas.ToArray(), cellXOffset, cellZOffset, tileScale, boundedBoundableDatas.ToArray(), min, max, possiblePathStartPoints);
    }

    //based on every boundables has same pivot point(0,0,0)
    private List<Vector2Int> CalculateBoundablesTilemapPos(Vector2Int mouseCurrentTilePos, BoundableData boundableData)
    {
        List<Vector2Int> points = new List<Vector2Int>();
        Vector2Int startPoint = mouseCurrentTilePos;
        int layer = 0;
        if (!tileMap.ContainsKey(startPoint))
        {
            return null;
        }
        while (layer < boundableData.size)
        {
            for (int i = 0; i < boundableData.size; i++)
            {
                Vector2Int pos = new Vector2Int(startPoint.x + 1 * i, startPoint.y);
                if (tileMap.ContainsKey(pos) && (!tileMap[pos].IsBlocked || !tileMap[pos].IsOccupied))
                {
                    points.Add(pos);
                }
                else
                {
                    return null;
                }
            }
            startPoint = new Vector2Int(startPoint.x, startPoint.y - 1);
            layer++;
        }
        return points;
    }

}
