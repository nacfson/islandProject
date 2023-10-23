using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum ESceneName
{
    START,
    LOAD,
    GAME,
}
public class SceneManagement : MonoBehaviour
{
    private static SceneManagement _instance;

    public static SceneManagement Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneManagement>();
            }

            return _instance;
        }
    }
    public event Action OnGameSceneLoaded; 

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        if (scene.name == ESceneName.GAME.ToString())
        {
            Debug.Log("OnGameSceneLoaded");
            OnGameSceneLoaded?.Invoke();   
        }
    }
    
    public void LoadScene(ESceneName sceneName,LoadSceneMode mode = LoadSceneMode.Single)
    {                                             
        SceneManager.LoadScene(sceneName.ToString(), mode);
    }
}
