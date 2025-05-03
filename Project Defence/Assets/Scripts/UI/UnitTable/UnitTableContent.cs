using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UnitTableContent : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private TextMeshProUGUI unitCostText;
    private int spawnableObjectIndex;
    [SerializeField] private Button button;
    void OnEnable()
    {
        button.onClick.AddListener(ButtonClick);
    }
    void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
    public void InitializeContent(Sprite icon, string name, int cost, int spawnableObjectIndex)
    {
        this.icon.sprite = icon;
        unitNameText.text = name;
        unitCostText.text = cost.ToString();
        this.spawnableObjectIndex = spawnableObjectIndex;
    }
    private void ButtonClick()
    {
        UIManager.Instance.UnitSelectedFromUnitTable(unitNameText.text, spawnableObjectIndex);

    }
}
