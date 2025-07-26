using UnityEngine;

//몬스터 기초 고정 데이터를 저장하는 ScriptableObject.
[CreateAssetMenu(fileName = "MonsterBaseDataTable", menuName = "Scriptable Object/MonsterBaseDataTable", order = int.MaxValue)]
public class MonsterBaseDataTable : ScriptableObject
{
    public string Name;

    public E_Monster_Body_Type Body_Type;
    public E_Monster_Player_Reaction_Type Reaction_Type;

    public float MaxHp;
    public float MaxMana;

    public float Power;
    public float Speed;
    public float ManaRegenSpeed;

    public float Intelligence;
    [Range(0f, 50f)]
    public float FOVRange;
    [Range(0f, 360f)]
    public float FOVAngle;
    [Range(0f,10f)]
    public float MinPlayerHostility;

    public S_MonsterRelationshipGroup RelationshipGroup;
}
