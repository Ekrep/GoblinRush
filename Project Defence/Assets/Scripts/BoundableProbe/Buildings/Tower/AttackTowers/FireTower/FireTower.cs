using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : AttackTower
{
    public override void OnUnbound()
    {
        //throw new System.NotImplementedException();
    }
    protected override void OnGoblinEnter(IGoblin goblin, int goblinIndex, Vector2Int tile, Vector3 enterPos)
    {
        base.OnGoblinEnter(goblin, goblinIndex, tile, enterPos);
        if (!isAlerted)
        {
            StartCoroutine(AttackSequence());
        }

    }
    private IEnumerator AttackSequence()
    {
        isAlerted = true;
        while (currentTarget != null)
        {
            currentTarget.Damage(attackTowerData.attackDamage);
            yield return new WaitForSeconds(attackTowerData.cooldown);
        }
        isAlerted = false;
    }
    public override void OnEnqueuePool()
    {
        base.OnEnqueuePool();
        StopCoroutine(AttackSequence());
    }
}
