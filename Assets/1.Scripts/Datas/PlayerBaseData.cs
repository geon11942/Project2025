using UnityEngine;


//플레이어 기초 고정 데이터를 저장하는 ScriptableObject.
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
