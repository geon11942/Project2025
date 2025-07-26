using UnityEngine;


//�÷��̾� ���� ���� �����͸� �����ϴ� ScriptableObject.
[CreateAssetMenu(fileName = "PlayerBaseDataTable", menuName = "Scriptable Object/PlayerBaseDataTable", order = int.MaxValue)]
public class PlayerBaseDataTable : ScriptableObject
{
    public string Name;

    public float MaxHp;
    public float MaxMana;

    public float Power;
    public float Speed;
    public float ManaRegenSpeed;

    public int HandNum;
    public float SearchRange;

}
