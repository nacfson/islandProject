using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUpdateManager;
using System.Linq;
using System;
using JetBrains.Annotations;

public class FarmManager : Singleton<FarmManager>,IUpdatable
{
    public override void Init(GameManager root)
    {
        base.Init(root);
        _farmParentTrm = _agentTrm.Find("PosData/FarmPos");
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
            Debug.Log(_trees[i].gameObject.name);
        }

        _isInit = true;
    }


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
    
    //bool?? true ??????? ????? ????
    public bool CanPlantCrops(Vector3 pos,int itemID)
    {
        foreach(Farm farm in _farmList)
        {
            bool result = farm.CanPlantCrop(pos);
            if(result)
            {
                //????? Crop?? ???????
                Crop crop = farm.AddCrop(pos,itemID);
                //HashSet?? ???
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
        if (!_isInit) return;
        foreach (IGrowable crop in _cropHash)
        {
            crop.UpgradeLevel(1);
        }
        foreach(IGrowable tree in _trees)
        {
            tree.UpgradeLevel(1);
        }
    }
    
    private void OnEnable() => Add(this);
    private void OnDisable() => Remove(this);
    public void Add(IUpdatable updatable) => UpdateManager.Instance.Add(updatable);
    public void Remove(IUpdatable updatable) => UpdateManager.Instance.Remove(updatable);
    #endregion
}

