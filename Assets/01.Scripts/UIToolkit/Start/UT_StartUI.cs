using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
namespace UI_Toolkit
{
    public class UT_StartUI : MonoBehaviour
    {
        private UIDocument _uiDocument;
        private bool _isLoadGame = false;
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            GameManager.Instance.GameInput.OnAnyKeyPress += LoadGameScene;
        }
        private void OnEnable()
        {
            var root = _uiDocument.rootVisualElement;
        }
        private void OnDisable()
        {
            GameManager.Instance.GameInput.OnAnyKeyPress -= LoadGameScene;
        }

        public void LoadGameScene()
        {
            if (_isLoadGame) return;
            SceneManagement.Instance.LoadScene(ESceneName.GAME);
            _isLoadGame = true;
        }
    }
}
