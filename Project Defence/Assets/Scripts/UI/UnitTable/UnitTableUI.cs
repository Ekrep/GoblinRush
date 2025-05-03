using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PoolSystem;
using Scriptables.UnitData.GoblinData;


public class UnitTableUI : MonoBehaviour
{
    [SerializeField] private UnitTableContent unitTableContentPrefab;
    [SerializeField] private Transform contentField;
    [SerializeField] private Spawnables spawnables;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        for (int i = 0; i < spawnables.spawnablesDatas.Count; i++)//currently no need for poolsystem
        {
            UnitTableContent obj = Instantiate(unitTableContentPrefab);
            obj.transform.SetParent(contentField);
            obj.InitializeContent(spawnables.spawnablesDatas[i].prefab.UnitData.icon, spawnables.spawnablesDatas[i].prefab.GetType().Name, spawnables.spawnablesDatas[i].prefab.UnitData.trainCost, i);
        }

    }
}
