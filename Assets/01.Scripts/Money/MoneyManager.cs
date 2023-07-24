using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    private static MoneyManager _instance;
    public static MoneyManager Instance => _instance;

    public const int MAX_MONEY = 1000000;

    private int _money;
    public int Money => _money;

    void Awake()
    {
        if (_instance == null) _instance = this;
        //DontDestroyOnLoad(this.gameObject);

        _money = 3000;
    }

    public void AddMoney(int plus)
    {
        _money += plus;
        _money = Mathf.Clamp(_money, 0, MAX_MONEY);
        Debug.Log($"Money: {_money}");
    }

    public bool CanUseMoney(int prize) => _money >= prize;
}