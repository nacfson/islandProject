using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUpdateManager;

public class FarmManager : MonoBehaviour
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
    public void Generate() { }

    private Transform _cropParentTrm;
    private List<Farm> _farmList = new List<Farm>();
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
        _cropParentTrm = transform.Find("FarmPos");
        _cropParentTrm.GetComponentsInChildren<Farm>(_farmList);
    }

    //스크립트를 만들어서 그 위치에 스크립트가 존재하는지 확인후 bool 값 리턴
    //bool이 true 상태이면 식물을 심음
    public bool CanPlantCrops(Vector3 pos)
    {
        foreach(Farm farm in _farmList)
        {
            bool result = farm.CanPlanCrop(pos);
            if(result)
            {
                //일단 position만 넘겨줌 
                //나중에 식물 데이터도 넘겨줘서 식물을 알아서 심도록 바꾸어야 함
                farm.AddCrop(pos);
            }
        }
        return true;
    }
}

