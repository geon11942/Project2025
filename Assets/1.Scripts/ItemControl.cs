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
            //왼쪽 쉬프트 버튼을 눌렀을 때 정해진 범위에 아이템이 있을 경우 아이템을 획득
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
            //마우스 오른쪽 버튼을 눌렀을 때 아이템 사용상태가 되고 사용할 아이템을 지정함
            Item_Using = true;
            UsingIndexInit();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //아이템 사용상태를 종료하고 사용할 아이템의 이미지를 숨김
            Item_Using = false;
            Using_Image.gameObject.SetActive(false);
        }
        ObjectHolding();
        if(Item_Using)
        {
            //아이템 사용 상태일 때 마우스의 위치를 저장하고 마우스휠을 돌리면 현재 플레이어가 가지고 있는 아이템 중에 사용할 아이템을 변경함
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
            //왼쪽 마우스 버튼을 누르면 아이템 사용상태일 때는 아이템을 사용, 아닐 때는 마우스 위치의 아이템을 획득(마우스 획득은 아직 미완성)
            if(Item_Using)
            {
                UsingItem();
            }
            else
            {
                GetItem();//미완성
            }    
        }
    }
    void ObjectHolding()
    {
        //아이템을 가지고 있을 경우 아이템들을 플레이어의 손 위치로 이동
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
        //아이템을 획득했을 때 플레이어의 손이 비었을 경우 해당 손에 아이템을 저장. 아이템의 상태를 플레이어에게 소속된 상태로 변경
        for(int i=0;i<Items.Length;i++)
        {
            if(Items[i] ==null)
            {
                Items[i] = item.GetComponent<ItemObject>();
                item.GetComponent<ItemObject>().IsState= E_Item_State.PlayerHold;
                break;
            }
        }
        //손이 가득찼을 때에도 아이템을 획득하는 코드(추가 예정)
    }
    void GetItem()
    {
        //미완성
    }
    void UsingItem()
    {
        //아이템이 없을 경우 사용X. 있을 경우 아이템의 사용 타입에 따라 사용
        if(Item_UsingIndex<0)
        {
            return;
        }
        switch(Items[Item_UsingIndex].GetComponent<ItemObject>().Item_Data.type)
        {
            case E_Item_Use_Type.Basic://기본 타입. 아이템을 약하게 마우스 방향으로 던짐
                Items[Item_UsingIndex].StartMovement(mouseWorldPos, 5f);
                Items[Item_UsingIndex].GetComponent<ItemObject>().IsState = E_Item_State.PlayerMove;
                Items[Item_UsingIndex] = null;
                break;
            case E_Item_Use_Type.Throw://투척 타입. 아이템을 강하게 마우스 방향으로 던짐
                Items[Item_UsingIndex].StartMovement(mouseWorldPos, 20f);
                Items[Item_UsingIndex].GetComponent<ItemObject>().IsState = E_Item_State.PlayerMove;
                Items[Item_UsingIndex] = null;
                break;
            case E_Item_Use_Type.Tool://추가 예정
                break;
            case E_Item_Use_Type.Eat://음식 타입. 현재는 아이템을 제거하고 끝이지만 나중에 사용효과 추가예정
                Destroy(Items[Item_UsingIndex].gameObject);
                Items[Item_UsingIndex] = null;
                break;
            case E_Item_Use_Type.Special://특수 타입. 해당 아이템 ID에 따른 고유 효과 발동 예정
                break;
            default://만약 사용 타입이 없는 아이템을 사용할 경우 아이템이 있는 자리에 아이템을 내려둠. 이 부분은 고쳐서 바닥에 바로 버리는 기능으로 변경예정
                Items[Item_UsingIndex].GetComponent<ItemObject>().IsState = E_Item_State.Stay;
                Items[Item_UsingIndex] = null;
                break;
        }
        if(Items[Item_UsingIndex]==null)
        {
            //아이템을 사용했을 경우 사용할 아이템을 재지정
            Item_UsingIndex = -1;
            UsingIndexInit();
        }
    }

    void UsingIndexInit()
    {
        //사용할 아이템의 번호를 지정. 손들을 하나씩 확인해서 아이템을 가지고 있을 경우 해당 손의 아이템을 사용할 아이템으로 지정
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
        //만약 모든 손이 비었을 경우 사용할 아이템을 지정하지 않음.
        Item_UsingIndex = -1;
    }
}
