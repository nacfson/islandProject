using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot : MonoBehaviour{
    private Inventory _inventory;
    private VisualElement _image;
    private Label _count;

    public InventorySlot(VisualElement image, Label count){
        this._image = image;
        this._count = count;
    }

    public void UpdateUI(){
        if(_inventory == null ) return;
        string result = $"x{_inventory.amount}";
        _image.style.backgroundImage = _inventory?.item?.itemSprite?.texture;
        _count.text = result;
    }   

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