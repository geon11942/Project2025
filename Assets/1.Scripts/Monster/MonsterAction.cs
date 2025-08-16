using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class MonsterAction : MonoBehaviour
{
    private MonsterData monsterData;
    private D_Action Action = null;// 몬스터마다 지정할 행동을 담아둠
    //private D_Action FreeAction = null;// 따른 행동을 하지 않을 때 자유행동
    private D_Action AttackAction = null;

    [SerializeField]
    private float angle;
    [SerializeField]
    private float ActionTime = 0f;
    [SerializeField]
    private float AttackTime = 0f;
    public float SetTime = 1f;

    public LayerMask DetectionLayer; //다른 몬스터나 플레이어 레이어

    [SerializeField]
    private Transform TargetObject = null;

    public List<Transform> DetectedObjects = new List<Transform>();//인식한 몹,플레이어 저장용
    [SerializeField]
    private List<Transform> MyParts = new List<Transform>();//자기 신체 오브젝트

    [SerializeField]
    private Transform SightObject;

    [SerializeField]
    private Vector3 SpawnPos;
    [SerializeField]
    private Vector3 MovePos;
    private bool MovePosCheck = false;

    private void Start()
    {
        monsterData = gameObject.GetComponent<MonsterData>();
        SightObject = transform.GetChild(0);

        SpawnPos = transform.position;

        MyParts.Add(transform);

        ActionSetting();
    }
    private void Update()
    {
        if(monsterData.States.Contains("Stun"))
        {
            ActionTime += Time.deltaTime;
            if (ActionTime > 3f) 
            {
                monsterData.States.Remove("Stun");
            }
            else
            {
                return;
            }
        }
        if (Action != null)
        {
            Action();
        }
        if (AttackAction != null)
        {
            AttackAction();
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (monsterData.States.Contains("Charge"))
            {
                monsterData.States.Add("Stun");
                monsterData.States.Remove("Charge");
                monsterData.MonsterCurentAction = E_Monster_Current_Action.Move;
                ActionTime = 0f;
            }
        }
    }


    private void ActionSetting()
    {
        switch (monsterData.MonsterBaseData.Name)
        {
            case "BigTestRizzard":
                Action += DetectObjects;
                Action += TargetSelect;
                Action += SpawnPosRandomMove;
                Action += TargetMove;
                AttackAction += ChargeAttack;
                break;
            case "SmallTestRizzard":
                Action += DetectObjects;
                Action += TargetSelect;
                Action += SpawnPosRandomMove;
                Action += TargetRun;
                break;
            default:
                break;
        }
    }

    //이 아래론 Action에 넣을 함수들


    //자신의 시야에 들어온 오브젝트 중 플레이어나 다른 몬스터를 저장
    private void DetectObjects()
    {
        if (DeadCheck())
            return;

        DetectedObjects.Clear();
        // 주변 모든 콜라이더 가져오기 (원 범위)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, monsterData.MonsterBaseData.FOVRange, DetectionLayer);

        foreach (Collider2D hit in hits)
        {
            if (MyParts.Contains(hit.transform)) continue;//자신에게 포함된 오브젝트일 경우 무시

            Vector2 toTarget = (hit.transform.position - transform.position).normalized;
            Vector2 forward = SightObject.transform.right;  // 오른쪽이 2D에서 forward 방향

            float angleToTarget = Vector2.Angle(forward, toTarget);

            if (angleToTarget <= monsterData.MonsterBaseData.FOVAngle / 2f)
            {
                // 범위 안의 대상 추가
                DetectedObjects.Add(hit.transform);
            }
        }
    }

    //자신에게 저장된 다른 개체중에 거리나 개체별 보정치에 따라 점수를 매기고 가장 점수가 높은 대상을 타겟으로 선정
    private void TargetSelect()
    {
        if (DeadCheck())
            return;

        if (ActionTime <= 0f)
        {
            TargetObject = null;
        }
        if (DetectedObjects.Count == 0) return;

        float Dist;

        float SelectPoint = 0f;
        float TargetPoint = 0f;

        foreach (var target in DetectedObjects)
        {
            TargetPoint = 0f;
            Dist = Vector2.Distance(transform.position, target.position);
            if (target.tag == "Player")
            {
                TargetPoint = Dist * monsterData.PlayerHostility * 1f; //1f는 나중에 평판 별로 정해진 수치를 받아 변경되게 바꾸기
            }
            else
            {
                TargetPoint = 20f;
                //다른 몬스터에 대한 점수판정
            }
            if (TargetPoint > SelectPoint)
            {
                GameObject awakeObject = null;
                if (TargetObject != null)
                {
                    awakeObject = TargetObject.gameObject;
                }
                SelectPoint = TargetPoint;
                TargetObject = target;
                if (awakeObject != null)
                {
                    if (awakeObject == TargetObject)
                        break;
                }
                if (monsterData.MonsterCurentAction != E_Monster_Current_Action.Attack)
                {
                    ActionTime = SetTime;
                }

                monsterData.MonsterCurentAction = E_Monster_Current_Action.Move;
            }
        }

    }

    //타겟오브젝트가 있으면 해당 오브젝트 추적. 나중엔 없을 때 자유행동하게 수정
    private void TargetMove()
    {
        if (DeadCheck())
            return;

        if (monsterData.MonsterCurentAction == E_Monster_Current_Action.Move)
        {
            if (TargetObject != null)
            {
                Vector2 direction = (TargetObject.position - transform.position);
                float distance = direction.magnitude;

                if (distance > (monsterData.MonsterBaseData.FOVRange * 0.5f) * (0.9f + monsterData.MonsterBaseData.Intelligence))
                {
                    Vector2 moveDir = direction.normalized;

                    transform.position += (Vector3)(moveDir * monsterData.FinalSpeed * Time.deltaTime);

                    SpriteRotation(moveDir);

                }
                else
                {
                    if (monsterData.MonsterCurentAction == E_Monster_Current_Action.Move)
                    {
                        ActionTime = SetTime * 5f;
                        if (AttackTime <= 0)
                        {
                            AttackTime = 1.5f - (monsterData.MonsterBaseData.Intelligence * 0.5f);
                        }
                        monsterData.MonsterCurentAction = E_Monster_Current_Action.Attack;
                    }
                }
                ActionTime -= Time.deltaTime;
            }
        }
    }

    private void SpawnPosRandomMove()
    {
        if (DeadCheck())
            return;

        if (TargetObject == null)
        {
            ActionTime -= Time.deltaTime * 0.2f;
            if (!MovePosCheck)
            {
                MovePosCheck = true;
                MovePos = new Vector3(SpawnPos.x + Random.Range(-3f, 3f), SpawnPos.y + Random.Range(-3f, 3f), 0);
                ActionTime = Random.Range(-SetTime * 0.5f, SetTime * 2);
            }
            Vector2 direction = (MovePos - transform.position);
            float distance = direction.magnitude;

            if (distance < 0.1f)
            {
                ActionTime -= Time.deltaTime *0.8f;
            }
            else
            {
                Vector2 moveDir = direction.normalized;

                transform.position += (Vector3)(moveDir * monsterData.FinalSpeed * Time.deltaTime);

                SpriteRotation(moveDir);
            }
            if (ActionTime < 0)
            {
                MovePosCheck = false;
            }
        }
    }

    //타겟에게서 도망치는 함수
    private void TargetRun()
    {
        if (DeadCheck())
            return;

        if (monsterData.MonsterCurentAction == E_Monster_Current_Action.Move)
        {
            if (TargetObject != null)
            {
                Vector2 direction = (transform.position - TargetObject.position);
                float distance = direction.magnitude;
                Vector2 moveDir = direction.normalized;

                transform.position += (Vector3)(moveDir * (monsterData.FinalSpeed * 2) * Time.deltaTime);

                SpriteRotation(moveDir);

                if (distance > (monsterData.MonsterBaseData.FOVRange * 0.5f) * (0.9f + monsterData.MonsterBaseData.Intelligence))
                {
                    ActionTime -= Time.deltaTime * 0.5f;
                    SpawnPos = transform.position;
                    if (MovePosCheck)
                    {
                        MovePosCheck = false;
                    }
                }
                else
                {
                }
            }
        }
    }
    //돌진 공격 함수.
    private void ChargeAttack()
    {
        if (monsterData.MonsterCurentAction == E_Monster_Current_Action.Attack)
        {
            AttackTime -= Time.deltaTime;
            if (AttackTime > 0.5f)
            {
                Vector2 direction = (TargetObject.position - transform.position);
                Vector2 moveDir = direction.normalized;
                SpriteRotation(moveDir);
            }
            else
            {
                transform.position += (Vector3)(SightObject.right * (monsterData.FinalSpeed * monsterData.MonsterBaseData.Power) * Time.deltaTime);
                if(!monsterData.States.Contains("Charge"))
                {
                    monsterData.States.Add("Charge");
                }

            }
            if (AttackTime <= 0)
            {
                monsterData.MonsterCurentAction = E_Monster_Current_Action.Move;
                if (monsterData.States.Contains("Charge"))
                {
                    monsterData.States.Remove("Charge");
                }
            }
        }
    }

    //이 위론 Action에 넣을 함수들

    //스프라이트를 좌우 반전하는 함수
    private void SpriteRotation(Vector2 movedir)
    {
        angle = Mathf.Atan2(movedir.y, movedir.x) * Mathf.Rad2Deg;
        SightObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        if (angle < 90f && angle > -90f)
        {
            transform.GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().flipX = true;
        }
    }


    //강제로 플레이어를 인식하는 함수
    public void ForcePlayerCheck()
    {
        TargetObject = GameObject.Find("Player").transform;
        ActionTime = SetTime * 3f;
        monsterData.MonsterCurentAction = E_Monster_Current_Action.Move;
    }

    public void HitWall()
    {
        ActionTime = 0f;
    }

    //이 몬스터가 죽었는지 확인하는 함수
    private bool DeadCheck()
    {
        if (monsterData.MonsterCurentAction == E_Monster_Current_Action.Dead)
        { return true; }

        return false;
    }
    //Scene에 시야 범위가 어느 정도인지 확인용 Action에 넣지말것
    private void OnDrawGizmosSelected()
    {
        if (monsterData != null)
        {
            // 시각화용 (Scene View에서만 보임)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, monsterData.MonsterBaseData.FOVRange);

            Vector3 rightDir = Quaternion.Euler(0, 0, monsterData.MonsterBaseData.FOVAngle / 2) * SightObject.transform.right;
            Vector3 leftDir = Quaternion.Euler(0, 0, -monsterData.MonsterBaseData.FOVAngle / 2) * SightObject.transform.right;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, rightDir * monsterData.MonsterBaseData.FOVRange);
            Gizmos.DrawRay(transform.position, leftDir * monsterData.MonsterBaseData.FOVRange);
        }
    }

}
