using System.Collections.Generic;
using System.Linq;
using Scriptables.Towers.AttackTower;
using UnityEngine;

public abstract class AttackTower : Tower
{
    protected IGoblin currentTarget;
    protected AttackTowerData attackTowerData;
    protected bool isAlerted;
    public override void Initialize(Vector2Int[] occupiedTilePositions, Vector2Int towerPivotPoint)
    {
        base.Initialize(occupiedTilePositions, towerPivotPoint);
        attackTowerData = (AttackTowerData)boundableData;
    }
    protected override void OnGoblinEnter(IGoblin goblin, int goblinIndex, Vector2Int tile, Vector3 enterPos)
    {
        base.OnGoblinEnter(goblin, goblinIndex, tile, enterPos);
        if (currentTarget == null)
        {
            currentTarget = goblin;
        }
    }
    protected override void OnGoblinExit(IGoblin goblin, int goblinIndex, Vector2Int tile, Vector3 exitPos)
    {
        base.OnGoblinExit(goblin, goblinIndex, tile, exitPos);
        if (currentTarget.GetInstanceId() == goblin.GetInstanceId())
        {
            currentTarget = GetAvailableTarget();
        }

    }
    private IGoblin GetAvailableTarget()
    {
        if (goblinsInRange.Count < 1)
            return null;
        else
            return goblinsInRange.ElementAt(0);

    }
    public override void OnEnqueuePool()
    {
        base.OnEnqueuePool();
        isAlerted = false;
        currentTarget = null;
    }
}
