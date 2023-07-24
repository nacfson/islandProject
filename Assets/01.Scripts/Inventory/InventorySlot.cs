using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventorySlot
{
    private Inventory _inventory;
    private VisualElement _image;
    private Label _count;
    public InventorySlot(VisualElement image, Label count)
    {
        this._image = image;
        this._count = count;
    }
    public void UpdateUI()
    {
        if (_inventory == null)
        {
            _count.text = "x0";
            _image.style.backgroundImage = null;
            return;
        }
        string result = $"x{_inventory.amount}";
        _image.style.backgroundImage = _inventory.item?.itemSprite.texture;
        _count.text = result;
    }
    public bool IsFull()
    {
        //Null이면 꽉 차지 않은 상태
        if (_inventory == null) return false;
        //지금 용량이 item의 최대 칸을 넘었는지
        return _inventory.amount >= _inventory.item.maxCnt;
    }

    public Item GetItem()
    {
        if (_inventory == null) return null;
        return _inventory.item;
    }
    public void SetItem(Item item, int amount)
    {
        _inventory = new Inventory(item, amount);
    }
    public void AddItem(int amount)
    {
        _inventory.AddItem(amount);
        if (_inventory.amount <= 0)
        {
            _inventory = null;
        }
    }
    public int GetAmount()
    {
        return _inventory.amount;
    }
}