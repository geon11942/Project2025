using UnityEngine;
using static UnityEditor.Progress;

public class ItemControl : MonoBehaviour
{
    [SerializeField]
    private ObjectSearch Item_Search_Object;
    [SerializeField]
    private Transform Holds_Parent_Object;
    [SerializeField]
    public Transform Using_Image;
    [SerializeField]
    public Transform[] Holds;
    [SerializeField]
    public ItemObject[] Items;

    [SerializeField]
    public int Item_UsingIndex = -1;
    private void Start()
    {
        Item_Search_Object = transform.GetChild(1).GetComponent<ObjectSearch>();
        Holds_Parent_Object=transform.GetChild(2).GetComponent<Transform>();
        Using_Image = transform.GetChild(3).GetComponent<Transform>();
        Holds = new Transform[Holds_Parent_Object.childCount];
        Items= new ItemObject[Holds.Length];
        for (int i = 0; i < Holds_Parent_Object.childCount; i++) 
        {
            Holds[i]= Holds_Parent_Object.GetChild(i).GetComponent<Transform>();
        }
        Using_Image.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            //���� ����Ʈ ��ư�� ������ �� ������ ������ �������� ���� ��� �������� ȹ��
            if (Item_Search_Object.GetClosestTarget() != null) 
            {
                ItemHoldSetting(Item_Search_Object.GetClosestTarget());
                UsingIndexInit();
            }
        }
        ObjectHolding();

    }
    private void ObjectHolding()
    {
        //�������� ������ ���� ��� �����۵��� �÷��̾��� �� ��ġ�� �̵�
        for (int i = 0; i < Holds.Length; i++) 
        {
            if (Items[i] != null)
            {
                Items[i].transform.position = Holds[i].transform.position;
            }
        }
    }
    private void ItemHoldSetting(Transform item)
    {
        //�������� ȹ������ �� �÷��̾��� ���� ����� ��� �ش� �տ� �������� ����. �������� ���¸� �÷��̾�� �Ҽӵ� ���·� ����
        for(int i=0;i<Items.Length;i++)
        {
            if(Items[i] ==null)
            {
                item.GetComponent<ItemObject>().IsState = E_Item_State.PlayerHold;
                Items[i] = item.GetComponent<ItemObject>();
                Items[i].IsState= E_Item_State.PlayerHold;
                break;
            }
        }
        //���� ����á�� ������ �������� ȹ���ϴ� �ڵ�(�߰� ����)
    }
    public void GetItem()
    {
        //�̿ϼ�
    }
    public void UsingItem(Vector3 mousepos)
    {
        //�������� ���� ��� ���X. ���� ��� �������� ��� Ÿ�Կ� ���� ���
        if(Item_UsingIndex<0)
        {
            return;
        }
        switch(Items[Item_UsingIndex].GetComponent<ItemObject>().Item_Data.type)
        {
            case E_Item_Use_Type.Basic://�⺻ Ÿ��. �������� ���ϰ� ���콺 �������� ����
                Items[Item_UsingIndex].StartMovement(mousepos, 5f);
                Items[Item_UsingIndex].GetComponent<ItemObject>().IsState = E_Item_State.PlayerMove;
                Items[Item_UsingIndex] = null;
                break;
            case E_Item_Use_Type.Throw://��ô Ÿ��. �������� ���ϰ� ���콺 �������� ����
                Items[Item_UsingIndex].StartMovement(mousepos, 20f);
                Items[Item_UsingIndex].GetComponent<ItemObject>().IsState = E_Item_State.PlayerMove;
                Items[Item_UsingIndex] = null;
                break;
            case E_Item_Use_Type.Tool://�߰� ����
                break;
            case E_Item_Use_Type.Eat://���� Ÿ��. ����� �������� �����ϰ� �������� ���߿� ���ȿ�� �߰�����
                Destroy(Items[Item_UsingIndex].gameObject);
                Items[Item_UsingIndex] = null;
                break;
            case E_Item_Use_Type.Special://Ư�� Ÿ��. �ش� ������ ID�� ���� ���� ȿ�� �ߵ� ����
                break;
            default://���� ��� Ÿ���� ���� �������� ����� ��� �������� �ִ� �ڸ��� �������� ������. �� �κ��� ���ļ� �ٴڿ� �ٷ� ������ ������� ���濹��
                Items[Item_UsingIndex].GetComponent<ItemObject>().IsState = E_Item_State.Stay;
                Items[Item_UsingIndex] = null;
                break;
        }
        if(Items[Item_UsingIndex]==null)
        {
            //�������� ������� ��� ����� �������� ������
            Item_UsingIndex = -1;
            UsingIndexInit();
        }
    }

    public void UsingIndexInit()
    {
        //����� �������� ��ȣ�� ����. �յ��� �ϳ��� Ȯ���ؼ� �������� ������ ���� ��� �ش� ���� �������� ����� ���������� ����
        if(Item_UsingIndex==-1)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i]!=null)
                {
                    Item_UsingIndex = i;
                    return;
                }
            }
        }
        else
        {
            for (int i = Item_UsingIndex; i < Items.Length; i++)
            {
                if (Items[i] != null)
                {
                    Item_UsingIndex = i;
                    return;
                }
            }
            for (int i = 0; i < Item_UsingIndex; i++)
            {
                if (Items[i] != null)
                {
                    Item_UsingIndex = i;
                    return;
                }
            }
        }
        //���� ��� ���� ����� ��� ����� �������� �������� ����.
        Item_UsingIndex = -1;
    }
}
