using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System;

namespace UI_Toolkit{
    public class UT_MainUI : MonoBehaviour {
        private UIDocument _document;

        private VisualElement _root;
        [SerializeField] private VisualTreeAsset _itemUXML;

        private InfoUI _infoUI;
        private VisualElement _iu;
        private VisualElement _inventoryUI;
        private VisualElement _fadeUI;

        [Header("Shop")]
        private VisualElement _shopUI;
        private ScrollView _shopView;
        private Item _selectedItem;
        private Dictionary<VisualElement,Item> _slotDictionary = new Dictionary<VisualElement, Item>();

        public static UT_MainUI Instance;

        private void Awake() {
            if(Instance == null) {
                Instance = this;
            }
        }
        private void OnEnable() {
            _document = GetComponent<UIDocument>();

            _root = _document.rootVisualElement;

            _iu = _root.Q<VisualElement>("InfoUI");
            _inventoryUI = _root.Q<VisualElement>("InventoryUI");
            //_fadeUI = _root.Q<VisualElement>("FadeUI");
            _shopUI = _root.Q<VisualElement>("ShopUI");

            Button buyBtn = _shopUI.Q<Button>("BuyBtn");
            Button sellBtn = _shopUI.Q<Button>("SellBtn");

            buyBtn.RegisterCallback<ClickEvent>(e => {
                BuyItem();
            });

            sellBtn.RegisterCallback<ClickEvent>(e => {
                SellItem();
            });
            _shopView = _shopUI.Q<ScrollView>("ShopView");

            _inventoryUI.RemoveFromClassList("active");

            _infoUI = new InfoUI(_iu);
        }

        private void Start() {
            List<InventorySlot> slotList = new List<InventorySlot>();
            int count = _inventoryUI.childCount;
            for(int i = 0; i < count ; i++){
                VisualElement ele = _inventoryUI.ElementAt(i);

                VisualElement image = ele.Q<VisualElement>("ItemImage");
                Label cnt = ele.Q<Label>("ItemCount");
                
                InventorySlot slot = new InventorySlot(image,cnt);
                slot.UpdateUI();
                slotList.Add(slot);
            }

            InventoryManager.Instance.SetSlotList(ref slotList);
            OpenInv(false);

            //_fadeUI.transform.scale = Vector3.zero;
        }

        #region Inventory Logic
        public void OpenInv(bool result){
            if(result){
                _inventoryUI.AddToClassList("active");
                GameManager.Instance.PlayerBrain.ChangeState(StateType.UI);
            }
            else{
                _inventoryUI.RemoveFromClassList("active");
                GameManager.Instance.PlayerBrain.ChangeState(StateType.Idle);
            }
        }



        public bool IsInvOpen(){
            return _inventoryUI.ClassListContains("active");
        }
        #endregion
        
        
        public void StartTalk(TalkData talkData,string name){
            _infoUI.SetUp(talkData,name, () => { 
                _infoUI.ShowText();
            });
            _infoUI.ShowText();
        }
        #region ShopUI

        public void ShowShopUI(){
            _shopUI.AddToClassList("active");

            CreateItemUI();
        }

        private void CreateItemUI(){
            _shopView.Clear();
            _slotDictionary.Clear();
            List<InventorySlot> slotList = InventoryManager.Instance.SlotList;


            foreach(InventorySlot slot in slotList){
                Item item = slot.GetItem();
                if(item == null) continue;

                VisualElement itemUXML = _itemUXML.Instantiate();

                VisualElement itemImage = itemUXML.Q<VisualElement>("ItemImage");
                Label nameLabel = itemUXML.Q<Label>("NameLabel");
                
                Label priceLabel = itemUXML.Q<Label>("PriceLabel");

                _slotDictionary.Add(itemUXML,item);
                itemUXML.RegisterCallback<ClickEvent>(e => {
                    _selectedItem = _slotDictionary[itemUXML];
                    itemUXML.AddToClassList("select");
                    //Debug.LogError("SelectedItem");
                });

                Action<Sprite,string,int> SetUI = (image,name,price) => {
                    itemImage.style.backgroundImage = image.texture;
                    nameLabel.text = name;
                    priceLabel.text = $"{price}BP";
                };

                SetUI(item.itemSprite,item.itemName,item.price);

                _shopView.Add(itemUXML);
            }
        }

        public bool BuyItem(){
            if(_selectedItem == null) return false; 

            bool enoughMoney = MoneyManager.Instance.CanUseMoney(_selectedItem.price);
            if(enoughMoney){
                InventoryManager.Instance.AddItem(_selectedItem,1); // 아이템 추가
                MoneyManager.Instance.AddMoney(-_selectedItem.price);  //돈 사용
                CreateItemUI();
                return true;
            }
            return false;
        }

        public bool SellItem(){
            if(_selectedItem == null) return false;

            bool enoughItem = InventoryManager.Instance.SubtractItem(_selectedItem,1); //아이템 감소
            if(enoughItem){
                MoneyManager.Instance.AddMoney(_selectedItem.sellPrice); // 돈 추가
                CreateItemUI();
                return true;
            }
            return false;
        }
        #endregion

        public void UnShowAllUI(){
            _inventoryUI.RemoveFromClassList("active");
            _iu.RemoveFromClassList("active");
            _shopUI.RemoveFromClassList("active");
        }
    }   


#region TalkClass(InfoUI)
    public class InfoUI {
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

        public InfoUI(VisualElement infoUI) {
            this._infoUI = infoUI;
            

            _contentLabel = infoUI.Q<Label>("ContentLabel");
            _nameLabel = infoUI.Q<VisualElement>("NameUI").Q<Label>("NameLabel");

            RegisterAction();
        }

        public void SetUp(TalkData talkData,string name, Action action) {
            _callback = action;
            this._talkData = talkData;

            _nameLabel.text = name;
            _returnIdx = talkData.talkList.Count - 1;
        }

        //���� �ִϸ��̼��� �������̸� ��ŵ, �������� �ƴϸ� ���� ��ȭ�� �̵�
        public void ShowText() {
            //Debug.LogError("ShowText");
            if (_isAnimating) {
                //_contentLabel.text = _contentLabel.tooltip;
                UI_Toolkit.UT_MainUI.Instance.StopAllCoroutines();
                _contentLabel.text = _targetText;
                _isAnimating = false;
            }
            else {
                if (_talkData.CanGetTalk(_returnIdx)) {
                    UI_Toolkit.UT_MainUI.Instance.StartCoroutine(AnimateTextCor(_talkData.GetTalk(_returnIdx),0.5f));
                    _returnIdx--;
                }
                else {
                    _infoUI.RemoveFromClassList("active");
                    GameManager.Instance.PlayerBrain.ChangeState(StateType.Idle);
                    _returnIdx = _talkData.talkList.Count - 1;
                    GameManager.Instance.CamController.TalkMode(false);
                }
            }
        }

        //startDelay => Transition Animation�� ���������� ��ٷ��ִ� �ð�
        //typingDelay => ���ڰ� �ԷµǴ� �ӵ�
        public IEnumerator AnimateTextCor(string text, float startDelay, float typingDelay = 0.05f) {
            _contentLabel.text = string.Empty;
            //_contentLabel.tooltip = text;
            _targetText = text;
            _infoUI.AddToClassList("active");
            GameManager.Instance.PlayerBrain.ChangeState(StateType.UI);
            GameManager.Instance.CamController.TalkMode(true);

            _isAnimating = true;

            yield return new WaitForSeconds(startDelay);
            int i = 0;
            while (_contentLabel.text.Length != text.Length) {
                if (!_isAnimating) yield break;

                float timer = 0f;

                while (timer < typingDelay) {
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

        //���ʿ� �׼� ��ü�� VisualElement Ŭ������ �Ѱ���
        public void RegisterAction() {
            //Callback = action;
            _infoUI.RegisterCallback<PointerDownEvent>(e => _callback?.Invoke());
        }
    }
    #endregion
}

