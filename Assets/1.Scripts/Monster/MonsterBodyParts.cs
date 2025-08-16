using UnityEngine;

public class MonsterBodyParts : MonoBehaviour
{
    //[SerializeField]
    //private string PartsTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Wall"))
        {
            transform.parent.parent.GetComponent<MonsterMove>()?.HitWall();
        }
    }

    //�÷��̾ ���� ������ �޾ƿ�  ������� monsterdata���� �����ϴ� �Լ�
    public void HitPlayerAttack(float damage)
    {
        transform.parent.parent.GetComponent<MonsterData>()?.HitPlayerAttack(damage);
    }
}
