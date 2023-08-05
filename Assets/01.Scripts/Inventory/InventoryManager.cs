using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UI_Toolkit;
using UnityEngine;
using UnityEngine.Video;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;
    public static InventoryManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InventoryManager>();
            }
            return _instance;
        }
    }
    private List<InventorySlot> _slotList = new List<InventorySlot>();
    public List<InventorySlot> SlotList => _slotList;

    [SerializeField] private ItemListData _itemListSO;
    private Dictionary<int, Item> _itemDictionary;
    private Dictionary<ItemType, MethodInfo> _itemActionDictionary;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        _itemDictionary = new Dictionary<int, Item>();
        _itemActionDictionary = new Dictionary<ItemType, MethodInfo>();
        _itemListSO.itemList.ForEach(i => _itemDictionary.Add(i.uniqueID, i)); //ItemDictionary에 고유 id에 맞는 아이템 추가

        foreach (ItemType itemType in Enum.GetValues((typeof(ItemType))))
        {
            try
            {
                MethodInfo mInfo = this.GetType().GetMethod(String.Format("Do{0}Item", itemType), BindingFlags.Instance | BindingFlags.NonPublic);
                _itemActionDictionary.Add(itemType, mInfo);
            }
            catch
            {
                Debug.LogError(String.Format("This is an error at {0}", itemType));
            }

        }
    }

    //똑같은 아이템을 보유하고있는 슬롯 있으면 개수 추가,
    //아니면 아이템을 보유하고 있지 않은 슬롯에 아이템 설정
    public bool AddItem(Item item, int amount)
    {
        if (IsFullInv()) return false;

        Debug.Log("AddItem");
        //item이 null인 slot을 한 개 미리 찾음
        InventorySlot nullSlot = null;

        foreach (var i in _slotList)
        {
            if (i.GetItem() == null && nullSlot == null) nullSlot = i;

            if (i.GetItem() == item)
            {
                i.AddItem(amount);
                UpdateInventory();
                return true;
            }
        }
        //만약 nullSlot 이 없으면 Inventory가 꽉찬 상태임
        if (nullSlot == null)
        {
            Debug.LogError("InventoryIsFull");
            return false;
        }
        //nullSlot에 item을 설정해줌
        nullSlot.SetItem(item, amount);
        UpdateInventory();
        return true;
    }

    public bool SubtractItem(Item item, int amount)
    {
        foreach (var i in _slotList)
        {
            if (i.GetItem() == item && i.GetAmount() >= amount)
            {
                if (i.AddItem(-amount))
                {
                    UT_MainUI.Instance.UnActiveSelectUI();
                    return true;
                }
            }
        }
        return false;
    }

    public bool CanSubtractItem(Item item,int amount)
    {
        foreach (var i in _slotList)
        {
            if (i.GetItem() == item && i.GetAmount() >= amount)
            {
                return true;
            }
        }
        return false;
    }
    private void UpdateInventory()
    {
        Debug.Log("UpdateInventory");
        foreach (var i in _slotList)
        {
            if (i.GetItem() != null)
            {
                i.UpdateUI();
                Debug.Log($"Item: {i.GetItem()} Count {i.GetAmount()}");
            }
        }
    }

    public bool IsFullInv()
    {
        bool result = false;
        foreach (var i in _slotList)
        {
            result = i.IsFull();
            if (!result) return result;
        }
        return result;
    }

    public void SetSlotList(ref List<InventorySlot> slotList) => _slotList = slotList;
    public void SetSlotItem(List<CustomKeyValue<Item, int>> itemKeyValues)
    {
        Debug.Log("SetSlotItem");
        for (int i = 0; i < itemKeyValues.Count; i++)
        {
            CustomKeyValue<Item, int> pair = itemKeyValues.ElementAt(i);
            Debug.Log(String.Format("Pair: {0}", pair));
            _slotList[i].SetItem(pair.key, pair.value);
        }
        UpdateInventory();
    }
    #region ItemLogic
    public Item GetItemFromID(int uniqueID)
    {
        Item item = _itemDictionary[uniqueID];
        if (item == null) return null;
        return item;
    }
    public void DoItemAction(int itemID)
    {
        Item item = GetItemFromID(itemID);

        if(CanSubtractItem(item,1))
        {
            ItemType itemType = item.itemType;

            MethodInfo mInfo = _itemActionDictionary[itemType];
            mInfo?.Invoke(this, new object[] { item });
        }


    }
    private void DoNormalItem(Item item) => Debug.Log("DoNormalItem");
    private void DoCropItem(Item item)
    {
        Debug.Log("DoCropItem");
        Vector3 playerPos = GameManager.Instance.PlayerBrain.transform.position;
        bool result = FarmManager.Instance.CanPlantCrops(playerPos, item.uniqueID);
        if(result)
        {
            if(SubtractItem(item, 1) == false)
            {
                Debug.LogError("This is not enough item");
            }
            UpdateInventory();
        }
        else
        {
            Debug.LogError(String.Format("Can't Plant Crops this pos! {0}",GameManager.Instance.PlayerBrain.transform.position));
        }

        Debug.Log(String.Format("Result: {0}", result));
    }
    private void DoToolItem(Item item)
    {
        PlayerBrain pb = GameManager.Instance.PlayerBrain;
        IActionable actionable = pb.FindTransform(String.Format("Tools/{0}",item.itemName)).GetComponent<IActionable>();
        pb.Actionable = actionable;
        Debug.Log(String.Format("PlayerBrain.Actionable: {0}",pb.Actionable));
    }
    #endregion

    public void Generate() { }
}