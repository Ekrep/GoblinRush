using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[System.Serializable]
public struct GridCell
{
    private Vector2Int gridPos;
    public Vector2Int GridPos => gridPos;
    private Vector3 worldPos;
    public Vector3 WorldPos => worldPos;

    public GridCell(Vector2Int gridPos, Vector3 worldPos)
    {
        this.gridPos = gridPos;
        this.worldPos = worldPos;
    }
    //public bool isBlocked; no need for cell 
}
