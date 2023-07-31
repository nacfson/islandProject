using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;
using System.Text;
[System.Serializable]
public class CustomKeyValue<TKey,TValue>
{
    public TKey key;
    public TValue value;
    public CustomKeyValue(TKey key, TValue value)
    {
        this.key = key;
        this.value = value;
    }
    public CustomKeyValue(){}
}
[System.Serializable]
public class SaveData
{
    public int money;
    public Vector3 playerPos;

    //Key => 아이템 고유 id, Value => 아이템 개수
    public List<CustomKeyValue<int, int>> itemKeyValues = new List<CustomKeyValue<int, int>>();
    public CropSaveData[] cropDatas;
    public void SetDatas(int money, Vector3 playerPos, List<CustomKeyValue<int, int>> itemKeyValues,CropSaveData[] cropDatas)
    {
        this.money = money;
        this.playerPos = playerPos;
        this.itemKeyValues = itemKeyValues;

        this.cropDatas = new CropSaveData[cropDatas.Length];
        this.cropDatas = cropDatas;
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

        var itemKeyValues = new List<CustomKeyValue<int, int>>();
        /* 슬롯 리스트에서 아이템 아이디, 얼마나 있는지 가져와서 Dictionary에 넣어줌*/
        foreach(InventorySlot slot in InventoryManager.Instance.SlotList)
        {
            Item item = slot.GetItem();
            if (item == null) continue;
            int id = item.uniqueID;
            Debug.Log(string.Format("Item ID: {0}", id));

            var keyValue = new CustomKeyValue<int, int>(id,slot.GetAmount());
            itemKeyValues.Add(keyValue);
        }

        _saveData.SetDatas(money, playerPos, itemKeyValues,FarmManager.Instance.CropDatas);

        string jsonData = JsonUtility.ToJson(_saveData, true);
        File.WriteAllText(_savePath + _fileName, jsonData);
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(_savePath + _fileName))
        {
            Debug.Log("Load");
            string json = File.ReadAllText(_savePath + _fileName);
            _saveData = JsonUtility.FromJson<SaveData>(json);

            Vector3 pos = _saveData.playerPos;
            GameManager.Instance.PlayerBrain.AgentMovement.SetPlayerPos(pos);
            Debug.Log(_saveData.playerPos);

            MoneyManager.Instance.SetMoney(_saveData.money);

            var itemIntKeyValues = new List<CustomKeyValue<Item, int>>();
            foreach (CustomKeyValue<int, int> pair in _saveData.itemKeyValues)
            {
                Item item = InventoryManager.Instance.GetItemFromID(pair.key);
                var keyValue = new CustomKeyValue<Item, int>(item,pair.value);
                Debug.Log(string.Format("KeyValue: {0}",keyValue));
                itemIntKeyValues.Add(keyValue);
            }
            InventoryManager.Instance.SetSlotItem(itemIntKeyValues);
        }
    }
    public void Generate(){}
}
