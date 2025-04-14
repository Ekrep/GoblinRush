using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Data/MapProbesData")]
public class MapProbesData : SerializedScriptableObject
{
    public Dictionary<string, BaseTile> baseTileDict;
    public Dictionary<string, BoundableProbe> boundableProbes;
}
