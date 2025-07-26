using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//������ �̵��̳� �þ�, ���� ���� �ൿ�� �����ϴ� �ڵ�
public class MonsterMove : MonoBehaviour
{
    private MonsterData monsterData;
    private D_Action Action = null;// ���͸��� ������ �ൿ�� ��Ƶ�
    //private D_Action FreeAction = null;// ���� �ൿ�� ���� ���� �� �����ൿ

    [SerializeField]
    private float ActionTime = 0f;

    public LayerMask DetectionLayer; //�ٸ� ���ͳ� �÷��̾� ���̾�

    [SerializeField]
    private Transform TargetObject = null;

    public List<Transform> DetectedObjects = new List<Transform>();//�ν��� ��,�÷��̾� �����
    [SerializeField]
    private List<Transform> MyParts = new List<Transform>();//�ڱ� ��ü ������Ʈ

    

    private void Start()
    {
        monsterData = gameObject.GetComponent<MonsterData>();
        for(int i = 0; i < transform.GetChild(0).transform.childCount; i++)
        {
            MyParts.Add(transform.GetChild(0).transform.GetChild(i));
        }

        Action += DetectObjects;
        Action += TargetSelect;
        Action += TargetMove;
    }
    private void Update()
    {
        if (Action != null)
        {
            Action();
        }
    }

    private void ActionSetting()
    {
        switch (monsterData.MonsterBaseData.Name)
        {
            default:
                break;
        }
    }



    //�� �Ʒ��� Action�� ���� �Լ���


    //�ڽ��� �þ߿� ���� ������Ʈ �� �÷��̾ �ٸ� ���͸� ����
    private void DetectObjects()
    {
        DetectedObjects.Clear();
        // �ֺ� ��� �ݶ��̴� �������� (�� ����)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, monsterData.MonsterBaseData.FOVRange, DetectionLayer);

        foreach (Collider2D hit in hits)
        {
            if (MyParts.Contains(hit.transform)) continue;//�ڽſ��� ���Ե� ������Ʈ�� ��� ����

            Vector2 toTarget = (hit.transform.position - transform.position).normalized;
            Vector2 forward = transform.right;  // �������� 2D���� forward ����

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
        if(ActionTime<=0f)
        {
            TargetObject = null;
        }
        if(DetectedObjects.Count == 0) return;

        float Dist;

        float SelectPoint = 0f;
        float TargetPoint = 0f;

        foreach (var target in DetectedObjects)
        {
            TargetPoint = 0f;
            Dist =Vector2.Distance(transform.position, target.position);
            if(target.tag=="Player")
            {
                TargetPoint= Dist * monsterData.PlayerHostility * 1f; //1f�� ���߿� ���� ���� ������ ��ġ�� �޾� ����ǰ� �ٲٱ�
            }
            else
            {
                //�ٸ� ���Ϳ� ���� ��������
            }
            if (TargetPoint > SelectPoint) 
            {
                SelectPoint = TargetPoint;
                TargetObject= target;
                ActionTime = 4f;
            }
        }
        
    }

    //Ÿ�ٿ�����Ʈ�� ������ �ش� ������Ʈ ����. ���߿� ���� �� �����ൿ�ϰ� ����
    private void TargetMove()
    {
        if(TargetObject != null)
        {
            Vector2 direction = (TargetObject.position - transform.position);
            float distance = direction.magnitude;

            if (distance > 0.5f)
            {
                Vector2 moveDir = direction.normalized;

                transform.position += (Vector3)(moveDir * monsterData.MonsterBaseData.Speed * Time.deltaTime);

            }
            ActionTime-= Time.deltaTime;
        }
        else
        {

        }
    }

    //�� ���� Action�� ���� �Լ���


    //Scene�� �þ� ������ ��� �������� Ȯ�ο� Action�� ��������
    private void OnDrawGizmosSelected()
    {
        if (monsterData != null)
        {
            // �ð�ȭ�� (Scene View������ ����)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, monsterData.MonsterBaseData.FOVRange);

            Vector3 rightDir = Quaternion.Euler(0, 0, monsterData.MonsterBaseData.FOVAngle / 2) * transform.right;
            Vector3 leftDir = Quaternion.Euler(0, 0, -monsterData.MonsterBaseData.FOVAngle / 2) * transform.right;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, rightDir * monsterData.MonsterBaseData.FOVRange);
            Gizmos.DrawRay(transform.position, leftDir * monsterData.MonsterBaseData.FOVRange);
        }
    }

}