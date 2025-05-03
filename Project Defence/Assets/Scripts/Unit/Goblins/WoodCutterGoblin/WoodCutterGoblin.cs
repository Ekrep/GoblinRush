using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StaticHelpers.Util.Utils;
using StaticHelpers.PathFinder;

public class WoodCutterGoblin : BaseGoblinUnit
{
    protected override void OnReachedTarget()
    {
        base.OnReachedTarget();
        if (path.PathStatus == PathStatus.Reachable)
        {
            Nexus nexus = (Nexus)GridMap.Instance.GetTileByGridPos(path.PathEndGridPos).BoundableProbe;
            nexus.Damage(attackDamage);
            Disappear();
        }
        else
        {
            DoSpecialThing();
        }
    }
    protected override void DoSpecialThing()
    {
        Vector2Int[] neigbors = GridMap.Instance.GetFourAxisNeighbors(currentGridPosition);
        GroundTile closestBoundedWithTree = null;
        int manhattanDistanceValue = 9999;
        int currentDistance;
        GroundTile current;
        for (int i = 0; i < neigbors.Length; i++)
        {
            current = GridMap.Instance.GetTileByGridPos(neigbors[i]);
            currentDistance = GridMap.Instance.TileManhattanDistance(neigbors[i], path.PathEndGridPos);
            if (current.BoundableProbe is BoundableTree && currentDistance < manhattanDistanceValue)
            {
                manhattanDistanceValue = currentDistance;
                closestBoundedWithTree = current;
            }
        }
        if (closestBoundedWithTree != null)
        {
            Debug.Log(closestBoundedWithTree.GridPosition);
            closestBoundedWithTree.UnboundTheBoundable();
            GridMap.Instance.TileModified();
            Death();
        }
        else
        {
            Disappear();
        }
    }


}
