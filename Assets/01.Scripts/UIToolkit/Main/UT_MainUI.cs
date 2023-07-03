using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

namespace UI_Toolkit{
    public class UT_MainUI : MonoBehaviour {
        private UIDocument _document;

        private VisualElement _root;
        private VisualElement _infoUI;

        private void OnEnable() {
            _document = GetComponent<UIDocument>();

            _root = _document.rootVisualElement;
            _infoUI = _root.Q<VisualElement>("InfoUI");
        }


        private void Update() {
            if (Input.GetKeyDown(KeyCode.T)) {
                SetInfoUI("I Found The Rocket");
            }
        }

        public void SetInfoUI(string content) {
            Label con = _infoUI.Q<Label>("ContentLabel");
            _infoUI.AddToClassList("active");

            StartCoroutine(AnimateTextCor(content, con, 0.5f));
        }
        //startDelay => Transition Animation이 끝날때까지 기다려주는 시간
        //typingDelay => 글자가 입력되는 속도ㄴ
        private IEnumerator AnimateTextCor(string text,Label label,float startDelay,float typingDelay = 0.08f) {
            yield return new WaitForSeconds(startDelay);
            for(int i = 0; i <= text.Length; i++) {
                label.text = text.Substring(0,i);
                yield return new WaitForSeconds(typingDelay);
            }
        }
    }
}

