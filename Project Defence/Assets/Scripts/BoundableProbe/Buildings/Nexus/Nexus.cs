using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : BoundableProbe, IDamageableBuilding
{
    public void Damage(int damageAmount)
    {
        Debug.Log($"OHH DAMN I GOT HUGE DAMAGE (btw my current health is->{boundableHealth})");
        boundableHealth = Mathf.Max(0, boundableHealth - damageAmount);
        if (boundableHealth == 0)
        {
            DestroyBuilding();
        }
    }

    public void DestroyBuilding()
    {
        GameManager.Instance.GameEnd(true);
    }

    public override void OnUnbound()
    {
        //throw new System.NotImplementedException();
    }
}
