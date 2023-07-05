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
        private InfoUI _infoUI;
        private VisualElement _iu;

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
            
            _infoUI = new InfoUI(_iu);
        }

        public void StartTalk(TalkData talkData,string name){
            _infoUI.SetUp(talkData,name, () => { 
                _infoUI.ShowText();
            });
            _infoUI.ShowText();
        }
        
    }

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
}

