using System;
using System.Collections;
using System.Collections.Generic;
using Scriptables.MapCreation.MapData;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    #region PlayerCurrentValues
    private int playerCurrentCurrency;

    #endregion
    #region Goblin Values
    private int currentGoblinCount = 0;
    private int cheepestGoblinPrice;
    #endregion
    #region Events
    public static event Action<int> OnPlayerBoughtUnit;
    public static event Action<int> OnPlayerCurrencyChanged;
    public static event Action<bool> OnGameEnd;
    public static event Action OnGoblinDeath;
    public static event Action OnGoblinSpawned;

    #endregion
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
    private void OnEnable()
    {
        GridMap.OnGridMapInitialized += GridMap_OnGridMapInitialized;
    }
    private void GridMap_OnGridMapInitialized(Transform mapCenter, MapData mapData)
    {
        playerCurrentCurrency = mapData.levelStartMoney;
        PlayerCurrencyChanged();
    }
    private void OnDisable()
    {
        GridMap.OnGridMapInitialized -= GridMap_OnGridMapInitialized;
    }
    public void SetCheepestGoblinPrice(int price)
    {
        cheepestGoblinPrice = price;
    }
    public void GiveCoinToPlayer(int amount)
    {
        playerCurrentCurrency += amount;
        PlayerCurrencyChanged();
    }
    public bool CanBuyUnit(int unitCost)
    {
        if (playerCurrentCurrency >= unitCost)
        {
            //money changed event
            return true;
        }
        else
        {
            return false;
        }

    }
    private bool CheckLoseCondition()
    {
        if (currentGoblinCount < 1 && playerCurrentCurrency < cheepestGoblinPrice)
        {
            GameEnd(false);
            return true;
        }
        return false;

    }
    #region Event Func
    public void PlayerBoughtUnit(int unitCost)
    {
        playerCurrentCurrency = Mathf.Max(0, playerCurrentCurrency - unitCost);
        PlayerCurrencyChanged();
        if (OnPlayerBoughtUnit != null)
        {
            OnPlayerBoughtUnit(playerCurrentCurrency);
        }

    }
    public void PlayerCurrencyChanged()
    {
        if (OnPlayerCurrencyChanged != null)
        {
            OnPlayerCurrencyChanged(playerCurrentCurrency);
        }
    }
    public void GoblinSpawned()
    {
        if (OnGoblinSpawned != null)
        {
            OnGoblinSpawned();
        }
        currentGoblinCount++;

    }
    public void GoblinDeath()
    {
        if (OnGoblinDeath != null)
        {
            OnGoblinDeath();
        }
        currentGoblinCount--;
        CheckLoseCondition();
    }
    public void GameEnd(bool isWin)
    {
        if (OnGameEnd != null)
        {
            OnGameEnd(isWin);
        }

    }

    #endregion
}
