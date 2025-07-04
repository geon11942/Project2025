using UnityEngine;

public enum E_Item_Use_Type//아이템의 사용 타입
{
    Basic,
    Throw,
    Eat,
    Tool,
    Special
}

public enum E_Item_State//아이템 오브젝트의 현재 상태(대기,플레이어한테 소속, 플레이어의 의해 발사. 나중에 다른 상태 추가 예정)
{
    Stay,
    PlayerMove,
    PlayerHold,
}
//아이템 기본 데이터를 저장하는 ScriptableObject.
[CreateAssetMenu(fileName = "ItemDataTable", menuName = "Scriptable Object/ItemDataTable", order = int.MaxValue)]
public class ItemDataTable : ScriptableObject
{
    public int ID;
    public string Item_Name = "No_Name";
    public string Image_Name = "Items_none";
    public E_Item_Use_Type type;
    public float Weight=0f;
    public float MovingTime = 0f;
}