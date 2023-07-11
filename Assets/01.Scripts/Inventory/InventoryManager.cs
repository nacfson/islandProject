using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour{
    private static InventoryManager _instance;
    public static InventoryManager Instance{
        get{
            if(_instance == null){
                Debug.LogError("InventroyManager is null!");
            }
            return _instance;
        }
    }
    private List<InventorySlot> _slotList = new List<InventorySlot>();


    private void Awake() {
        if(_instance == null){
            _instance = this;
        }
        DontDestroyOnLoad(this.gameObject);

        GetComponentsInChildren<InventorySlot>(_slotList);
    }


    public void AddItem(Item item,int amount){
        Debug.Log("AddItem");
        foreach(var i in _slotList){
            if(i.GetItem() == item){
                i.AddItem(amount);
                UpdateInventory();
                return;
            }
        }

        foreach(var i in _slotList){
            if(i.GetItem() == null){
                i.SetItem(item,amount);
                UpdateInventory();
                return;
            }
        }
    }

    private void UpdateInventory(){
        Debug.Log("UpdateInventory");
        foreach(var i in _slotList){
            if(i.GetItem() != null){
                i.UpdateUI();
                Debug.Log($"Item: {i.GetItem()} Count {i.GetAmount()}");
            }
        }
    }

    public void SetSlotList(ref List<InventorySlot> slotList){
        Debug.Log("SetSlotList");
        _slotList = slotList;
    }
}