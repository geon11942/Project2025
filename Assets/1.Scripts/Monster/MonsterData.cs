using UnityEngine;

//������ �����͸� �����ϴ� �ڵ�
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
        //������� �Դ� �ڵ�
        GetComponent<MonsterMove>()?.ForcePlayerCheck();
    }
}
