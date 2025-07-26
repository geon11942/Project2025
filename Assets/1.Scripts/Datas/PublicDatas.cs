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

public enum E_Monster_Body_Type//������ �ٵ� ������Ʈ �� ������ �Ǵ� ������Ʈ. only�� ���� ������Ʈ, allmain�� ��� ������Ʈ�� ����
{
    Only,
    HeadMain,
    BodyMain,
    AllMain,
}

public enum E_Monster_Player_Reaction_Type//���Ͱ� �÷��̾ �ν����� �� �⺻���� ����. �����̳� ���� ���ο� ���� �޶��� �� ����
{
    Ignore,
    Attack,
    Escape,
    Observe,
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

//���� ���ͳ� ���¿� ���� �÷��̾��� ������ �����ϴ� ����ü
//���� s�� ������ �ش� �� ����. �ƴϸ� Ư�� �з�
[System.Serializable]
public struct S_PlayerReputation
{
    //�ӽ÷� ����
    public int Rizzards;
    public int Rabbits;
    public int NormalMonkey;
}

//�ش� ���Ϳ� ���� ���踦 �����ϴ� ����ü
[System.Serializable]
public struct S_MonsterRelationship
{
    public float attack;//�� ��ġ�� ���� ��븦 ���� ������ ������
    public float eat;//�� ��ġ�� ���� ��븦 ���� ��Ȳ�� ���� ������. attack�̶� ��������� ���� �ٸ� ����
    public float beware;//�� ��ġ�� ���� ��뿡�� �׻� ���� �Ÿ��� �����Ϸ���. ��������� ���� �Ÿ����� �������� �̵��� �� �ش� ��븦 ���ذ�
    public float afraid;//�� ��ġ�� ���� ��븦 �ν��ϸ� �� ��뿡�Լ� �ָ� ������. beware�� �޸� �ν��� Ǯ���� ������
    public float pack;//�� ��ġ�� ���� ���� �� ������� ����. ���� ������ �ν��ϴ� ���.
    public float rivalize;//�� ��ġ�� ���� ��븦 ������ �� �ڽ��� ���°� ������ ������. ���°� ���ڸ� ����ħ. �ش� ������ �ڽ��� hp�� sick, mood�� ���� ����
}

//�� ���Ϳ� �ٸ� ���͵��� ������� �����ϴ� ����ü
[System.Serializable]
public struct S_MonsterRelationshipGroup
{
    //�ӽ÷� ����
    public S_MonsterRelationship bigrizzard;
    public S_MonsterRelationship smallrizzard;
    public S_MonsterRelationship rabbit;
}