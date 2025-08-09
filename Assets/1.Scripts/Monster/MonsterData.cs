using UnityEngine;

//몬스터의 데이터를 저장하는 코드
public class MonsterData : MonoBehaviour
{
    public MonsterBaseDataTable MonsterBaseData;

    [Range(-1,1)]
    public float Mood;
    public float Sick;

    public float PlayerHostility;

    public E_Monster_Current_Action MonsterCurentAction;

   // [SerializeField]
    //private 
    private void Start()
    {
        PlayerHostility = MonsterBaseData.MinPlayerHostility;
    }

    public void HitPlayerAttack(float damage)
    {
        //대미지를 입는 코드
        GetComponent<MonsterMove>()?.ForcePlayerCheck();
    }
}
