using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUpdateManager;

public class TimeManager : Singleton<TimeManager>,IUpdatable
{
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
    private void Start()
    {
        OnHourChanged?.Invoke(Hour);
    }
    private void OnEnable() => Add(this);
    private void OnDisable() => Remove(this);

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

    public void Add(IUpdatable updatable) => UpdateManager.Instance.Add(updatable);
    public void Remove(IUpdatable updatable) => UpdateManager.Instance.Remove(updatable);
    public override void Init(GameManager root)
    {
        base.Init(root);
        _savedHour = Hour;
    }
}
