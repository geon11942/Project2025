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
    //플레이어에 의해 이동상태가 됐을 때 전달된 마우스의 위치 방향에 전달된 속도로 이동
    private void ItemMoving()
    {
        if (!(IsState == E_Item_State.PlayerMove || IsState ==E_Item_State.FreeMove)) return;

        timer += Time.deltaTime;

        if (timer < movingTime)
        {
            // movingTime만큼 전달된 속도로 이동
            transform.position = Vector3.MoveTowards(transform.position, transform.position + moveDirection, currentSpeed * Time.deltaTime);
            if(Item_Data.ID == 4)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }

        }
        else
        {
            // movingtime만큼 이동 후 감속
            if (currentSpeed > 0)
            {
                //폭탄같은 일부 아이템은 감속될 때 감속대신 충돌효과 발동
                if (Disposable)
                {
                    DisposableEffect();
                }

                currentSpeed -= Time.deltaTime * (speed / 0.3f); // 0.5초 내 감속
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
            if(collision.CompareTag("Wall"))
            {

                //1회성 아이템의 사용효과 발동
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

                //1회성 아이템의 사용효과 발동
                if (Disposable)
                {
                    DisposableEffect();
                }

                //몬스터 등의 오브젝트와 부딫히면 해당 오브젝트와의 반대방향으로 튕겨나감. 좀 더 수정 예정?
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



    //나중에 고쳐서 다른 방식으로 사용

    //어디에 소속되지 않고 이동중이 아닐 때 플레이어와 접촉할 경우 플레이어의 반대방향으로 밀려남
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(IsState == E_Item_State.Stay)
        {
            if (collision.CompareTag("Player"))
            {
                // Vector2 pushDirection = (transform.position - collision.transform.position).normalized;

                //rb.AddForce(pushDirection * 0.01f, ForceMode2D.Force); 나중에 다시 사용
            }
        }
    }
    //밀려나다가 플레이어와 벗어났을 경우 이동종료
    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsState == E_Item_State.Stay)
        {
            if (other.CompareTag("Player"))
            {
                //rb.linearVelocity = Vector2.zero; //나중에 고쳐서 다시 사용
            }
        }
    }

    //아이템의 ID에 따라 사용효과를 설정하는 함수
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

    //아이템의 제거와 제거될 때 효과가 있는 아이템은 해당 효과를 발동하는 함수
    private void DisposableEffect()
    {
        Destroy(gameObject);
        if (Item_Data.ID == 2)
        {
            GameObject boomeffect = Instantiate(Resources.Load("Prefabs/BoomEffect") as GameObject);
            boomeffect.transform.position = transform.position;
            Destroy(boomeffect,0.1f);
            //폭탄이나 그외에 제거시 특수효과가 있는 아이템의 효과 발동
        }
    }




    //이 아래론 아이템의 사용효과 함수들. 나중에 ItemEvent로 옮겨서 사용
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
