using System.Collections.Generic;
using UnityEngine;

public class ObjectSearch : MonoBehaviour
{
    //상속된 오브젝트에 collider에 충돌한 오브젝트 중에서 targetTag와 같은 태그의 오브젝트를 저장하고
    //그 중에서 상속 오브젝트와 가장 가까운 오브젝트를 따로 저장. 다른 코드에 전달용.
    public string targetTag = "";
    private List<Transform> targetsInRange = new List<Transform>();
    private Transform closestTarget;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag(targetTag))
        {
            targetsInRange.Add(other.transform);
            UpdateClosestTarget();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            targetsInRange.Remove(other.transform);
            UpdateClosestTarget();
        }
    }

    private void UpdateClosestTarget()
    {
        float closestDistance = float.MaxValue;
        Transform nearest = null;

        foreach (Transform t in targetsInRange)
        {
            if (t == null) continue; // 제거되었을 경우 무시
            if (t.GetComponent<ItemObject>()!.IsState!=E_Item_State.Stay) continue; // 아이템일 때 아이템이 어딘가에 속해 있을 경우(예:플레이어가 가지고있음) 무시
            float dist = Vector2.SqrMagnitude(t.position - transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                nearest = t;
            }
        }

        closestTarget = nearest;
    }


    // 외부에서 가장 가까운 대상 접근할 수 있게 속성 제공
    public Transform GetClosestTarget()
    {
        UpdateClosestTarget();
        return closestTarget;
    }

    public void TagControl(string tag)
    {
        targetTag = tag;
    }
}
