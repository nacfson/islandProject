using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
namespace UI_Toolkit
{
    public class UT_StartUI : MonoBehaviour
    {
        private UIDocument _uiDocument;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            GameManager.Instance.GameInput.OnAnyKeyPress += () => SceneManager.LoadScene("Map");

        }

        private void OnEnable()
        {
            var root = _uiDocument.rootVisualElement;
            
            
        }
        
        
    }

}
