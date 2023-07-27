using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUpdateManager;

public class FarmManager : MonoBehaviour, IUpdatable
{
    private static FarmManager _instance;
    public static FarmManager Instance
    {
        get
        {
            if(_instance == null )
            {
                _instance = FindObjectOfType<FarmManager>();
            }
            return _instance;
        }
    }
    private Transform _cropParentTrm;
    private List<Crop> _cropList = new List<Crop>();
    public CropSaveData[] CropDatas
    {
        get
        {
            CropSaveData[] cropDatas = new CropSaveData[_cropList.Count];

            for(int i =0; i < cropDatas.Length; i++)
            {
                cropDatas[i] = _cropList[i].CropSaveData;
            }
            return cropDatas;
        }
    }

    private float _timer = 0f;
    private float _targetTime = 10f;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _cropParentTrm = transform.Find("Map/Crops");
        _cropParentTrm.GetComponentsInChildren<Crop>(_cropList);
    }
    private void OnEnable() => Add(this);
    private void OnDisable() => Remove(this);
    public void Generate(){}

    public void CustomUpdate()
    {
        _timer += Time.deltaTime;
        if(_timer > _targetTime)
        {
            _cropList.ForEach(c => c.UpgradeLevel(1));
            _timer = 0f;
        }
    }

    public void Add(IUpdatable updatable) => UpdateManager.Add(updatable);
    public void Remove(IUpdatable updatable) => UpdateManager.Remove(updatable);
}

