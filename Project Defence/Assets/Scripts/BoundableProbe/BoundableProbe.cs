using System.Collections;
using System.Collections.Generic;
using PoolSystem.Poolable;
using UnityEngine;

[System.Serializable]
public abstract class BoundableProbe : MonoBehaviour, IPoolable
{
    [SerializeField] protected BoundableData boundableData;
    public BoundableData BoundableData => boundableData;
    public MeshFilter meshFilter;
    public Vector2Int probePivotPoint;
    public List<GroundTile> boundedTiles;

    #region StatValues
    protected int boundableHealth;
    public int BoundableHealth => boundableHealth;
    #endregion

    public abstract void OnUnbound();
    public virtual void Initialize(Vector2Int[] occupiedTilePositions, Vector2Int boundablePivotPoint)
    {
        boundedTiles = new List<GroundTile>(occupiedTilePositions.Length);
        OccupyTile(occupiedTilePositions);
        probePivotPoint = boundablePivotPoint;
        boundableHealth = boundableData.health;
    }
    public void OccupyTile(Vector2Int tilePos)
    {
        GroundTile tile = GridMap.Instance.GetTileByGridPos(tilePos);
        Bound(tile, true);
    }
    public void OccupyTile(Vector2Int[] tilePositions)
    {
        for (int i = 0; i < tilePositions.Length; i++)
        {
            GroundTile tile = GridMap.Instance.GetTileByGridPos(tilePositions[i]);
            Bound(tile, true);
        }

    }
    private void Bound(GroundTile tile, bool canBlockTile)
    {
        tile.BoundTheBoundable(this, canBlockTile);
        boundedTiles.Add(tile);
    }
    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }
    protected virtual void SubscribeEvents()
    {
    }
    protected virtual void UnSubscribeEvents()
    {
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
        boundedTiles = null;

    }

    public virtual void OnDequeuePool()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnDeletePool()
    {
        gameObject.SetActive(false);
    }
}
