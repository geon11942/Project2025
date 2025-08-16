using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

//������ �����͸� �����ϴ� �ڵ�
public class MonsterData : MonoBehaviour
{
    public MonsterBaseDataTable MonsterBaseData;

    public float FinalSpeed;

    [Range(-1,1)]
    public float Mood;
    public float Sick;

    public float PlayerHostility;

    public E_Monster_Current_Action MonsterCurentAction;

    public List<string> States;

   // [SerializeField]
    //private 
    private void Start()
    {
        PlayerHostility = MonsterBaseData.MinPlayerHostility;
    }

    private void Update()
    {
        FinalSpeed = MonsterBaseData.Speed;
        if (States.Contains("Wet"))
        {
            FinalSpeed = FinalSpeed * 0.5f;
            transform.GetComponent<SpriteRenderer>().color = Color.cyan;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void HitPlayerAttack(float damage)
    {
        //������� �Դ� �ڵ�
        GetComponent<MonsterMove>()?.ForcePlayerCheck();
        GetComponent<MonsterAction>()?.ForcePlayerCheck();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            if (!States.Contains("Wet"))
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
