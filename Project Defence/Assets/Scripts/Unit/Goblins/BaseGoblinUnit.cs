using System.Collections;
using System.Collections.Generic;
using PoolSystem.Poolable;
using Scriptables.UnitData.GoblinData;
using UnityEngine;

public abstract class BaseGoblinUnit : Unit, IGoblin
{
    public GridPath path;
    public GoblinData GoblinData => unitData as GoblinData;
    public override void Initialize()
    {
        base.Initialize();
        currentGridPosition = path.Path[0];
        transform.position = new Vector3(GridMap.Instance.TilePositionToWorldPosition(path.Path[0]).x, 1f, GridMap.Instance.TilePositionToWorldPosition(path.Path[0]).z);
        GroundTile currentTile = GridMap.Instance.GetTileByGridPos(currentGridPosition);
        currentTile.AssignUnit(this);
        StartCoroutine(WalkPathCoroutine(path.Path));
    }
    public void SetPath(GridPath path)
    {
        this.path = path;
    }
    protected abstract void DoSpecialThing();
    public override void Death()
    {
        base.Death();
        path = null;
        GameManager.Instance.GiveCoinToPlayer(UnitData.deathIncome);
    }
    public override void OnEnqueuePool()
    {
        base.OnEnqueuePool();
        GameManager.Instance.GoblinDeath();
    }
    public virtual void ApplyDebuff()
    {
        Debug.Log("debuffed");
    }

    public virtual void RemoveDebuff()
    {
        Debug.Log("Cleansed");
    }
}
