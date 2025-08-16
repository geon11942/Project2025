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

    //플레이어에 의한 공격을 받아와  대미지를 monsterdata에게 전달하는 함수
    public void HitPlayerAttack(float damage)
    {
        transform.parent.parent.GetComponent<MonsterData>()?.HitPlayerAttack(damage);
    }
}
