using UnityEngine;

public class MonsterBodyParts : MonoBehaviour
{
    //[SerializeField]
    //private string PartsTag;
    
    //�÷��̾ ���� ������ �޾ƿ�  ������� monsterdata���� �����ϴ� �Լ�
    public void HitPlayerAttack(float damage)
    {
        transform.parent.parent.GetComponent<MonsterData>()?.HitPlayerAttack(damage);
    }
}
