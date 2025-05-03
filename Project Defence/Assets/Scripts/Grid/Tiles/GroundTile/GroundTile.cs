using System;
using UnityEngine;

[System.Serializable]
public class GroundTile : BaseTile
{
    [Header("Visual")]
    [SerializeField] private MeshRenderer[] tileVisuals;

    [Header("Classic Values")]
    public IGoblin[] assignedGoblinUnits;
    public INonGoblin[] assignedNonGoblinUnits;
    private int assignedGoblinIndex = 0;
    private int assignedNonGoblinIndex = 0;
    public event Action<IGoblin, int, Vector2Int, Vector3> OnGoblinEntered;
    public event Action<IGoblin, int, Vector2Int, Vector3> OnGoblinExit;
    [HideInInspector] public GroundTile parent;
    [HideInInspector] public int g, h;//SO BAD AQ
    [HideInInspector] public int f => g + h;
    public override void Initialize()
    {
        base.Initialize();
        assignedGoblinUnits = new IGoblin[20];//experimental value
        assignedNonGoblinUnits = new INonGoblin[20];
        InitializeVisual(gridPosition.x + gridPosition.y);
    }
    public void InitializeVisual(int creationIndex)
    {
        for (int i = 0; i < tileVisuals.Length; i++)//just in case
        {
            tileVisuals[i].enabled = false;
        }
        int modifiedValue = creationIndex % 2;
        tileVisuals[modifiedValue].enabled = true;
        tileRenderer = tileVisuals[modifiedValue];
    }
    public void AssignUnit(IAssignableUnit unit)
    {
        if (unit is IGoblin)
        {
            unit.PassAssignedIndex(assignedGoblinIndex, gridPosition);
            assignedGoblinUnits[assignedGoblinIndex] = (IGoblin)unit;
            assignedGoblinIndex = (assignedGoblinIndex + 1) % assignedGoblinUnits.Length;
            OnGoblinEntered?.Invoke((IGoblin)unit, assignedGoblinIndex, gridPosition, unit.GetWorldPosition());
        }
        else
        {
            unit.PassAssignedIndex(assignedNonGoblinIndex, gridPosition);
            assignedNonGoblinUnits[assignedNonGoblinIndex] = (INonGoblin)unit;
            assignedNonGoblinIndex = (assignedNonGoblinIndex + 1) % assignedNonGoblinUnits.Length;
        }

    }
    public void RemoveUnit(IAssignableUnit unit, int index)
    {
        if (unit is IGoblin)
        {
            assignedGoblinUnits[index] = null;
            OnGoblinExit?.Invoke((IGoblin)unit, assignedGoblinIndex, gridPosition, unit.GetWorldPosition());
        }
        else
        {
            assignedNonGoblinUnits[index] = null;
        }

    }
    public IGoblin GetGoblinByIndex(int index)
    {
        if (assignedGoblinUnits[index] != null)
        {
            return assignedGoblinUnits[index];
        }
        else
        {
            return null;
        }
    }
}
