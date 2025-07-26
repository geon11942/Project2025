using UnityEngine;

//플레이어캐릭터의 능력치 관련 코드
public class PlayerData : MonoBehaviour
{
    public PlayerBaseDataTable BaseData;

    public float FinalMaxHp;
    public float FinalMaxMana;
    public float FinalHp;
    public float FinalMana;
    public float FinalPower;
    public float FinalSpeed;
    public float FinalManaRegenSpeed;

    public string[] States;

    private void Start()
    {
        if(BaseData != null)
        {
            InitData();
        }
    }

    private void InitData()
    {
        FinalMaxHp = BaseData.MaxHp;
        FinalMaxMana= BaseData.MaxMana;
        FinalHp = FinalMaxHp;
        FinalMana = FinalMaxMana;
        FinalPower = BaseData.Power;
        FinalSpeed = BaseData.Speed;
        FinalManaRegenSpeed = BaseData.ManaRegenSpeed;
    }
}
