using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTile : MonoBehaviour
{
    protected BoundableProbe boundableProbe;
    public BoundableProbe BoundableProbe => boundableProbe;
    [SerializeField] protected bool isBlocked;
    public bool IsBlocked => isBlocked;
    protected bool isOccupied;
    public bool IsOccupied => isOccupied;
    public Vector2Int gridPosition;
    public Vector3 worldPosition;
    public MeshRenderer tileRenderer;
    public MeshFilter tileFilter;

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
        boundableProbe.OnUnbound();
        boundableProbe = null;
        isOccupied = false;
        SetTileBlockStatus(false);
    }
}
