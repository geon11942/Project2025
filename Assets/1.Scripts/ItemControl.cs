using UnityEngine;
using static UnityEditor.Progress;

public class ItemControl : MonoBehaviour
{
    [SerializeField]
    ObjectSearch Item_Search_Object;
    [SerializeField]
    Transform Holds_Parent_Object;
    [SerializeField]
    Transform Using_Image;
    [SerializeField]
    Transform[] Holds;
    [SerializeField]
    ItemObject[] Items;

    [SerializeField]
    bool Item_Using = false;
    [SerializeField]
    int Item_UsingIndex = -1;
    Vector3 mouseWorldPos;
    void Start()
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
        Item_Using = false;
        Using_Image.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        {
            //���� ����Ʈ ��ư�� ������ �� ������ ������ �������� ���� ��� �������� ȹ��
            if (Item_Search_Object.GetClosestTarget() != null) 
            {
                ItemHoldSetting(Item_Search_Object.GetClosestTarget());
                if(Item_Using)
                {
                    UsingIndexInit();
                }
            }
        }
        if(Input.GetMouseButtonDown(1))
        {
            //���콺 ������ ��ư�� ������ �� ������ �����°� �ǰ� ����� �������� ������
            Item_Using = true;
            UsingIndexInit();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //������ �����¸� �����ϰ� ����� �������� �̹����� ����
            Item_Using = false;
            Using_Image.gameObject.SetActive(false);
        }
        ObjectHolding();
        if(Item_Using)
        {
            //������ ��� ������ �� ���콺�� ��ġ�� �����ϰ� ���콺���� ������ ���� �÷��̾ ������ �ִ� ������ �߿� ����� �������� ������
            mouseWorldPos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            float wheelInput = Input.GetAxis("Mouse ScrollWheel"); 
            if (wheelInput > 0)
            {
                if (Item_UsingIndex < Holds.Length - 1)
                {
                    Item_UsingIndex++;
                }
                else
                {
                    Item_UsingIndex = 0;
                }
            }
            else if (wheelInput < 0)
            {
                if (Item_UsingIndex > 0)
                {
                    Item_UsingIndex--;
                }
                else
                {
                    Item_UsingIndex = Holds.Length - 1;
                }
            }
            if (Item_UsingIndex <0)
            {
                Using_Image.gameObject.SetActive(false);
            }
            else
            {
                if (Items[Item_UsingIndex] != null)
                {
                    Using_Image.gameObject.SetActive(true);
                    Using_Image.GetComponent<SpriteRenderer>().sprite = Items[Item_UsingIndex].GetComponent<SpriteRenderer>().sprite;
                }
            }
        }
        if(Input.GetMouseButtonDown(0))
        {
            //���� ���콺 ��ư�� ������ ������ �������� ���� �������� ���, �ƴ� ���� ���콺 ��ġ�� �������� ȹ��(���콺 ȹ���� ���� �̿ϼ�)
            if(Item_Using)
            {
                UsingItem();
            }
            else
            {
                GetItem();//�̿ϼ�
            }    
        }
    }
    void ObjectHolding()
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
    void ItemHoldSetting(Transform item)
    {
        //�������� ȹ������ �� �÷��̾��� ���� ����� ��� �ش� �տ� �������� ����. �������� ���¸� �÷��̾�� �Ҽӵ� ���·� ����
        for(int i=0;i<Items.Length;i++)
        {
            if(Items[i] ==null)
            {
                Items[i] = item.GetComponent<ItemObject>();
                item.GetComponent<ItemObject>().IsState= E_Item_State.PlayerHold;
                break;
            }
        }
        //���� ����á�� ������ �������� ȹ���ϴ� �ڵ�(�߰� ����)
    }
    void GetItem()
    {
        //�̿ϼ�
    }
    void UsingItem()
    {
        //�������� ���� ��� ���X. ���� ��� �������� ��� Ÿ�Կ� ���� ���
        if(Item_UsingIndex<0)
        {
            return;
        }
        switch(Items[Item_UsingIndex].GetComponent<ItemObject>().Item_Data.type)
        {
            case E_Item_Use_Type.Basic://�⺻ Ÿ��. �������� ���ϰ� ���콺 �������� ����
                Items[Item_UsingIndex].StartMovement(mouseWorldPos, 5f);
                Items[Item_UsingIndex].GetComponent<ItemObject>().IsState = E_Item_State.PlayerMove;
                Items[Item_UsingIndex] = null;
                break;
            case E_Item_Use_Type.Throw://��ô Ÿ��. �������� ���ϰ� ���콺 �������� ����
                Items[Item_UsingIndex].StartMovement(mouseWorldPos, 20f);
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

    void UsingIndexInit()
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
