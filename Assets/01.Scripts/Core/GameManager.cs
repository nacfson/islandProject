using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameManager : MonoBehaviour{
    private static GameManager _instnace;
    public static GameManager Instance => _instnace;

    [SerializeField] private PoolingListSO _poolingList;
    private List<Transform> _targetPositions;

    private CameraController _camController;
    public CameraController CamController => _camController;

    public AgentBrain<ActionData> PlayerBrain{
        get{
            if(_playerBrain == null){
                _playerBrain = GameObject.FindGameObjectWithTag("Player").GetComponent<AgentBrain<ActionData>>();
            }
            return _playerBrain;
        }
    }
    private AgentBrain<ActionData> _playerBrain;



    private void Awake() {
        if(_instnace == null){
            _instnace = this;
        }
        else{
            Debug.LogError($"Multiple GameManager is exist");
        }


        _targetPositions = new List<Transform>();
        GetComponentsInChildren<Transform>(_targetPositions);

        CreatePoolManager(this.transform);
        CreateCameraController();
        DontDestroyOnLoad(this);
    }

    private void CreateCameraController(){
        _camController = new CameraController(this.transform);
    }

    private void CreatePoolManager(Transform trm){
        PoolManager.Instance = new PoolManager(trm);

        foreach(var p in _poolingList.pairs){
            PoolManager.Instance.CreatePool(p.prefab,p.count);
        }
    }

    public Vector3 RandomTargetPos(){
        int randomIdx = Random.Range(0,_targetPositions.Count);
        return _targetPositions[randomIdx].position;
    }


}