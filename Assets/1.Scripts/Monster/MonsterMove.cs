using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//몬스터의 이동이나 시야, 공격 등의 행동을 실행하는 코드
public class MonsterMove : MonoBehaviour
{
    private MonsterData monsterData;
    private D_Action Action = null;// 몬스터마다 지정할 행동을 담아둠
    //private D_Action FreeAction = null;// 따른 행동을 하지 않을 때 자유행동

    [SerializeField]
    private float ActionTime = 0f;

    public LayerMask DetectionLayer; //다른 몬스터나 플레이어 레이어

    [SerializeField]
    private Transform TargetObject = null;

    public List<Transform> DetectedObjects = new List<Transform>();//인식한 몹,플레이어 저장용
    [SerializeField]
    private List<Transform> MyParts = new List<Transform>();//자기 신체 오브젝트

    

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



    //이 아래론 Action에 넣을 함수들


    //자신의 시야에 들어온 오브젝트 중 플레이어나 다른 몬스터를 저장
    private void DetectObjects()
    {
        DetectedObjects.Clear();
        // 주변 모든 콜라이더 가져오기 (원 범위)
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, monsterData.MonsterBaseData.FOVRange, DetectionLayer);

        foreach (Collider2D hit in hits)
        {
            if (MyParts.Contains(hit.transform)) continue;//자신에게 포함된 오브젝트일 경우 무시

            Vector2 toTarget = (hit.transform.position - transform.position).normalized;
            Vector2 forward = transform.right;  // 오른쪽이 2D에서 forward 방향

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
                TargetPoint= Dist * monsterData.PlayerHostility * 1f; //1f는 나중에 평판 별로 정해진 수치를 받아 변경되게 바꾸기
            }
            else
            {
                //다른 몬스터에 대한 점수판정
            }
            if (TargetPoint > SelectPoint) 
            {
                SelectPoint = TargetPoint;
                TargetObject= target;
                ActionTime = 4f;
            }
        }
        
    }

    //타겟오브젝트가 있으면 해당 오브젝트 추적. 나중엔 없을 때 자유행동하게 수정
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

    //이 위론 Action에 넣을 함수들


    //Scene에 시야 범위가 어느 정도인지 확인용 Action에 넣지말것
    private void OnDrawGizmosSelected()
    {
        if (monsterData != null)
        {
            // 시각화용 (Scene View에서만 보임)
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