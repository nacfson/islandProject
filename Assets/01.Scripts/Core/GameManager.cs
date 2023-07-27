using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using UI_Toolkit;

public class GameManager : MonoBehaviour
{
    private static GameManager _instnace;
    public static GameManager Instance => _instnace;

    [SerializeField] private PoolingListSO _poolingList;
    [SerializeField] private TargetPosListData _targetPosList;
    private CameraController _camController;
    public CameraController CamController => _camController;

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



    private void Awake()
    {
        if (_instnace == null)
        {
            _instnace = this;
        }
        else
        {
            Debug.LogError($"Multiple GameManager is exist");
        }



        CreatePoolManager(this.transform);
        CreateCameraController();
        UT_MainUI.Instance.Generate();
        SaveManager.Instance.Generate();
        InventoryManager.Instance.Generate();
        FarmManager.Instance.Generate();

        DontDestroyOnLoad(this);

        _targetPosList.posDatas.ForEach(p => p.SetPosDatas());
    }

    public void OnDestroy()
    {
        Debug.Log("Destroy GameManager");
    }
    private void CreateCameraController()
    {
        _camController = new CameraController(this.transform);
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
        return trm.GetComponentsInChildren<Transform>();
    }
}