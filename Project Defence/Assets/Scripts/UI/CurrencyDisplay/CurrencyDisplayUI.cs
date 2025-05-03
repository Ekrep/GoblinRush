using UnityEngine;
using TMPro;

public class CurrencyDisplayUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyDisplayText;
    private void OnEnable()
    {
        GameManager.OnPlayerCurrencyChanged += GameManager_OnCurrencyChanged;
    }
    private void GameManager_OnCurrencyChanged(int playerCurrentCurrency)
    {
        currencyDisplayText.text = playerCurrentCurrency.ToString();
    }
    private void OnDisable()
    {
        GameManager.OnPlayerCurrencyChanged -= GameManager_OnCurrencyChanged;
    }
}
