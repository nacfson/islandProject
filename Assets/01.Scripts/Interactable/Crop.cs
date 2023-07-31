using CustomUpdateManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropSaveData
{
    public Vector3 pos;
    public int level;
    public int uniqueID;
    public CropSaveData(Vector3 pos, int level, int uniqueID) 
    {
        this.pos = pos;
        this.level = level;
        this.uniqueID = uniqueID;
    }
}

public class Crop : PoolableMono, IInteractable
{
    [SerializeField] private CropData _cropSO;
    private CropSaveData _cropSaveData;
    public CropSaveData CropSaveData
    {
        get => _cropSaveData;
    }

    private MeshRenderer[] _meshRenderers;
    private MeshFilter[] _meshFilters;

    private int _currentLevel = 0;
    private void Awake()
    {
        _meshRenderers = GetComponentsInChildren<MeshRenderer>();
        _meshFilters = GetComponentsInChildren<MeshFilter>();

        ChangeCropState(_currentLevel);

        _cropSaveData = new CropSaveData(transform.position,_currentLevel,_cropSO.uniqueID); 
    }
    public void Interact(AgentBrain<ActionData> brain)
    {
        Debug.Log("Interact");
        if(_currentLevel == _cropSO.maxLevel)
        {
            PlayerBrain pb = (PlayerBrain)brain;
            pb.AgentAnimator.OnPickAnimationEndTrigger += DropItem;
            pb.ChangeState(StateType.Pick);
            return;
        }
        return;
    }
    public void UnInteract(AgentBrain<ActionData> brain)
    {
        PlayerBrain pb = (PlayerBrain)brain;
        pb.AgentAnimator.OnPickAnimationEndTrigger -= DropItem;
    }
    private void DropItem(AgentBrain<ActionData> brain)
    {
        brain.ChangeState(StateType.Idle);
        _currentLevel = 0;
        ChangeCropState(_currentLevel);
        InventoryManager.Instance.AddItem(_cropSO,Random.Range(1,3));
    }
    private float _timer = 0f;
    [SerializeField] private float _targetTime = 10f;
    public void UpgradeLevel(int plus)
    {
        if (_currentLevel >= _cropSO.maxLevel) return;

        _timer += Time.deltaTime;
        if (_timer > _targetTime)
        {
            Debug.Log("UpgradeLevel");
            _currentLevel += plus;
            _currentLevel = Mathf.Clamp(_currentLevel, 0, _cropSO.maxLevel);
            ChangeCropState(_currentLevel);
            _timer = 0f;
        }

    }

    private void ChangeCropState(int index)
    {
        for(int i =0; i< _meshRenderers.Length; i++)
        {
            try
            {
                _meshFilters[i].mesh = _cropSO.meshs[index];
                _meshRenderers[i].material = _cropSO.mat;
            }
            catch
            {
                _meshFilters[i].mesh = _cropSO.meshs[0];
                _meshRenderers[i].material = _cropSO.mat;
            }

        }
    }

    public override void Init()
    {
        Debug.Log("Init");
    }
}
