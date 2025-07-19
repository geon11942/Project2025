using UnityEngine;

//현재 사용하지 않음. 나중에 마우스 동작 추가예정
public class MouseManager : MonoBehaviour
{
    public Vector3 mousePosition;
    public Transform CursorObject;

    private ItemControl ItemControlScript;
    [SerializeField]
    private bool Item_Using = false;

    private D_OnClicked d_Click = null;

    private void Start()
    {
        CursorObject = GameObject.Find("MouseCursorObject").transform;
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, -Camera.main.transform.position.z));
        CursorObject.position = mousePosition;

        ItemControlScript=transform.GetComponent<ItemControl>();

        Item_Using = false;

        d_Click += LeftMouse;
        d_Click += RightMouse;
    }
    // Update is called once per frame
    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, -Camera.main.transform.position.z));
        CursorObject.position = mousePosition;

        d_Click();//델리게이트 테스트

        if (Item_Using)
        {
            MouseWheel();
        }
    }
    private void LeftMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //왼쪽 마우스 버튼을 누르면 아이템 사용상태일 때는 아이템을 사용, 아닐 때는 마우스 위치의 아이템을 획득(마우스 획득은 아직 미완성)
            if (Item_Using)
            {
                ItemControlScript.UsingItem();
            }
            else
            {
                //공격기능
            }
        }
    }
    private void RightMouse()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //마우스 오른쪽 버튼을 눌렀을 때 아이템 사용상태가 되고 사용할 아이템을 지정함
            Item_Using = true;
            ItemControlScript.UsingIndexInit();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //아이템 사용상태를 종료하고 사용할 아이템의 이미지를 숨김
            Item_Using = false;
            ItemControlScript.Using_Image.gameObject.SetActive(false);
        }
    }
    private void MouseWheel()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0)
        {
            if (ItemControlScript.Item_UsingIndex < ItemControlScript.Holds.Length - 1)
            {
                ItemControlScript.Item_UsingIndex++;
            }
            else
            {
                ItemControlScript.Item_UsingIndex = 0;
            }
        }
        else if (wheelInput < 0)
        {
            if (ItemControlScript.Item_UsingIndex > 0)
            {
                ItemControlScript.Item_UsingIndex--;
            }
            else
            {
                ItemControlScript.Item_UsingIndex = ItemControlScript.Holds.Length - 1;
            }
        }
        if (ItemControlScript.Item_UsingIndex < 0)
        {
            ItemControlScript.Using_Image.gameObject.SetActive(false);
        }
        else
        {
            if (ItemControlScript.Items[ItemControlScript.Item_UsingIndex] != null)
            {
                ItemControlScript.Using_Image.gameObject.SetActive(true);
                ItemControlScript.Using_Image.GetComponent<SpriteRenderer>().sprite = ItemControlScript.Items[ItemControlScript.Item_UsingIndex].GetComponent<SpriteRenderer>().sprite;
            }
        }
        ItemControlScript.UsingIndexInit();
    }
}
