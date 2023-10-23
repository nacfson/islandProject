using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using System.Collections;
using System.Reflection;
using CustomUpdateManager;
using UI_Toolkit;

public class GameManager : MonoBehaviour
{
    #region Property

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    
    [SerializeField] private PoolingListSO _poolingList;
    [SerializeField] private TargetPosListData _targetPosList;
    [SerializeField] private ItemRarityData _itemRarityData;
    [SerializeField] private ItemListData _itemListSO;

    public ItemListData ItemListSO => _itemListSO;
    public ItemRarityData ItemRarityData => _itemRarityData;
    
    
    [SerializeField] private FishDataList _fishDataList;
    public FishDataList FishDataList => _fishDataList;

    [SerializeField] private Input_GameInput _gameInput;
    public Input_GameInput GameInput => _gameInput;

    private bool _isSetUp = false;
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
    
    public void Init(GameManager root)
    {
        CreatePoolManager(this.transform);

        SceneManagement sceneManagement = GetComponent<SceneManagement>();
        sceneManagement.OnGameSceneLoaded += () =>
        {
            UpdateManager.Instance.Init(root);
            CameraManager.Instance.Init(root);
            TimeManager.Instance.Init(root);
            UIManager.Instance.Init(root);
            LightManager.Instance.Init(root);
            FarmManager.Instance.Init(root);
            InventoryManager.Instance.Init(root);
            MoneyManager.Instance.Init(root);
        };
        
        DontDestroyOnLoad(this);
        _targetPosList.posDatas.ForEach(p => p.SetPosDatas());

        _isSetUp = true;
    }

    private void Awake()
    {
        Instance.Init(this);
    }

    private void Update()
    {
        if (!_isSetUp) return; 
        UpdateManager.Instance.CustomUpdate();
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

    public T GenerateClass<T>() where T: new()
    {
        return new T();
    }
}