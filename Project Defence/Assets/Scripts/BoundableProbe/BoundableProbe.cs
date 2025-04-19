using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BoundableProbe : MonoBehaviour
{
    public BoundableData boundableData;
    public MeshFilter meshFilter;
    public Vector2Int probePivotPoint;
    public List<GroundTile> boundedTiles;

    public abstract void OnUnbound();
    public virtual void Initialize(Vector2Int[] occupiedTilePositions)
    {
        OccupyTile(occupiedTilePositions);
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
}
