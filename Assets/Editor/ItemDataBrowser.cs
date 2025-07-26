using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;


// Assets/Resources/ItemDatas�� �ִ� ������ �����͵��� ������ �ȿ� �ִ� ID�� ũ������� ���� �� ����� �ڵ�.
// �� ���� �ٸ� �κ��� �ǵ��� �ʰ� �ܼ��� ID�� �����ߴ��� Ȯ�ο�.
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

        string[] guids = AssetDatabase.FindAssets("t:ItemDataTable", new[] { "Assets/Resources/Datas/ItemDatas" }); // ���ϴ� ���� ��η� ����

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemDataTable item = AssetDatabase.LoadAssetAtPath<ItemDataTable>(path);
            if (item != null)
            {
                items.Add(item);
            }
        }

        // ����: ���ϴ� ���� ����
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