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

public class Crop : MonoBehaviour, IInteractable
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
            DropItem();
            return;
        }
        return;
    }
    public void UnInteract(AgentBrain<ActionData> brain)
    {

    }
    public void DropItem()
    {
        _currentLevel = 0;
        ChangeCropState(_currentLevel);
        InventoryManager.Instance.AddItem(_cropSO,Random.Range(1,3));
    }
    public void UpgradeLevel(int plus)
    {
        _currentLevel += plus;
        _currentLevel = Mathf.Clamp(_currentLevel, 0, _cropSO.maxLevel);
        ChangeCropState(_currentLevel);
    }

    private void ChangeCropState(int index)
    {
        for(int i =0; i< _meshRenderers.Length; i++)
        {
            //ÀÎµ¦½º ¿À·ù try catch plz
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
}
