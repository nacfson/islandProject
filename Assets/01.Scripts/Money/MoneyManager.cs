using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class MoneyManager : Singleton<MoneyManager>
{
    private const int MAX_MONEY = 1000000;
    private int _money;

    public event Action<int> OnMoneyChanged;
    public int Money => _money;

    public void AddMoney(int plus)
    {
        _money += plus;
        _money = Mathf.Clamp(_money, 0, MAX_MONEY);
        OnMoneyChanged?.Invoke(this._money);
    }
    public void SetMoney(int money)
    {
        this._money = money;
        OnMoneyChanged?.Invoke(this._money);
    }
    public bool CanUseMoney(int prize) => _money >= prize;
    public override void Init(GameManager root)
    {
        base.Init(root);
        _money = MAX_MONEY;
        Debug.Log(_money);
        OnMoneyChanged?.Invoke(this._money);
    }
}