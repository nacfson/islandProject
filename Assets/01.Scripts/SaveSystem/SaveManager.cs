using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class SavedInfo
{
    public int money;
    public Vector3 playerPos;

    //Key => 아이템 고유 id, Value => 아이템 개수
    public Dictionary<int, int> _itemDictionary = new Dictionary<int, int>(); 


    public SavedInfo(int money, Vector3 playerPos, Dictionary<int, int> itemDictionary)
    {
        this.money = money;
        this.playerPos = playerPos;
        this._itemDictionary = itemDictionary;
    }
}
public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SaveManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    [ContextMenu("Save")]
    public void Save()
    {

    }
    [ContextMenu("Load")]
    public void Load()
    {

    }

    public void Generate(){}

}
