using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System;

namespace UI_Toolkit
{
    public class UT_MainUI : MonoBehaviour
    {
        private UIDocument _document;

        private VisualElement _root;
        [SerializeField] private VisualTreeAsset _itemUXML;

        private InfoUI _infoUI;
        private VisualElement _iu;
        private VisualElement _inventoryUI;
        private VisualElement _fadeUI;
        private VisualElement _selectUI;
        private VisualElement _moneyUI;

        [Header("Select")]
        private Item _selectedItem;


        [Header("Shop")]
        private VisualElement _shopUI;
        private ScrollView _shopView;
        private Item _shopItem;
        private Dictionary<VisualElement, Item> _slotDictionary = new Dictionary<VisualElement, Item>();

        [Header("Money")]
        private Label _moneyLabel;

        private static UT_MainUI _instance;
        public static UT_MainUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<UT_MainUI>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this.gameObject);
        }
        private void OnEnable()
        {
            _document = GetComponent<UIDocument>();

            _root = _document.rootVisualElement;

            _iu = _root.Q<VisualElement>("InfoUI");
            _inventoryUI = _root.Q<VisualElement>("InventoryUI");
            //_fadeUI = _root.Q<VisualElement>("FadeUI");
            _shopUI = _root.Q<VisualElement>("ShopUI");
            _selectUI = _root.Q<VisualElement>("SelectUI");
            _moneyUI = _root.Q<VisualElement>("MoneyUI");

            Button buyBtn = _shopUI.Q<Button>("BuyBtn");
            Button sellBtn = _shopUI.Q<Button>("SellBtn");

            Button firstBtn = _selectUI.Q<Button>("FirstBtn");
            Button secondBtn = _selectUI.Q<Button>("SecondBtn");
            Button thirdBtn = _selectUI.Q<Button>("ThirdBtn");

            _moneyLabel = _moneyUI.Q<Label>("MoneyLabel");

            //각 아이템에 맞는 함수를 실행 시켜주어야 함
            firstBtn.RegisterCallback<ClickEvent>(e => InventoryManager.Instance.DoItemAction(_selectedItem.uniqueID));
            secondBtn.RegisterCallback<ClickEvent>(e => Debug.Log("SecondBtn"));
            thirdBtn.RegisterCallback<ClickEvent>(e => UnActiveSelectUI());

            buyBtn.RegisterCallback<ClickEvent>(e => BuyItem());
            sellBtn.RegisterCallback<ClickEvent>(e => SellItem());

            _shopView = _shopUI.Q<ScrollView>("ShopView");

            _inventoryUI.RemoveFromClassList("active");

            _infoUI = new InfoUI(_iu);

            MoneyManager.Instance.OnMoneyChanged += SetMoneyUI;
        }
        private void OnDisable()
        {
            //MoneyManager.Instance.OnMoneyChanged -= SetMoneyUI;
        }
        private void Start()
        {
            List<InventorySlot> slotList = new List<InventorySlot>();
            int count = _inventoryUI.childCount;
            for (int i = 0; i < count; i++)
            {
                VisualElement ele = _inventoryUI.ElementAt(i);

                VisualElement image = ele.Q<VisualElement>("ItemImage");
                Label cnt = ele.Q<Label>("ItemCount");

                InventorySlot slot = new InventorySlot(image, cnt);
                slot.UpdateUI();
                slotList.Add(slot);
            }

            InventoryManager.Instance.SetSlotList(ref slotList);
            OpenInv(false);

            //_fadeUI.transform.scale = Vector3.zero;
        }
        public void ActiveSelectUI(Item item)
        {
            if (item == null) return;
            _selectedItem = item;

            Vector2 mousePos = Input.mousePosition;
            Vector2 localMousePos = _root.WorldToLocal(mousePos);
            Debug.Log(localMousePos);

            var style = _selectUI.resolvedStyle;

            _selectUI.style.left = localMousePos.x;
            _selectUI.style.bottom = localMousePos.y - style.height;
            _selectUI.AddToClassList("active");
        }
        public void UnActiveSelectUI()
        {
            _selectUI.RemoveFromClassList("active");
            _selectedItem = null;
        }




        #region Inventory Logic
        public void OpenInv(bool result)
        {
            if (result)
            {
                _inventoryUI.AddToClassList("active");
                GameManager.Instance.PlayerBrain.ChangeState(StateType.UI);
            }
            else
            {
                _inventoryUI.RemoveFromClassList("active");
                _selectUI.RemoveFromClassList("active");
                GameManager.Instance.PlayerBrain.ChangeState(StateType.Idle);
            }
        }

        public bool IsInvOpen()
        {
            return _inventoryUI.ClassListContains("active");
        }
        #endregion


        public void StartTalk(TalkData talkData, string name,Action Callback = null)
        {
            _infoUI.SetUp(talkData, name, Callback);
            _infoUI.ShowText();
        }
        #region ShopUI

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
        #endregion

        public void UnShowAllUI()
        {
            _inventoryUI.RemoveFromClassList("active");
            _iu.RemoveFromClassList("active");
            //_infoUI = null;
            _shopUI.RemoveFromClassList("active");
            _selectUI.RemoveFromClassList("active");

        }
        public void SetMoneyUI(int money)
        {
            _moneyLabel.text = String.Format("{0:n0}",money);
        }
        /// <summary>
        /// 그저 싱글톤 생성을 위해서 만들어 둔 함수.
        /// </summary>
        public void Generate() { }
    }



    #region TalkClass(InfoUI)
    public class InfoUI
    {
        //������ InfoUI�� VisualElement
        private VisualElement _infoUI;
        //InfoUI�� Label(text)
        private Label _contentLabel;
        private Label _nameLabel;

        private TalkData _talkData;
        private bool _isAnimating = false;
        private Action _callback;


        private string _targetText;

        private int _returnIdx;

        public InfoUI(VisualElement infoUI)
        {
            this._infoUI = infoUI;


            _contentLabel = infoUI.Q<Label>("ContentLabel");
            _nameLabel = infoUI.Q<VisualElement>("NameUI").Q<Label>("NameLabel");

            _infoUI.RegisterCallback<PointerDownEvent>(e => ShowText());
        }

        public void SetUp(TalkData talkData, string name, Action action)
        {
            _callback = action;
            this._talkData = talkData;

            _nameLabel.text = name;
            _returnIdx = talkData.talkList.Count - 1;
        }

        //���� �ִϸ��̼��� �������̸� ��ŵ, �������� �ƴϸ� ���� ��ȭ�� �̵�
        public void ShowText()
        {
            if (_isAnimating)
            {
                UI_Toolkit.UT_MainUI.Instance.StopAllCoroutines();
                _contentLabel.text = _targetText;
                _isAnimating = false;
            }
            else
            {
                if (_talkData.CanGetTalk(_returnIdx))
                {
                    UI_Toolkit.UT_MainUI.Instance.StartCoroutine(AnimateTextCor(_talkData.GetTalk(_returnIdx), 0.5f));
                    _returnIdx--;
                }
                else
                {
                    _infoUI.RemoveFromClassList("active");
                    _returnIdx = _talkData.talkList.Count - 1;
                    GameManager.Instance.CamController.TalkMode(false);
                    _callback?.Invoke();
                }
            }
        }

        //startDelay => Transition Animation�� ���������� ��ٷ��ִ� �ð�
        //typingDelay => ���ڰ� �ԷµǴ� �ӵ�
        public IEnumerator AnimateTextCor(string text, float startDelay, float typingDelay = 0.05f)
        {
            _contentLabel.text = string.Empty;
            //_contentLabel.tooltip = text;
            _targetText = text;
            _infoUI.AddToClassList("active");
            GameManager.Instance.PlayerBrain.ChangeState(StateType.UI);
            GameManager.Instance.CamController.TalkMode(true);

            _isAnimating = true;

            yield return new WaitForSeconds(startDelay);
            int i = 0;
            while (_contentLabel.text.Length != text.Length)
            {
                if (!_isAnimating) yield break;

                float timer = 0f;

                while (timer < typingDelay)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
                _contentLabel.text = text.Substring(0, i);
                i++;
                yield return null;
            }
            _contentLabel.text = text.Substring(0, text.Length);
            _isAnimating = false;
        }
    }
    #endregion
}

