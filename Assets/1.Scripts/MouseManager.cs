using UnityEngine;

//���� ������� ����. ���߿� ���콺 ���� �߰�����
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

        d_Click();//��������Ʈ �׽�Ʈ

        if (Item_Using)
        {
            MouseWheel();
        }
    }
    private void LeftMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //���� ���콺 ��ư�� ������ ������ �������� ���� �������� ���, �ƴ� ���� ���콺 ��ġ�� �������� ȹ��(���콺 ȹ���� ���� �̿ϼ�)
            if (Item_Using)
            {
                ItemControlScript.UsingItem();
            }
            else
            {
                //���ݱ��
            }
        }
    }
    private void RightMouse()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //���콺 ������ ��ư�� ������ �� ������ �����°� �ǰ� ����� �������� ������
            Item_Using = true;
            ItemControlScript.UsingIndexInit();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            //������ �����¸� �����ϰ� ����� �������� �̹����� ����
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
