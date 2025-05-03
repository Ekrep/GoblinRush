using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance => instance;

    public static event Action<string, int> OnUnitSelectedFromUnitTable;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void UnitSelectedFromUnitTable(string unitName, int spawnablesIndex)
    {
        if (OnUnitSelectedFromUnitTable != null)
        {
            OnUnitSelectedFromUnitTable(unitName, spawnablesIndex);
        }

    }
}
