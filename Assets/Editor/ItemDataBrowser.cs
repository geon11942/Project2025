using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;


// Assets/Resources/ItemDatas에 있는 아이템 데이터들을 데이터 안에 있는 ID의 크기순으로 보는 뷰어를 만드는 코드.
// 그 외의 다른 부분을 건들지 않고 단순히 ID를 변경했는지 확인용.
public class ItemDataBrowser : EditorWindow
{
    private List<ItemDataTable> items = new List<ItemDataTable>();

    [MenuItem("Tools/Item Data Viewer")]
    public static void ShowWindow()
    {
        GetWindow<ItemDataBrowser>("Item Data Viewer");
    }

    private void OnEnable()
    {
        LoadAndSortItems();
    }

    private void LoadAndSortItems()
    {
        items.Clear();

        string[] guids = AssetDatabase.FindAssets("t:ItemDataTable", new[] { "Assets/Resources/Datas/ItemDatas" }); // 원하는 폴더 경로로 수정

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemDataTable item = AssetDatabase.LoadAssetAtPath<ItemDataTable>(path);
            if (item != null)
            {
                items.Add(item);
            }
        }

        // 정렬: 원하는 변수 기준
        items = items.OrderBy(i => i.ID).ToList();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Reload"))
        {
            LoadAndSortItems();
        }

        GUILayout.Space(10);

        foreach (var item in items)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ID: " + item.ID.ToString(), GUILayout.Width(50));
            EditorGUILayout.LabelField(item.name, GUILayout.Width(200));
            EditorGUILayout.ObjectField(item, typeof(ItemDataTable), false);
            EditorGUILayout.EndHorizontal();
        }
    }
}