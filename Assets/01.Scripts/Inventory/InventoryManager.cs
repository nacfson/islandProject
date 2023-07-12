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

        //GetComponentsInChildren<InventorySlot>(_slotList);
    }
    
    //똑같은 아이템을 보유하고있는 슬롯 있으면 개수 추가,
    //아니면 아이템을 보유하고 있지 않은 슬롯에 아이템 설정
    public bool AddItem(Item item,int amount){
        if(IsFullInv()) return false;

        Debug.Log("AddItem");
        //item이 null인 slot을 한 개 미리 찾음
        InventorySlot nullSlot = null;

        foreach(var i in _slotList){
            if(i.GetItem() == null && nullSlot == null ) nullSlot = i;
            
            if(i.GetItem() == item){
                i.AddItem(amount);
                UpdateInventory();
                return true;
            }
        }
        //만약 nullSlot 이 없으면 Inventory가 꽉찬 상태임
        if(nullSlot == null) {
            Debug.LogError("InventoryIsFull");
            return false;
        }
        //nullSlot에 item을 설정해줌
        nullSlot.SetItem(item,amount);
        UpdateInventory();
        return true;
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

    public bool IsFullInv(){
        bool result = false;
        foreach(var i in _slotList){
            result = i.IsFull();
            if(!result) return result;
        }
        return result;
    }

    public void SetSlotList(ref List<InventorySlot> slotList){
        _slotList = slotList;
    }
}