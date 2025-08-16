using UnityEngine;


//맵 만드는 기초 데이터를 저장하는 ScriptableObject.
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
