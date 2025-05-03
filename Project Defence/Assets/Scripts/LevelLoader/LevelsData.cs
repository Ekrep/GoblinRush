using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Scriptables.MapCreation.MapData;

[CreateAssetMenu(menuName = "Data/LevelsData")]
public class LevelsData : SerializedScriptableObject
{
    public List<MapData> levels;
}
