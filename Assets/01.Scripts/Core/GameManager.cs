using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour{
    private static GameManager _instnace;
    public static GameManager Instance => _instnace;

    [SerializeField] private PoolingListSO _poolingList;
    private List<Transform> _targetPositions;

    public AgentBrain PlayerBrain{
        get{
            if(_playerBrain == null){
                _playerBrain = GameObject.FindGameObjectWithTag("Player").GetComponent<AgentBrain>();
            }
            return _playerBrain;
        }
    }
    private AgentBrain _playerBrain;



    private void Awake() {
        if(_instnace == null){
            _instnace = this;
        }
        else{
            Debug.LogError($"Multiple GameManager is exist");
        }


        _targetPositions = new List<Transform>();
        GetComponentsInChildren<Transform>(_targetPositions);

        DontDestroyOnLoad(this);
    }

    private void CreatePoolManager(Transform trm){
        PoolManager.Instance = new PoolManager(trm);

        foreach(var p in _poolingList.pairs){
            PoolManager.Instance.CreatePool(p.prefab,p.count);
        }
    }

    public Vector3 RandomTargetPos(){
        int randomIdx = Random.Range(0,_targetPositions.Count);
        Debug.Log(_targetPositions[randomIdx].gameObject.name);
        return _targetPositions[randomIdx].position;
    }


}