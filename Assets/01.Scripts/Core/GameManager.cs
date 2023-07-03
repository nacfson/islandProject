using UnityEngine;

public class GameManager : MonoBehaviour{
    private static GameManager _instnace;
    public static GameManager Instance => _instnace;

    [SerializeField] private PoolingListSO _poolingList;

    public AgentBrain PlayerBrain;

    private void Awake() {
        if(_instnace == null){
            _instnace = this;
        }
        else{
            Debug.LogError($"Multiple GameManager is exist");
        }

        if(PlayerBrain == null) {
            PlayerBrain = GameObject.FindGameObjectWithTag("Player").GetComponent<AgentBrain>();
        }

        DontDestroyOnLoad(this);
    }

    private void CreatePoolManager(Transform trm){
        PoolManager.Instance = new PoolManager(trm);

        foreach(var p in _poolingList.pairs){
            PoolManager.Instance.CreatePool(p.prefab,p.count);
        }
    }


}