
using System.Collections.Generic;
using Scriptables.Towers;
using StaticHelpers.PathFinder;
using UnityEngine;

public abstract class Tower : BoundableProbe
{
    protected TowerData towerData;
    public TowerData TowerData => towerData;
    public List<GroundTile> tilesinrange;
    protected HashSet<IGoblin> goblinsInRange;
    private float distanceToFurtherestRangeTile;
    public override void Initialize(Vector2Int[] occupiedTilePositions, Vector2Int towerPivotPoint)
    {
        base.Initialize(occupiedTilePositions, towerPivotPoint);
        towerData = (TowerData)boundableData;
        tilesinrange = GetTilesInRange();
        goblinsInRange = new HashSet<IGoblin>(20);//Exprerimental value
        for (int i = 0; i < tilesinrange.Count; i++)
        {
            tilesinrange[i].tileRenderer.material.color = Color.red; //for debug
        }
        SubscribeEvents();
    }
    private void GoblinEnter(IGoblin goblin, int goblinIndex, Vector2Int tile, Vector3 enterPos)
    {
        if (!goblinsInRange.Contains(goblin))
        {
            OnGoblinEnter(goblin, goblinIndex, tile, enterPos);
            goblinsInRange.Add(goblin);
        }

    }
    private void GoblinExit(IGoblin goblin, int goblinIndex, Vector2Int tile, Vector3 exitPos)
    {
        if (!goblin.IsAlive())
        {
            goblinsInRange.Remove(goblin);
            OnGoblinExit(goblin, goblinIndex, tile, exitPos);
            return;
        }
        if (goblinsInRange.Contains(goblin) && Vector3.Distance(GridMap.Instance.TilePositionToWorldPosition(probePivotPoint), exitPos) > distanceToFurtherestRangeTile)
        {
            goblinsInRange.Remove(goblin);
            OnGoblinExit(goblin, goblinIndex, tile, exitPos);
        }
    }
    protected virtual void OnGoblinEnter(IGoblin goblin, int goblinIndex, Vector2Int tile, Vector3 enterPos) { }
    protected virtual void OnGoblinExit(IGoblin goblin, int goblinIndex, Vector2Int tile, Vector3 exitPos) { }
    protected override void SubscribeEvents()
    {
        base.SubscribeEvents();
        List<GroundTile> tilesInRange = GetTilesInRange();//reduce memory useage(local)
        for (int i = 0; i < tilesInRange.Count; i++)
        {
            tilesInRange[i].OnGoblinEntered += GoblinEnter;
            tilesInRange[i].OnGoblinExit += GoblinExit;
        }
    }
    protected override void UnSubscribeEvents()
    {
        base.UnSubscribeEvents();
        List<GroundTile> tilesInRange = GetTilesInRange();
        for (int i = 0; i < tilesInRange.Count; i++)
        {
            tilesInRange[i].OnGoblinEntered -= GoblinEnter;
            tilesInRange[i].OnGoblinExit -= GoblinExit;
        }
    }

    protected List<GroundTile> GetTilesInRange()
    {
        GroundTile pivotTile = GridMap.Instance.GetTileByGridPos(probePivotPoint);
        List<GroundTile> tilesInRange = new List<GroundTile>();
        GroundTile[] holderArray = PathFinder.GetEightAxisNeighbors(pivotTile, GridMap.Instance.CurrentTileMap, towerData.range);
        for (int i = 0; i < holderArray.Length; i++)
        {
            tilesInRange.Add(holderArray[i]);
        }
        distanceToFurtherestRangeTile = Vector3.Distance(tilesInRange[tilesInRange.Count - 1].WorldPosition, GridMap.Instance.TilePositionToWorldPosition(probePivotPoint));
        return tilesInRange;

    }
    public override void OnEnqueuePool()
    {
        base.OnEnqueuePool();
        for (int i = 0; i < tilesinrange.Count; i++)
        {
            tilesinrange[i].tileRenderer.material.color = Color.white; //for debug
        }
        goblinsInRange = null;
        tilesinrange = null;
        UnSubscribeEvents();
    }
}
