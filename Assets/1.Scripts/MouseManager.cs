using UnityEngine;

//현재 사용하지 않음. 나중에 마우스 동작 추가예정
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
