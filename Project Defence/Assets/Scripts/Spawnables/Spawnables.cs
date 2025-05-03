using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Data/Spawnables")]
public class Spawnables : SerializedScriptableObject
{
    [System.Serializable]
    public struct SpawnablesData
    {
        public Unit prefab;//bad maybe?
        public int amount;
    }
    public List<SpawnablesData> spawnablesDatas;
}
