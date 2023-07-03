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

        public static UT_MainUI Instance;

        [SerializeField] private TalkData _talkData;
        private void Awake() {
            if(Instance == null) {
                Instance = this;
                Debug.LogError(Instance);
            }
        }
        private void OnEnable() {
            _document = GetComponent<UIDocument>();

            _root = _document.rootVisualElement;

            VisualElement iu = _root.Q<VisualElement>("InfoUI");

            _infoUI = new InfoUI(iu,_talkData,() => Debug.Log("OnInfoUIClicked"));
        }


        private void Update() {
            if (Input.GetKeyDown(KeyCode.T)) {
                _infoUI.ShowText();
            }
        }
    }


    public class InfoUI {
        //실질적 InfoUI의 VisualElement
        private VisualElement _infoUI;
        //InfoUI의 Label(text)
        private Label _contentLabel;
        private TalkData _talkData;
        private bool _isAnimating = false;
        private int _returnIdx;

        public InfoUI(VisualElement infoUI,TalkData talkData, Action action) {
            this._infoUI = infoUI;
            this._talkData = talkData;
            _contentLabel = infoUI.Q<Label>("ContentLabel");
            _returnIdx = talkData.talkList.Count - 1;

            RegisterAction(action);
        }
        public void RegisterAction(Action action) {
            _infoUI.RegisterCallback<PointerDownEvent>(e => action());
        }
        public void ShowText() {
            if (_isAnimating) {
                _contentLabel.text = _contentLabel.tooltip;
                _isAnimating = false;
            }
            else {
                if (_talkData.CanGetTalk(_returnIdx)) {
                    Debug.LogError("CanTalk");
                    UI_Toolkit.UT_MainUI.Instance.StartCoroutine(AnimateTextCor(_talkData.GetTalk(_returnIdx),0.5f));
                    _returnIdx--;
                }
            }
        }

        //startDelay => Transition Animation이 끝날때까지 기다려주는 시간
        //typingDelay => 글자가 입력되는 속도
        public IEnumerator AnimateTextCor(string text, float startDelay, float typingDelay = 0.1f) {
            _contentLabel.text = string.Empty;
            _contentLabel.tooltip = text;
            _infoUI.AddToClassList("active");

            _isAnimating = true;

            yield return new WaitForSeconds(startDelay);
            int i = 0;
            while (_contentLabel.text.Length != text.Length) {
                if (!_isAnimating) yield break;

                float timer = 0f;

                while (timer < typingDelay) {
                    //여기서도 yield break;를 할진느 고민해야됨
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
}

