using UnityEngine;

public class CameraMove : MonoBehaviour
{
    void Start()
    {
        Vector3 pos = new Vector3(Target.position.x, Target.position.y,transform.position.z);
        transform.position = pos;
        m_TargetOffset = Target.position - transform.position;
    }

    public Transform Target;

    [Range(0f, 1f)]
    public float LerpVal = 0.2f; // 0~1f

    protected Vector3 m_TargetOffset;

    // Update is called once per frame
    void LateUpdate()
    {

        Vector3 temppos = Vector3.Lerp(transform.position
                            , Target.position - m_TargetOffset
                            , LerpVal);
        transform.position = temppos;
    }
}
