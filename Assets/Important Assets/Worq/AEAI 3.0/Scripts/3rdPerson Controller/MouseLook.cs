using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed = 5;

    void Start()
    {
        transform.parent = target.transform;
        transform.LookAt(target.transform);
    }

    void LateUpdate()
    {
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
//        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        target.transform.RotateAround(target.transform.position, Vector3.up, horizontal);
//        transform.RotateAround(target.transform.position, Vector3.left, vertical);
    }
}