using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PoolSystem.Poolable;
using UnityEngine;
using static StaticHelpers.Util.Utils;
public class BasicGoblin : BaseGoblinUnit
{
    protected override void OnReachedTarget()
    {
        base.OnReachedTarget();
        if (path.PathStatus == PathStatus.Reachable)
        {
            Debug.Log("I CAM SEMSE TJE SMELL OF NEXUS");
            //it means reached Nexus
            Nexus nexus = (Nexus)GridMap.Instance.GetTileByGridPos(path.PathEndGridPos).BoundableProbe;
            nexus.Damage(attackDamage);
            Death();
        }
        else
        {
            Disappear();
        }
    }
    protected override void DoSpecialThing()
    {
        //basic unit
    }
    public override void Disappear()
    {
        base.Disappear();
        GameManager.Instance.GiveCoinToPlayer(UnitData.trainCost);
    }

}
