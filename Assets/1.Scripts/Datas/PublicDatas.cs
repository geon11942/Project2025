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
    FreeMove,
}

public enum E_Monster_Body_Type//몬스터의 바디 오브젝트 중 메인이 되는 오브젝트. only는 단일 오브젝트, allmain은 모든 오브젝트가 메인
{
    Only,
    HeadMain,
    BodyMain,
    AllMain,
}

public enum E_Monster_Player_Reaction_Type//몬스터가 플레이어를 인식했을 때 기본적인 반응. 평판이나 공격 여부에 따라 달라질 수 있음
{
    Ignore,
    Attack,
    Escape,
    Observe,
}

//몬스터의 현재행동 상태
public enum E_Monster_Current_Action
{
    Wait,
    Attack,
    Move,
    Dead
}

//해당 몬스터에 대한 관계를 설정하는 구조체
[System.Serializable]
public enum E_Monster_Relationship
{
    attack,//이 관계인 상대를 보면 무조건 공격함
    eat,//이 관계인 상대를 보고 상황에 따라 공격함. attack이랑 비슷하지만 뭔가 다를 예정
    beware,//이 관계인 상대에겐 항상 일정 거리를 유지하려함. 가까워지면 일정 거리까지 도망가고 이동할 때 해당 상대를 피해감
    afraid,//이 관계인 상대를 인식하면 그 상대에게서 멀리 도망감. beware랑 달리 인식이 풀려도 도망감
    pack,//이 관계인 상대는 별 상대하지 않음. 동료 등으로 인식하는 취급.
    rivalize,//이 관계인 상대를 보았을 때 자신의 상태가 좋으면 공격함. 상태가 나쁘면 도망침. 해당 기준은 자신의 hp와 sick, mood에 따라 결정
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

//각종 몬스터나 세력에 대한 플레이어의 평판을 저장하는 구조체
//끝에 s가 붙으면 해당 종 전부. 아니면 특수 분류
[System.Serializable]
public struct S_PlayerReputation
{
    //임시로 지정
    public int Rizzards;
    public int Rabbits;
    public int NormalMonkey;
}


//각 몬스터와 다른 몬스터들의 관계들을 저장하는 구조체
[System.Serializable]
public struct S_MonsterRelationshipGroup
{
    //임시로 지정
    public E_Monster_Relationship Red;
    public E_Monster_Relationship Yellow;
}