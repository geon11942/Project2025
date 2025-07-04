using UnityEngine;

public enum E_Item_Use_Type//�������� ��� Ÿ��
{
    Basic,
    Throw,
    Eat,
    Tool,
    Special
}

public enum E_Item_State//������ ������Ʈ�� ���� ����(���,�÷��̾����� �Ҽ�, �÷��̾��� ���� �߻�. ���߿� �ٸ� ���� �߰� ����)
{
    Stay,
    PlayerMove,
    PlayerHold,
}
//������ �⺻ �����͸� �����ϴ� ScriptableObject.
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