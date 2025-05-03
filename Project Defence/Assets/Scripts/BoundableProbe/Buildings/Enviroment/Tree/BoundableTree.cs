using System.Collections;
using System.Collections.Generic;
using PoolSystem;
using UnityEngine;

public class BoundableTree : BoundableProbe
{
    public override void OnUnbound()
    {
        PoolManager.Instance.EnqueueItemToPool(GetType().Name, this);
        Debug.Log("I interact with woodcutter");
    }

}
