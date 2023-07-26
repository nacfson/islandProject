using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
[System.Serializable]
public class JsonData
{
    public List<CustomKeyValue<int,int>> keyValuePairList = new List<CustomKeyValue<int, int>>();

    public void SetData(List<CustomKeyValue<int, int>> keyValuePairList)
    {
        this.keyValuePairList = keyValuePairList;
    }
}
public class JsonTest : MonoBehaviour
{
    private JsonData _jsonData;
    private string _savePath;
    private string _fileName = "/SaveFile.txt";

    private void Awake()
    {
        _jsonData = new JsonData();

        _savePath = Application.dataPath + "/SaveData.txt";
        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }
    }
    [ContextMenu("Save")]
    public void Save()
    {
        List<CustomKeyValue<int, int>> tempList = new List<CustomKeyValue<int, int>>();
        for(int i = 0; i < 3; i++)
        {
            tempList.Add(new CustomKeyValue<int,int>(i, i));
        }
        _jsonData.SetData(tempList);

        string jsonData = JsonUtility.ToJson(_jsonData, true);
        File.WriteAllText(_savePath + _fileName, jsonData);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        string json = File.ReadAllText(_savePath + _fileName);
        _jsonData = JsonUtility.FromJson<JsonData>(json);

        for(int i =0; i < _jsonData.keyValuePairList.Count; i++)
        {
            CustomKeyValue<int, int> keyValue = _jsonData.keyValuePairList[i];
            Debug.Log(string.Format("Key: {0}", keyValue.key));
            Debug.Log(string.Format("Value: {0}", keyValue.value));
        }
    }
}
