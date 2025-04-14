using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scriptables.Towers;
using StaticHelpers.PathFinder;
using UnityEngine;

public abstract class Tower : BoundableProbe
{
    protected TowerData towerData;
    public TowerData TowerData => towerData;
    protected List<BaseTile> tilesInRange;
    public abstract void UnboundFromTileProcess();
    public virtual void Initialize(Vector2Int[] occupiedTilePositions, Vector2Int towerPivotPoint)
    {
        OccupyTile(occupiedTilePositions);
        tilesInRange=GetTilesInRange();
    }

    public override void OnUnbound()
    {
        UnboundFromTileProcess();
    }
    protected List<BaseTile> GetTilesInRange()
    {
        GroundTile pivotTile = GridMap.Instance.GetTileByGridPos(probePivotPoint);
        List<BaseTile> tilesInRange = new List<BaseTile>();
        for (int i = 1; i < towerData.range; i++)
        {
            BaseTile[] holderArray = PathFinder.GetEightAxisNeighbors(pivotTile, GridMap.Instance.CurrentTileMap, i);
            for (int j = 0; j < holderArray.Length; j++)
            {
                tilesInRange.Add(holderArray[j]);
            }
        }
        return tilesInRange;

    }
}
