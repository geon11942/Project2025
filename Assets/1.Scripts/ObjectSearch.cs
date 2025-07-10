using System.Collections.Generic;
using UnityEngine;

public class ObjectSearch : MonoBehaviour
{
    //��ӵ� ������Ʈ�� collider�� �浹�� ������Ʈ �߿��� targetTag�� ���� �±��� ������Ʈ�� �����ϰ�
    //�� �߿��� ��� ������Ʈ�� ���� ����� ������Ʈ�� ���� ����. �ٸ� �ڵ忡 ���޿�.
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
            if (t == null) continue; // ���ŵǾ��� ��� ����
            if (t.GetComponent<ItemObject>()!.IsState!=E_Item_State.Stay) continue; // �������� �� �������� ��򰡿� ���� ���� ���(��:�÷��̾ ����������) ����
            float dist = Vector2.SqrMagnitude(t.position - transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                nearest = t;
            }
        }

        closestTarget = nearest;
    }


    // �ܺο��� ���� ����� ��� ������ �� �ְ� �Ӽ� ����
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
