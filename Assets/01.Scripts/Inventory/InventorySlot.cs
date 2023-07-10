using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour{
    private Inventory _inventory;

    public void SetItem(Item item, int amount){
        _inventory = new Inventory(item,amount);
    }

    public Item GetItem(){
        if(_inventory == null) return null;
        return _inventory.item;
    }

    public void AddItem(int amount){
        _inventory.AddItem(amount);
    }
    public int GetAmount(){
        return _inventory.amount;
    }
}