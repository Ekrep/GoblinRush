using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StatusEffectTower : Tower
{
    public override void Initialize(Vector2Int[] occupiedTilePositions, Vector2Int towerPivotPoint)
    {
        base.Initialize(occupiedTilePositions, towerPivotPoint);
    }
    protected override void OnGoblinEnter(IGoblin goblin, int goblinIndex, Vector2Int tile, Vector3 enterPos)
    {
        base.OnGoblinEnter(goblin, goblinIndex, tile, enterPos);
        goblin.ApplyDebuff(/*debuff type*/);
    }
    protected override void OnGoblinExit(IGoblin goblin, int goblinIndex, Vector2Int tile, Vector3 exitPos)
    {
        base.OnGoblinExit(goblin, goblinIndex, tile, exitPos);
        goblin.RemoveDebuff(/*debuff type*/);
    }
    public override void OnEnqueuePool()
    {
        base.OnEnqueuePool();
    }
}
