using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PoolSystem;
using PoolSystem.Poolable;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Scriptables.UnitData.GoblinData;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Spawnables spawnables;
    [SerializeField] private string currentSpawnObjectName;
    [SerializeField] private int currentSpawnObjectIndex;
    public string CurrentSpawnObjectName => currentSpawnObjectName;
    private Plane ground;
    private bool canSpawn = true;
    public bool CanSpawn => canSpawn;
    private void OnEnable()
    {
        UIManager.OnUnitSelectedFromUnitTable += UIManager_OnUnitSelectedFromUnitTable;
        GameManager.OnGameEnd += GameManager_OnGameEnd;
        GridMap.OnTileModified += GridMap_OnTileModified;
    }
    private void UIManager_OnUnitSelectedFromUnitTable(string name, int spawnablesIndex)
    {
        currentSpawnObjectName = name;
        currentSpawnObjectIndex = spawnablesIndex;
    }
    private void GameManager_OnGameEnd(bool isWin)
    {
        currentSpawnObjectName = "";
        currentSpawnObjectIndex = -1;
    }
    private void GridMap_OnTileModified()
    {
        SetCanSpawn(true);
    }
    private void OnDisable()
    {
        UIManager.OnUnitSelectedFromUnitTable -= UIManager_OnUnitSelectedFromUnitTable;
        GameManager.OnGameEnd -= GameManager_OnGameEnd;
        GridMap.OnTileModified -= GridMap_OnTileModified;
    }
    void Start()
    {
        Initialize();
    }
    private void Update()
    {
        if (InputController.Instance.Inputs.Gameplay.MouseLeftClick.WasPerformedThisFrame() && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
            ground.Raycast(ray, out float enter);
            Vector2Int tilePos = GridMap.Instance.WorldPositionToTilePosition(ray.GetPoint(enter));
            if (canSpawn && currentSpawnObjectName != string.Empty && GameManager.Instance.CanBuyUnit(spawnables.spawnablesDatas[currentSpawnObjectIndex].prefab.UnitData.trainCost)
            && GridMap.Instance.CurrentTileMap.ContainsKey(tilePos) && GridMap.Instance.TryToGetPathWithStartPosition(tilePos, out GridPath path))
            {
                BaseGoblinUnit unit = (BaseGoblinUnit)SpawnObject<Unit>(currentSpawnObjectName, tilePos);
                GameManager.Instance.PlayerBoughtUnit(unit.GoblinData.trainCost);
                unit.SetPath(path);
                unit.Initialize();
                GameManager.Instance.GoblinSpawned();
                if (unit.GoblinData.goblinType == StaticHelpers.Util.Utils.GoblinType.Breach)
                {
                    SetCanSpawn(false);
                }
            }
        }
    }
    private void Initialize()
    {
        ground = new Plane(Vector3.up, Vector3.zero);
        CreatePools();
        GameManager.Instance.SetCheepestGoblinPrice(GetCheepestGoblinPrice());
    }
    private void SetCanSpawn(bool canSpawn)
    {
        this.canSpawn = canSpawn;
    }
    private int GetCheepestGoblinPrice()
    {
        int price = 9999;
        for (int i = 0; i < spawnables.spawnablesDatas.Count; i++)
        {
            if (spawnables.spawnablesDatas[i].prefab.UnitData.trainCost < price)
            {
                price = spawnables.spawnablesDatas[i].prefab.UnitData.trainCost;
            }
        }
        return price;
    }
    private void CreatePools()
    {
        for (int i = 0; i < spawnables.spawnablesDatas.Count; i++)
        {
            PoolManager.Instance.CreatePool(spawnables.spawnablesDatas[i].prefab.UnitData.unitName, spawnables.spawnablesDatas[i].prefab,
            spawnables.spawnablesDatas[i].amount, null);
        }

    }
    public T SpawnObject<T>(string poolName, Vector2Int spawnTilePos) where T : MonoBehaviour, IPoolable
    {
        if (poolName != null)
        {
            T obj = PoolManager.Instance.DequeueItemFromPool<T>(poolName);
            obj.transform.position = GridMap.Instance.TilePositionToWorldPosition(spawnTilePos);
            return obj;
        }
        return null;
    }

    public T[] SpawnObject<T>(string poolName, Vector2Int[] spawnTilePositions) where T : MonoBehaviour, IPoolable
    {
        if (poolName != null)
        {
            T[] spawnedObjects = new T[spawnTilePositions.Length];
            for (int i = 0; i < spawnTilePositions.Length; i++)
            {
                T obj = PoolManager.Instance.DequeueItemFromPool<T>(poolName);
                obj.transform.position = GridMap.Instance.TilePositionToWorldPosition(spawnTilePositions[i]);
                spawnedObjects[i] = obj;
            }

            return spawnedObjects;
        }
        else
        {
            return null;
        }

    }


}
