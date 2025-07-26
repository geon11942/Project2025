using UnityEngine;

//몬스터의 데이터를 저장하는 코드
public class MonsterData : MonoBehaviour
{
    public MonsterBaseDataTable MonsterBaseData;

    [Range(-1,1)]
    public float Mood;
    public float Sick;

    public float PlayerHostility;

    private void Start()
    {
        PlayerHostility = MonsterBaseData.MinPlayerHostility;
    }
}
