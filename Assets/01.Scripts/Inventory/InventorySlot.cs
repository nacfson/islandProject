using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UI_Toolkit;
using System.Text;

public class InventorySlot
{
    private Inventory _inventory;
    private VisualElement _image;
    private Label _count;

    private StringBuilder _sb;
    public InventorySlot(VisualElement image, Label count)
    {
        this._image = image;
        this._count = count;
        _image.RegisterCallback<ClickEvent>(e => UT_MainUI.Instance.ActiveSelectUI(GetItem()));
        _sb = new StringBuilder();
    }
    public void UpdateUI()
    {
        _sb.Remove(0, _sb.Length);
        if (_inventory == null)
        {
            _image.style.backgroundImage = null;
            _image.RemoveFromClassList("active");
        }
        else
        {
            _sb.Append($"x{_inventory.amount}");
            _image.style.backgroundImage = _inventory.item?.itemSprite.texture;
            _image.AddToClassList("active");
        }
        _count.text = _sb.ToString();
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
        if (_inventory == null)
        {
            _inventory = new Inventory(item, amount);
        }
        else
        {
            _inventory.item = item;
            _inventory.amount = amount;
        }
    }
    /// <summary>
    /// It returns Inventroy.Amount is zero
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool AddItem(int amount)
    {
        bool result = false;
        _inventory.AddItem(amount);
        if (_inventory.amount <= 0)
        {
            _inventory = null;
            result = true;
        }
        UpdateUI();
        return result;
    }
    public int GetAmount() => _inventory.amount;
}