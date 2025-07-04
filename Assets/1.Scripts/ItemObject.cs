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
    //플레이어에 의해 이동상태가 됐을 때 전달된 마우스의 위치 방향에 전달된 속도로 이동
    void ItemMoving()
    {
        if (IsState != E_Item_State.PlayerMove) return;

        timer += Time.deltaTime;

        if (timer < movingTime)
        {
            // movingTime만큼 전달된 속도로 이동
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, currentSpeed * Time.deltaTime);
        }
        else
        {
            // movingtime만큼 이동 후 감속
            if (currentSpeed > 0)
            {
                currentSpeed -= Time.deltaTime * (speed / 0.3f); // 0.5초 내 감속
                currentSpeed = Mathf.Max(currentSpeed, 0f);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, currentSpeed * Time.deltaTime);
            }
            else
            {
                IsState = E_Item_State.Stay;
            }
        }
    }    
    //아이템 데이터가 있을 경우 해당 아이템의 이미지와 데이터를 가져옴. 없을 경우 대체 이미지 적용
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
    //플레이어에 의해 발생하는 함수. 기본적으로 마우스 좌표와 속도를 받아온 후 해당 위치 방향에 받은 속도로 이동
    public void StartMovement(Vector3 targetPosition,float spd)
    {
        moveDirection = (targetPosition - transform.position).normalized;
        speed = spd;
        currentSpeed = speed;
        IsState = E_Item_State.PlayerMove;
    }
    //이동 중에 다른 오브젝트와 부딪혔을 경우 작동(아직 미완성)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsState == E_Item_State.PlayerMove)
        {
            if (collision.CompareTag("Monster"))
            {
                IsState = E_Item_State.Stay;//임시처리
                return;
            }
        }
    }
    //어디에 소속되지 않고 이동중이 아닐 때 플레이어와 접촉할 경우 플레이어의 반대방향으로 밀려남
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
    //밀려나다가 플레이어와 벗어났을 경우 이동종료
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
