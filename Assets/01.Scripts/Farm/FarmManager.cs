using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUpdateManager;
using System.Linq;
using System;

public class FarmManager : MonoBehaviour,IUpdatable
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

    private Transform _farmParentTrm;
    private List<Farm> _farmList = new List<Farm>();
    private HashSet<Crop> _cropHash = new HashSet<Crop>();
    public CropSaveData[] CropDatas
    {
        get
        {
            CropSaveData[] cropDatas = new CropSaveData[_cropHash.Count];
            for (int i = 0; i < cropDatas.Length; i++)
            {
                
                cropDatas[i] = _cropHash.ElementAt(i).CropSaveData;
            }
            return cropDatas;
        }
    }


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        _farmParentTrm = transform.Find("PosData/FarmPos");
        _farmParentTrm.GetComponentsInChildren(_farmList);

        foreach(Farm farm  in _farmList)
        {
            farm.SetUp();
            for(int i = 0; i < farm.Crops.Length; i++)
            {
                Crop crop = farm.Crops[i];
                if(!_cropHash.Contains(crop))
                {
                    _cropHash.Add(crop);
                }
            }
        }

        foreach(Crop crop in _cropHash)
        {
            Debug.Log(String.Format("Crop: {0}", crop));
        }
    }
    //bool이 true 상태이면 식물을 심음
    public bool CanPlantCrops(Vector3 pos,int itemID)
    {
        foreach(Farm farm in _farmList)
        {
            bool result = farm.CanPlantCrop(pos);
            if(result)
            {
                farm.AddCrop(pos,itemID);
                return true;
            }
        }
        return false;
    }
    public void AddFarm(Farm farm)
    {
        if(!_farmList.Exists(x => x == farm))
        {
            _farmList.Add(farm);
        }
    }
    #region UpdateSystem
    public void CustomUpdate()
    {
        foreach (Crop crop in _cropHash)
        {
            crop.UpgradeLevel(1);
        }
    }
    private void OnEnable() => Add(this);
    private void OnDisable() => Remove(this);
    public void Add(IUpdatable updatable) => UpdateManager.Add(updatable);
    public void Remove(IUpdatable updatable) => UpdateManager.Remove(updatable);
    #endregion
}

