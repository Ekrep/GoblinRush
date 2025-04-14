using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Data/Boundables/Boundable")]
public class BoundableData : SerializedScriptableObject
{
    public int size;
    public float height;
    public bool canBlockTile = true;
}
