using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;
using System.Text;

[System.Serializable]
public class SaveData
{
    public int money;
    public Vector3 playerPos;

    //Key => 아이템 고유 id, Value => 아이템 개수
    public Dictionary<int, int> itemDictionary = new Dictionary<int, int>(); 
    public void SetDatas(int money, Vector3 playerPos, Dictionary<int, int> itemDictionary)
    {
        this.money = money;
        this.playerPos = playerPos;
        this.itemDictionary = itemDictionary;
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

    private SaveData _saveData;
    private string _savePath;
    private string _fileName = "/SaveFile.txt";
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        _saveData = new SaveData();

        _savePath = Application.dataPath + "/SaveData.txt";
        if(!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {


        int money = MoneyManager.Instance.Money;
        Vector3 playerPos = GameManager.Instance.PlayerBrain.transform.position;
        Debug.Log(string.Format("PlayerPos: {0}",playerPos.ToString()));
        Dictionary<int, int> itemDictionary = new Dictionary<int, int>();
        /* 슬롯 리스트에서 아이템 아이디, 얼마나 있는지 가져와서 Dictionary에 넣어줌*/
        foreach(InventorySlot slot in InventoryManager.Instance.SlotList)
        {
            Item item = slot.GetItem();
            if (item == null) continue;
            int id = item.uniqueID;
            itemDictionary.Add(id, slot.GetAmount());
        }

        _saveData.SetDatas(money, playerPos, itemDictionary);

        string jsonData = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(_savePath + _fileName, jsonData);
    }
    [ContextMenu("Load")]
    public void Load()
    {
        //Load Debug가 찍히지 않음
        if(File.Exists(_savePath + _fileName))
        {
            Debug.Log("Load");
            string json = File.ReadAllText(_savePath + _fileName);
            _saveData = JsonUtility.FromJson<SaveData>(json);

            Vector3 pos = _saveData.playerPos;
            GameManager.Instance.PlayerBrain.AgentMovement.SetPlayerPos(pos);
            Debug.Log(_saveData.playerPos);

            MoneyManager.Instance.SetMoney(_saveData.money);

            Dictionary<Item, int> itemDictionary = new Dictionary<Item, int>();
            foreach(KeyValuePair<int,int> pair  in _saveData.itemDictionary)
            {
                Item item = InventoryManager.Instance.GetItemFromID(pair.Key);
                itemDictionary.Add(item,pair.Value);
            }
            InventoryManager.Instance.SetSlotItem(itemDictionary);
        }
    }
    public void Generate(){}
}
