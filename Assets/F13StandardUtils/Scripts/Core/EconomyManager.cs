using System;
using F13StandardUtils.Scripts.Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class EconomyManager : Singleton<EconomyManager>
{
    [SerializeField,OnValueChanged(nameof(UpdateMoney))]
    int _moneyCount;


    public int MoneyCount => _moneyCount;
    
    
    private void Awake()
    {
        SetMoneyCount(GameController.Instance.PlayerData.Money,false);
    }

    public void SetMoneyCount(int money,bool updatePlayerPref=true)
    {
        _moneyCount = money;
        UpdateMoneyCount(updatePlayerPref);
    }

    public void IncrementMoney(int increment,bool updatePlayerPref=true)
    {
        _moneyCount += increment;
        UpdateMoneyCount(updatePlayerPref);
    }

    private void UpdateMoney() => UpdateMoneyCount(true);
    private void UpdateMoneyCount(bool updatePlayerPref)
    {
        if(updatePlayerPref)GameController.Instance.PlayerData.Money = _moneyCount;
    }
    
    
}
