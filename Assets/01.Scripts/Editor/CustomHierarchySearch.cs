using UnityEngine;
using UnityEditor;

public class CustomHierarchySearch : EditorWindow
{
    private string searchText = "Tools";

    [MenuItem("Custom/Search Hierarchy")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CustomHierarchySearch));
    }

    private void OnGUI()
    {
        GUILayout.Label("Hierarchy Search and Filter", EditorStyles.boldLabel);

        // 검색어 입력 필드
        searchText = EditorGUILayout.TextField("Search:", searchText);

        if (GUILayout.Button("Search"))
        {
            SearchAndFilterHierarchy();
        }

        if (GUILayout.Button("Clear Filter"))
        {
            ClearHierarchyFilter();
        }
    }

    private void SearchAndFilterHierarchy()
    {
        // Hierarchy 창의 모든 게임 오브젝트 가져오기
        GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();

        // 검색어가 포함된 오브젝트를 찾아서 필터링
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name.Contains(searchText))
            {
                Selection.activeObject = obj;
                Debug.LogWarning($"This is not exist in hieracy: {obj.name}");
            }
        }
    }

    private void ClearHierarchyFilter()
    {
        // Hierarchy 창 필터 해제
        Selection.activeObject = null;
    }
}