using System.Collections;
using UnityEngine;
using Scriptables.UnitData;
using PoolSystem.Poolable;
using StaticHelpers.Convertor;
using UnityEngine.Rendering;
using Unity.Mathematics;
using PoolSystem;

public abstract class Unit : MonoBehaviour, IAssignableUnit, IPoolable
{
    [SerializeField] protected UnitData unitData;
    public UnitData UnitData => unitData;
    #region Stats
    [HideInInspector] public int health;
    [HideInInspector] public int attackDamage;
    [HideInInspector] public int range;
    [HideInInspector] public float movementSpeed;
    private bool isAlive;
    #endregion
    #region GridValues
    [SerializeField] protected Vector2Int currentGridPosition;
    public Vector2Int CurrentGridPosition => currentGridPosition;
    protected int assignIndex = -1;
    #endregion
    public virtual void Initialize()
    {
        InitializeUnitStats();
    }
    protected void InitializeUnitStats()
    {
        health = unitData.health;
        attackDamage = unitData.attackDamage;
        range = unitData.range;
        movementSpeed = unitData.movementSpeed;
        isAlive = true;
    }
    protected IEnumerator WalkPathCoroutine(Vector2Int[] path)
    {
        GroundTile currentTile = GridMap.Instance.GetTileByGridPos(currentGridPosition);
        int index = 0;
        Vector3 target = GridMap.Instance.TilePositionToWorldPosition(path[index]);
        target = new Vector3(target.x, transform.position.y, target.z);
        Vector2Int holderGridPos = currentGridPosition;
        Vector3 lastTargetPos = new Vector3(GridMap.Instance.TilePositionToWorldPosition(path[path.Length - 1]).x, 1f, GridMap.Instance.TilePositionToWorldPosition(path[path.Length - 1]).z);
        OnStartMoving();
        while (transform.position != lastTargetPos && isAlive)
        {
            currentGridPosition = GridMap.Instance.WorldPositionToTilePosition(transform.position);
            if (currentGridPosition != holderGridPos)
            {
                holderGridPos = currentGridPosition;
                currentTile.RemoveUnit(this, assignIndex);
                currentTile = GridMap.Instance.GetTileByGridPos(currentGridPosition);
                currentTile.AssignUnit(this);
            }
            if (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
                Vector3 direction = new Vector3(target.x - transform.position.x, 0f, target.z - transform.position.z);
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
                }
                OnMoving();
            }
            else
            {
                index++;
                if (index >= path.Length)
                {
                    break;
                }
                target = GridMap.Instance.TilePositionToWorldPosition(path[index]);
                target = new Vector3(target.x, transform.position.y, target.z);
            }
            yield return new WaitForEndOfFrame();
        }
        if (isAlive)
        {
            OnReachedTarget();
        }
    }
    protected IEnumerator WalkPathReverseCoroutine(Vector2Int[] path)
    {
        GroundTile currentTile = GridMap.Instance.GetTileByGridPos(currentGridPosition);
        int index = path.Length;
        Vector3 target = GridMap.Instance.TilePositionToWorldPosition(path[index]);
        target = new Vector3(target.x, transform.position.y, target.z);
        Vector2Int holderGridPos = currentGridPosition;
        Vector3 lastTargetPos = new Vector3(GridMap.Instance.TilePositionToWorldPosition(path[0]).x, 1f, GridMap.Instance.TilePositionToWorldPosition(path[0]).z);
        OnStartMoving();
        while (transform.position != lastTargetPos && isAlive)
        {
            currentGridPosition = GridMap.Instance.WorldPositionToTilePosition(transform.position);
            if (currentGridPosition != holderGridPos)
            {
                holderGridPos = currentGridPosition;
                currentTile.RemoveUnit(this, assignIndex);
                currentTile = GridMap.Instance.GetTileByGridPos(currentGridPosition);
                currentTile.AssignUnit(this);
            }
            if (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, movementSpeed * Time.deltaTime);
                Vector3 direction = new Vector3(target.x - transform.position.x, 0f, target.z - transform.position.z);
                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
                }
                OnMoving();
            }
            else
            {
                index--;
                if (index <= path.Length)
                {
                    break;
                }
                target = GridMap.Instance.TilePositionToWorldPosition(path[index]);
                target = new Vector3(target.x, transform.position.y, target.z);
            }
            yield return new WaitForEndOfFrame();
        }
        if (isAlive)
        {
            OnReachedTarget();
        }

    }
    protected virtual void OnStartMoving() { }
    protected virtual void OnMoving() { }
    protected virtual void OnReachedTarget() { }
    public void PassAssignedIndex(int index, Vector2Int tilePos)
    {
        assignIndex = index;
        currentGridPosition = tilePos;
    }
    public int GetInstanceId()
    {
        return gameObject.GetInstanceID();
    }
    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public bool IsAlive()
    {
        return isAlive;
    }
    public virtual void Damage(int damageAmount)
    {
        Debug.Log("GOTDAMAGE" + damageAmount);
        health = Mathf.Max(0, health - damageAmount);
        if (health == 0)
        {
            Death();
        }
    }
    public virtual void Disappear()
    {
        isAlive = false;
        GroundTile currentTile = GridMap.Instance.GetTileByGridPos(currentGridPosition);
        currentTile.RemoveUnit(this, assignIndex);
        PoolManager.Instance.EnqueueItemToPool(GetType().Name, this);
    }
    public virtual void Death()
    {
        isAlive = false;
        GroundTile currentTile = GridMap.Instance.GetTileByGridPos(currentGridPosition);
        currentTile.RemoveUnit(this, assignIndex);
        StartCoroutine(DeathAnim());
    }
    protected virtual IEnumerator DeathAnim()
    {
        Quaternion targetRot = Quaternion.Euler(90, transform.rotation.y, transform.rotation.z);
        while (Quaternion.Angle(transform.rotation, targetRot) > 1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        PoolManager.Instance.EnqueueItemToPool(GetType().Name, this);
    }
    private void GameManager_OnGameEnd(bool isWin)
    {
        Invoke(nameof(Disappear), 0.2f);
    }
    public virtual void OnCreatedForPool()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnAssignPool()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnEnqueuePool()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        GameManager.OnGameEnd -= GameManager_OnGameEnd;
    }

    public virtual void OnDequeuePool()
    {
        gameObject.SetActive(true);
        GameManager.OnGameEnd += GameManager_OnGameEnd;
    }

    public virtual void OnDeletePool()
    {
        gameObject.SetActive(false);
    }
}
