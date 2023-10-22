using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using UI_Toolkit;

public class GameManager : MonoBehaviour
{

    
    #region Property

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    
    [SerializeField] private PoolingListSO _poolingList;
    [SerializeField] private TargetPosListData _targetPosList;
    [SerializeField] private ItemRarityData _itemRarityData;
    public ItemRarityData ItemRarityData => _itemRarityData;
    
    [SerializeField] private FishDataList _fishDataList;
    public FishDataList FishDataList => _fishDataList;

    [SerializeField] private Input_GameInput _gameInput;
    public Input_GameInput GameInput => _gameInput;
    public PlayerBrain PlayerBrain
    {
        get
        {
            if (_playerBrain == null)
            {
                _playerBrain = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBrain>();
            }
            return _playerBrain;
        }
    }
    private PlayerBrain _playerBrain;
    #endregion

    private void Awake()
    {
        Debug.Log(GameManager.Instance);
        
        CreatePoolManager(this.transform);
        TimeManager.Instance.Generate();
        MoneyManager.Instance.Generate();
        UT_MainUI.Instance.Generate();
        UIManager.Instance.Generate();
        LightManager.Instance.Generate();
        SaveManager.Instance.Generate();
        InventoryManager.Instance.Generate();
        FarmManager.Instance.Generate();
        CameraManager.Instance.Generate();
        
        DontDestroyOnLoad(this);

        _targetPosList.posDatas.ForEach(p => p.SetPosDatas());
    }
    private void CreatePoolManager(Transform trm)
    {
        PoolManager.Instance = new PoolManager(trm);

        foreach (var p in _poolingList.pairs)
        {
            PoolManager.Instance.CreatePool(p.prefab, p.count);
        }
    }
    public Transform[] GetPosDatas(string childName)
    {
        Transform trm = transform.Find(String.Format("PosData/{0}",childName));
        Transform[] trms = new Transform[trm.childCount];
        for(int i = 0; i < trms.Length; i++)
        {
            trms[i] = trm.GetChild(i).GetComponent<Transform>();
        }
        return trms;
    }
}