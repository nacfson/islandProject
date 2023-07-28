using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    private static MoneyManager _instance;
    public static MoneyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance= FindObjectOfType<MoneyManager>();    
            }
            return _instance;
        }
    }
    private const int MAX_MONEY = 1000000;
    private int _money;
    public int Money => _money;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _money = 3000;
    }
    public void AddMoney(int plus)
    {
        _money += plus;
        _money = Mathf.Clamp(_money, 0, MAX_MONEY);
    }
    public void SetMoney(int money) => this._money = money; 
    public bool CanUseMoney(int prize) => _money >= prize;
    public void Generate(){}
}