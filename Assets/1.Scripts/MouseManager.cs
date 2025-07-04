using UnityEngine;

//���� ������� ����. ���߿� ���콺 ���� �߰�����
public class MouseManager : MonoBehaviour
{
    public Vector3 mousePosition;
    void Start()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, -Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                    Input.mousePosition.y, -Camera.main.transform.position.z));
        transform.position = mousePosition;
    }
}
