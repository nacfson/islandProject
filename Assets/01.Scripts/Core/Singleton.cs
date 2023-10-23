using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : new()
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameManager.Instance.GenerateClass<T>();
            }

            return _instance;
        }
        set => _instance = value;
    }

    protected bool _isInit = false;
    protected Transform _agentTrm;
    public virtual void Init(GameManager root)
    {
        _agentTrm = root.transform;
    }
}
