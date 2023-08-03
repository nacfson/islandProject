using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUpdateManager;
using System.Linq;
using System;
using JetBrains.Annotations;

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
    private Tree[] _trees;

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

        //Farm List
        foreach(Farm farm  in _farmList)
        {
            farm.SetUp();
            for(int i = 0; i < farm.cropList.Count; i++)
            {
                Crop crop = farm.cropList[i];
                if(!_cropHash.Contains(crop))
                {
                    _cropHash.Add(crop);
                }
            }
        }
        //Crop Hash

        foreach(Crop crop in _cropHash)
        {
            Debug.Log(String.Format("Crop: {0}", crop));
        }
        //Tree Array

        Transform[] transforms = GameManager.Instance.GetPosDatas("TreePos");
        _trees = new Tree[transforms.Length];
        for(int i = 0; i < transforms.Length; i++) 
        {
            _trees[i] = transforms[i].GetComponent<Tree>();
            _trees[i].SetUp();
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
                //설치할 Crop을 반환받음
                Crop crop = farm.AddCrop(pos,itemID);
                //HashSet에 추가
                if(!_cropHash.Contains(crop)) { _cropHash.Add(crop); }
                else { Debug.LogError(string.Format("It is exist: {0}", crop)); }
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

