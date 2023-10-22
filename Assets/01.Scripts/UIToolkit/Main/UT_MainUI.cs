using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using System;
using System.Linq;

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
        private UT_Shop _utShop;

        public UT_Shop UT_Shop => _utShop;
        

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
            
            
            //UT_Shop Creation
            VisualElement shopUI = _root.Q<VisualElement>("ShopUI");
            ScrollView shopView = shopUI.Q<ScrollView>("ShopView");
            _utShop = new UT_Shop(shopUI,shopView,_itemUXML);

            
            _selectUI = _root.Q<VisualElement>("SelectUI");
            _moneyUI = _root.Q<VisualElement>("MoneyUI");



            Label firstLabel = _selectUI.Q<Label>("FirstLabel");
            Label secondLabel = _selectUI.Q<Label>("SecondLabel");
            Label thirdLabel = _selectUI.Q<Label>("ThirdLabel");

            _moneyLabel = _moneyUI.Q<Label>("MoneyLabel");

            //각 아이템에 맞는 함수를 실행 시켜주어야 함
            firstLabel.RegisterCallback<ClickEvent>(e => InventoryManager.Instance.DoItemAction(_selectedItem.uniqueID));
            secondLabel.RegisterCallback<ClickEvent>(e =>
            {
                Item item = InventoryManager.Instance.GetItemFromID(_selectedItem.uniqueID);
                InventoryManager.Instance.SubtractItem(item, 1);
            });
            thirdLabel.RegisterCallback<ClickEvent>(e => UnActiveSelectUI());



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
                CameraManager.Instance.TalkMode(false);
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



        public void UnShowAllUI()
        {
            _inventoryUI.RemoveFromClassList("active");
            _iu.RemoveFromClassList("active");
            //_infoUI = null;
            _utShop.RegisterClass("active",false);
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
                    CameraManager.Instance.TalkMode(false);
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
            CameraManager.Instance.TalkMode(true);

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

