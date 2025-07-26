using UnityEngine;

//������ �����͸� �����ϴ� �ڵ�
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
