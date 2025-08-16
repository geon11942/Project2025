using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class MonsterAction : MonoBehaviour
{
    private MonsterData monsterData;
    private D_Action Action = null;// ���͸��� ������ �ൿ�� ��Ƶ�
    //private D_Action FreeAction = null;// ���� �ൿ�� ���� ���� �� �����ൿ
    private D_Action AttackAction = null;

    [SerializeField]
    private float angle;
    [SerializeField]
    private float ActionTime = 0f;
    [SerializeField]
    private float AttackTime = 0f;
    public float SetTime = 1f;

    public LayerMask DetectionLayer; //�ٸ� ���ͳ� �÷��̾� ���̾�

    [SerializeField]
    private Transform TargetObject = null;

    public List<Transform> DetectedObjects = new List<Transform>();//�ν��� ��,�÷��̾� �����
    [SerializeField]
    private List<Transform> MyParts = new List<Transform>();//�ڱ� ��ü ������Ʈ

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

    //�� �Ʒ��� Action�� ���� �Լ���


    //�ڽ��� �þ߿� ���� ������Ʈ �� �÷��̾ �ٸ� ���͸� ����
    private void DetectObjects()
    {
        if (DeadCheck())
            return;

        DetectedObjects.Clear();
        // �ֺ� ��� �ݶ��̴� �������� (�� ����)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, monsterData.MonsterBaseData.FOVRange, DetectionLayer);

        foreach (Collider2D hit in hits)
        {
            if (MyParts.Contains(hit.transform)) continue;//�ڽſ��� ���Ե� ������Ʈ�� ��� ����

            Vector2 toTarget = (hit.transform.position - transform.position).normalized;
            Vector2 forward = SightObject.transform.right;  // �������� 2D���� forward ����

            float angleToTarget = Vector2.Angle(forward, toTarget);

            if (angleToTarget <= monsterData.MonsterBaseData.FOVAngle / 2f)
            {
                // ���� ���� ��� �߰�
                DetectedObjects.Add(hit.transform);
            }
        }
    }

    //�ڽſ��� ����� �ٸ� ��ü�߿� �Ÿ��� ��ü�� ����ġ�� ���� ������ �ű�� ���� ������ ���� ����� Ÿ������ ����
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
                TargetPoint = Dist * monsterData.PlayerHostility * 1f; //1f�� ���߿� ���� ���� ������ ��ġ�� �޾� ����ǰ� �ٲٱ�
            }
            else
            {
                TargetPoint = 20f;
                //�ٸ� ���Ϳ� ���� ��������
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

    //Ÿ�ٿ�����Ʈ�� ������ �ش� ������Ʈ ����. ���߿� ���� �� �����ൿ�ϰ� ����
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

    //Ÿ�ٿ��Լ� ����ġ�� �Լ�
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
    //���� ���� �Լ�.
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

    //�� ���� Action�� ���� �Լ���

    //��������Ʈ�� �¿� �����ϴ� �Լ�
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


    //������ �÷��̾ �ν��ϴ� �Լ�
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

    //�� ���Ͱ� �׾����� Ȯ���ϴ� �Լ�
    private bool DeadCheck()
    {
        if (monsterData.MonsterCurentAction == E_Monster_Current_Action.Dead)
        { return true; }

        return false;
    }
    //Scene�� �þ� ������ ��� �������� Ȯ�ο� Action�� ��������
    private void OnDrawGizmosSelected()
    {
        if (monsterData != null)
        {
            // �ð�ȭ�� (Scene View������ ����)
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
