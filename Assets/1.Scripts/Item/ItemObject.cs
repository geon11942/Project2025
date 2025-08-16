using TMPro;
using Unity.VisualScripting;
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

    private bool Disposable = false;
    private bool Bounce = false;

    private ItemEvent itemEvent;
    public D_ItemEvent d_Event;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        itemEvent = GameObject.Find("ItemEventManager").GetComponent<ItemEvent>();
        Item_Data_Set();
        ItemEventSetting();
    }
    public void SpawnInIt()
    {
        Item_Data_Set();
        ItemEventSetting();
    }
    private void Update()
    {
        ItemMoving();
    }
    //�÷��̾ ���� �̵����°� ���� �� ���޵� ���콺�� ��ġ ���⿡ ���޵� �ӵ��� �̵�
    private void ItemMoving()
    {
        if (!(IsState == E_Item_State.PlayerMove || IsState ==E_Item_State.FreeMove)) return;

        timer += Time.deltaTime;

        if (timer < movingTime)
        {
            // movingTime��ŭ ���޵� �ӵ��� �̵�
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, currentSpeed * Time.deltaTime);
            if(Item_Data.ID == 4)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }

        }
        else
        {
            // movingtime��ŭ �̵� �� ����
            if (currentSpeed > 0)
            {
                //��ź���� �Ϻ� �������� ���ӵ� �� ���Ӵ�� �浹ȿ�� �ߵ�
                if (Disposable)
                {
                    DisposableEffect();
                }

                currentSpeed -= Time.deltaTime * (speed / 0.3f); // 0.5�� �� ����
                currentSpeed = Mathf.Max(currentSpeed, 0f);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, currentSpeed * Time.deltaTime);
            }
            else
            {
                timer = 0f;
                IsState = E_Item_State.Stay;
                Bounce = false;
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
            if(collision.CompareTag("Wall"))
            {

                //1ȸ�� �������� ���ȿ�� �ߵ�
                if (Disposable)
                {
                    DisposableEffect();
                }

                timer = 0f;
                IsState = E_Item_State.Stay;
                Bounce = false;
                return;
            }
            if (collision.CompareTag("Monster"))
            {
                collision.GetComponent<MonsterBodyParts>()?.HitPlayerAttack(1f);
                collision.GetComponent<MonsterData>()?.HitPlayerAttack(1f);

                //1ȸ�� �������� ���ȿ�� �ߵ�
                if (Disposable)
                {
                    DisposableEffect();
                }

                //���� ���� ������Ʈ�� �΋H���� �ش� ������Ʈ���� �ݴ�������� ƨ�ܳ���. �� �� ���� ����?
                if(!Bounce)
                {
                    moveDirection= -(collision.transform.position - transform.position).normalized;
                    Bounce = true;
                    timer = 1f + timer * 2f;
                    speed = speed * 2f;
                    IsState = E_Item_State.FreeMove;
                }
                return;
            }
        }
    }



    //���߿� ���ļ� �ٸ� ������� ���

    //��� �Ҽӵ��� �ʰ� �̵����� �ƴ� �� �÷��̾�� ������ ��� �÷��̾��� �ݴ�������� �з���
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(IsState == E_Item_State.Stay)
        {
            if (collision.CompareTag("Player"))
            {
                // Vector2 pushDirection = (transform.position - collision.transform.position).normalized;

                //rb.AddForce(pushDirection * 0.01f, ForceMode2D.Force); ���߿� �ٽ� ���
            }
        }
    }
    //�з����ٰ� �÷��̾�� ����� ��� �̵�����
    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsState == E_Item_State.Stay)
        {
            if (other.CompareTag("Player"))
            {
                //rb.linearVelocity = Vector2.zero; //���߿� ���ļ� �ٽ� ���
            }
        }
    }

    //�������� ID�� ���� ���ȿ���� �����ϴ� �Լ�
    public void ItemEventSetting()
    {
        if (Item_Data == null)
            return;

        switch (Item_Data.ID)
        {
            case 0:
                d_Event += Basic;
                break;
            case 1:
                d_Event += Throw;
                break;
            case 2:
                d_Event += Throw;
                Disposable = true;
                break;
            case 3:
                d_Event += Throw;
                Disposable = true;
                break;
            case 4:
                d_Event += Throw;
                break;
            default:
                d_Event += Basic;
                break;
        }
    }

    //�������� ���ſ� ���ŵ� �� ȿ���� �ִ� �������� �ش� ȿ���� �ߵ��ϴ� �Լ�
    private void DisposableEffect()
    {
        Destroy(gameObject);
        if (Item_Data.ID == 2)
        {
            GameObject boomeffect = Instantiate(Resources.Load("Prefabs/BoomEffect") as GameObject);
            boomeffect.transform.position = transform.position;
            Destroy(boomeffect,0.1f);
            //��ź�̳� �׿ܿ� ���Ž� Ư��ȿ���� �ִ� �������� ȿ�� �ߵ�
        }
    }




    //�� �Ʒ��� �������� ���ȿ�� �Լ���. ���߿� ItemEvent�� �Űܼ� ���
    public void Basic()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, -Camera.main.transform.position.z));
        StartMovement(mousePosition, 1f);
        IsState = E_Item_State.PlayerMove;
    }

    public void Throw()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, -Camera.main.transform.position.z));
        StartMovement(mousePosition, 20f);
        IsState = E_Item_State.PlayerMove;
    }
}
