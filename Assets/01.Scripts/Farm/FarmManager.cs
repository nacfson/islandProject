using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomUpdateManager;

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
    private List<Crop> _cropList = new List<Crop>();
    public CropSaveData[] CropDatas
    {
        get
        {
            CropSaveData[] cropDatas = new CropSaveData[_cropList.Count];
            for (int i = 0; i < cropDatas.Length; i++)
            {
                cropDatas[i] = _cropList[i].CropSaveData;
            }
            return cropDatas;
        }
    }

    private float _timer = 0f;
    [SerializeField] private float _targetTime = 10f;



    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        _farmParentTrm = transform.Find("PosData/FarmPos");
        _farmParentTrm.GetComponentsInChildren(_farmList);
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
        _timer += Time.deltaTime;
        if (_timer > _targetTime)
        {
            //현재는 특정시간마다 모든 작물들을 업그레이드 시켜주는데
            //나중에는 작물마다 다른시간들을 적용하면 좋을 것 같다.
            Debug.Log("UpgradeLevel");
            _cropList.ForEach(c => c.UpgradeLevel(1));
            _timer = 0f;
        }
    }
    private void OnEnable() => Add(this);
    private void OnDisable() => Remove(this);
    public void Add(IUpdatable updatable) => UpdateManager.Add(updatable);
    public void Remove(IUpdatable updatable) => UpdateManager.Remove(updatable);
    #endregion
}

