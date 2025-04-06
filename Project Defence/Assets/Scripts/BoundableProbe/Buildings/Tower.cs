using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scriptables.Towers;
using UnityEngine;

public abstract class Tower : BoundableProbe
{
    [SerializeField] protected TowerData towerData;
    public TowerData TowerData => towerData;
    private Vector2Int towerPivotPoint;
    public abstract void UnboundFromTileProcess();
    public virtual void Initialize(Vector2Int[] occupiedTilePositions, Vector2Int towerPivotPoint)
    {
        OccupyTile(occupiedTilePositions);
    }

    public override void OnUnbound()
    {
        UnboundFromTileProcess();
    }
    protected BaseTile[] GetTilesInRange()
    {
        GroundTile pivotTile = GridMap.Instance.GetTileByGridPos(towerPivotPoint);
        BaseTile[] tilesInRange = new BaseTile[towerData.range * 8];
        for (int i = 1; i < towerData.range; i++)
        {
            BaseTile[] holderArray = PathFinder.GetEightAxisNeighbors(pivotTile, GridMap.Instance.CurrentTileMap, i);
            for (int j = 0; j < holderArray.Length; j++)
            {
                tilesInRange.Append(holderArray[j]);
            }
        }
        return tilesInRange;

    }
}
