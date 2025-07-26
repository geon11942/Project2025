using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    PlayerData PlayerData;
    public float acceleration = 20f;
    public float deceleration = 30f;
    public float turnThresholdAngle = 150f;

    private Vector2 velocity = Vector2.zero;
    private Vector2 inputDirection;
    private bool isBraking = false;
    private bool isMoving = false;
    private void Start()
    {
        PlayerData = gameObject.GetComponent<PlayerData>();
    }
    private void Update()
    {
        //플레이어의 이동처리. 이동 중 반대 방향으로 급격히 꺾을 떄 감속 후 이동 기능을 추가.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        inputDirection = new Vector2(h, v).normalized;

        if (inputDirection != Vector2.zero)
        {
            float angleBetween = Vector2.Angle(velocity.normalized, inputDirection);
            isMoving = true;
            // 급반전 감지
            if (velocity.magnitude > 0.1f && angleBetween > turnThresholdAngle)
            {
                isBraking = true;
            }

            // 브레이크 처리
            if (isBraking)
            {
                velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.deltaTime);

                if (velocity.magnitude < 0.1f)
                {
                    isBraking = false;
                    velocity = inputDirection * PlayerData.FinalSpeed;
                }
            }
            else
            {
                Vector2 desiredVelocity = inputDirection * PlayerData.FinalSpeed;
                velocity = Vector2.MoveTowards(velocity, desiredVelocity, acceleration * Time.deltaTime);
            }
        }
        else
        {
            // 입력 없을 때 감속
            velocity = Vector2.MoveTowards(velocity, Vector2.zero, deceleration * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            isMoving = false;
            velocity = Vector2.zero;
        }
        // 이동 
        if (isMoving)
        {
            Vector3 movement = new Vector3(velocity.x, velocity.y, 0f) * Time.deltaTime;
            transform.position += movement;
        }
        else
        {
            Vector3 movement = new Vector3(transform.position.x, transform.position.y, 0f);
        }
        isMoving = false;
    }
}
