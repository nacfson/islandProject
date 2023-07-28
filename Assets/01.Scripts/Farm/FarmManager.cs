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

    //��ũ��Ʈ�� ���� �� ��ġ�� ��ũ��Ʈ�� �����ϴ��� Ȯ���� bool �� ����
    //bool�� true �����̸� �Ĺ��� ����
    public bool CanPlantCrops(Vector3 pos)
    {
        foreach(Farm farm in _farmList)
        {
            bool result = farm.CanPlanCrop(pos);
            if(result)
            {
                //�ϴ� position�� �Ѱ��� 
                //���߿� �Ĺ� �����͵� �Ѱ��༭ �Ĺ��� �˾Ƽ� �ɵ��� �ٲپ�� ��
                farm.AddCrop(pos);
            }
        }
        return true;
    }
}

