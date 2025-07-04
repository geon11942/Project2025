using UnityEngine;
using UnityEngine.EventSystems;

public class ItemObject : MonoBehaviour
{
    public ItemDataTable Item_Data;
    Vector3 moveDirection;
    public E_Item_State IsState=E_Item_State.Stay;
    public float speed;
    public float movingTime;

    private float timer = 0f;
    private float currentSpeed;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        Item_Data_Set();
    }

    void Update()
    {
        ItemMoving();
    }
    //�÷��̾ ���� �̵����°� ���� �� ���޵� ���콺�� ��ġ ���⿡ ���޵� �ӵ��� �̵�
    void ItemMoving()
    {
        if (IsState != E_Item_State.PlayerMove) return;

        timer += Time.deltaTime;

        if (timer < movingTime)
        {
            // movingTime��ŭ ���޵� �ӵ��� �̵�
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, currentSpeed * Time.deltaTime);
        }
        else
        {
            // movingtime��ŭ �̵� �� ����
            if (currentSpeed > 0)
            {
                currentSpeed -= Time.deltaTime * (speed / 0.3f); // 0.5�� �� ����
                currentSpeed = Mathf.Max(currentSpeed, 0f);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, currentSpeed * Time.deltaTime);
            }
            else
            {
                IsState = E_Item_State.Stay;
            }
        }
    }    
    //������ �����Ͱ� ���� ��� �ش� �������� �̹����� �����͸� ������. ���� ��� ��ü �̹��� ����
    public void Item_Data_Set()
    {
        if (Item_Data == null)
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Items/Items_none");
            return;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/Items/{Item_Data.Image_Name}");
            movingTime = Item_Data.MovingTime;
        }
    }
    //�÷��̾ ���� �߻��ϴ� �Լ�. �⺻������ ���콺 ��ǥ�� �ӵ��� �޾ƿ� �� �ش� ��ġ ���⿡ ���� �ӵ��� �̵�
    public void StartMovement(Vector3 targetPosition,float spd)
    {
        moveDirection = (targetPosition - transform.position).normalized;
        speed = spd;
        currentSpeed = speed;
        IsState = E_Item_State.PlayerMove;
    }
    //�̵� �߿� �ٸ� ������Ʈ�� �ε����� ��� �۵�(���� �̿ϼ�)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsState == E_Item_State.PlayerMove)
        {
            if (collision.CompareTag("Monster"))
            {
                IsState = E_Item_State.Stay;//�ӽ�ó��
                return;
            }
        }
    }
    //��� �Ҽӵ��� �ʰ� �̵����� �ƴ� �� �÷��̾�� ������ ��� �÷��̾��� �ݴ�������� �з���
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(IsState == E_Item_State.Stay)
        {
            if (collision.CompareTag("Player"))
            {
                Vector2 pushDirection = (transform.position - collision.transform.position).normalized;

                rb.AddForce(pushDirection * 0.01f, ForceMode2D.Force);
            }
        }
    }
    //�з����ٰ� �÷��̾�� ����� ��� �̵�����
    void OnTriggerExit2D(Collider2D other)
    {
        if (IsState == E_Item_State.Stay)
        {
            if (other.CompareTag("Player"))
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
