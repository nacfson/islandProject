using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUpdateManager;

public class TimeManager : MonoBehaviour,IUpdatable
{
    private static TimeManager _instance;
    public static TimeManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<TimeManager>();
            }
            return _instance;
        }
    }
    public int Hour
    {
        get
        {
            return DateTime.Now.Hour;
        }
    }
    public int Minute
    {
        get
        {
            return DateTime.Now.Minute;
        }
    }
    public event Action<int> OnHourChanged;
    private int _savedHour;
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        _savedHour = Hour;
    }
    private void Start()
    {
        OnHourChanged?.Invoke(Hour);
    }
    private void OnEnable() => Add(this);
    private void OnDisable() => Remove(this);
    public void Generate(){}

    public void CustomUpdate()
    {
        if(Time.frameCount > 60)
        {
            if(_savedHour != Hour)
            {
                _savedHour = Hour;
                OnHourChanged?.Invoke(Hour);
            }
        }
    }

    public void Add(IUpdatable updatable) => UpdateManager.Add(updatable);
    public void Remove(IUpdatable updatable) => UpdateManager.Remove(updatable);
}
