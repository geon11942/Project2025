using System.Collections.Generic;
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

    public List<string> States;

    private void Start()
    {
        if(BaseData != null)
        {
            InitData();
        }
    }

    private void Update()
    {
        FinalSpeed = BaseData.Speed;
        if(States.Contains("Wet"))
        {
            FinalSpeed = FinalSpeed * 0.5f;
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Water"))
        {
            if(!States.Contains("Wet"))
            {
                States.Add("Wet");
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            if (States.Contains("Wet"))
            {
                States.Remove("Wet");
            }
        }
    }
}
