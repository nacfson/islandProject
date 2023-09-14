using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UI_Toolkit
{
    public class UT_Shop
    {
        [Header("Shop")]
        private VisualElement _shopUI;
        private ScrollView _shopView;
        private VisualTreeAsset _itemUXML;
        
        private Item _shopItem;
        private Dictionary<VisualElement, Item> _slotDictionary = new Dictionary<VisualElement, Item>();
        public UT_Shop(VisualElement shopUI, ScrollView shopView,VisualTreeAsset itemUXML)
        {
            this._shopUI = shopUI;
            this._itemUXML = itemUXML;

            this._shopView = _shopUI.Q<ScrollView>("ShopView");
            
            Button buyBtn = _shopUI.Q<Button>("BuyBtn");
            Button sellBtn = _shopUI.Q<Button>("SellBtn");
            
            buyBtn.RegisterCallback<ClickEvent>(e => BuyItem());
            sellBtn.RegisterCallback<ClickEvent>(e => SellItem());

        } 
        
        public void ShowShopUI(HashSet<Item> itemList)
        {
            _shopUI.AddToClassList("active");
            CreateItemUI(itemList);
        }

        private void CreateItemUI(HashSet<Item> itemList)
        {
            _shopView.Clear();
            _slotDictionary.Clear();
            InventoryManager.Instance.SlotList.ForEach(s => itemList.Add(s.GetItem()));

            foreach (Item item in itemList)
            {
                if (item == null) continue;

                VisualElement itemUXML = _itemUXML.Instantiate();

                VisualElement itemImage = itemUXML.Q<VisualElement>("ItemImage");
                Label nameLabel = itemUXML.Q<Label>("NameLabel");

                Label priceLabel = itemUXML.Q<Label>("PriceLabel");

                _slotDictionary.Add(itemUXML, item);
                itemUXML.RegisterCallback<ClickEvent>(e =>
                {
                    _shopItem = _slotDictionary[itemUXML];
                    foreach (var kvp in _slotDictionary)
                    {
                        kvp.Key.RemoveFromClassList("select");
                    }
                    itemUXML.AddToClassList("select");
                    //Debug.LogError("SelectedItem");
                });

                Action<Sprite, string, int> SetUI = (image, name, price) =>
                {
                    itemImage.style.backgroundImage = image.texture;
                    nameLabel.text = name;
                    priceLabel.text = $"{price}BP";
                };

                SetUI(item.itemSprite, item.itemName, item.price);

                _shopView.Add(itemUXML);
            }
        }


        public bool BuyItem()
        {
            if (_shopItem == null) return false;

            bool enoughMoney = MoneyManager.Instance.CanUseMoney(_shopItem.price);
            if (enoughMoney)
            {
                InventoryManager.Instance.AddItem(_shopItem, 1); // 아이템 추가
                MoneyManager.Instance.AddMoney(-_shopItem.price);  //돈 사용
                //CreateItemUI(); 아이템을 샀을 때 인벤토리 리스트랑 상점 리스트랑 같이 업데이트 해주어야 되는데 지금은 생략 ( 어차피 돈 없으면 못 사고 개수 부족하면 판매 안됨 )
                return true;
            }
            return false;
        }

        public bool SellItem()
        {
            if (_shopItem == null) return false;

            bool enoughItem = InventoryManager.Instance.SubtractItem(_shopItem, 1); //아이템 감소
            if (enoughItem)
            {
                MoneyManager.Instance.AddMoney(_shopItem.sellPrice); // 돈 추가
                //CreateItemUI();
                return true;
            }
            return false;
        }

        public void RegisterClass(string cssName, bool add = true)
        {
            if (add)
            {
                _shopUI.AddToClassList(cssName);
            }
            else
            {
                _shopUI.RemoveFromClassList(cssName);
            }
        }
    }
}
