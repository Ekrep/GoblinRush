using System.Collections;
using System.Collections.Generic;
using PoolSystem.Poolable;
using UnityEngine;
public abstract class BaseTile : MonoBehaviour, IPoolable
{
    [SerializeField] protected BoundableProbe boundableProbe;
    public BoundableProbe BoundableProbe => boundableProbe;
    [SerializeField] protected bool isBlocked;
    public bool IsBlocked => isBlocked;
    protected bool isOccupied;
    public bool IsOccupied => isOccupied;
    protected Vector2Int gridPosition;
    public Vector2Int GridPosition => gridPosition;

    protected Vector3 worldPosition;
    public Vector3 WorldPosition => worldPosition;
    public MeshRenderer tileRenderer;
    public MeshFilter tileFilter;

    public virtual void Initialize()
    {
        //currently empty
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    public void SetWorldPosition(Vector3 worldPosition)
    {
        transform.position = worldPosition;
        this.worldPosition = worldPosition;
    }
    public void SetGridPosition(Vector2Int gridPos)
    {
        gridPosition = gridPos;
    }
    public void SetTileScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
    public void SetTileBlockStatus(bool blocked)
    {
        isBlocked = blocked;
    }
    public void BoundTheBoundable(BoundableProbe probe, bool canBlockTile)
    {
        boundableProbe = probe;
        isOccupied = true;
        SetTileBlockStatus(canBlockTile);
    }
    public void UnboundTheBoundable()
    {
        if (boundableProbe != null)
        {
            boundableProbe.OnUnbound();
        }
        boundableProbe = null;
        isOccupied = false;
        SetTileBlockStatus(false);
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
        gameObject.SetActive(false);
        boundableProbe = null;
    }

    public virtual void OnDequeuePool()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnDeletePool()
    {
        //throw new System.NotImplementedException();
    }
}
