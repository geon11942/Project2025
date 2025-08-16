using UnityEngine;


//�� ����� ���� �����͸� �����ϴ� ScriptableObject.
[CreateAssetMenu(fileName = "MapDataTable", menuName = "Scriptable Object/MapDataTable", order = int.MaxValue)]
public class MapDataTable : ScriptableObject
{
    public string MapName;
    public Vector2 MapSize;
    [Space]
    public string BaseTileName;

    [SerializeField]
    public int[][] Tiles;
}
