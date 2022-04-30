using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController mController;
    public float speed;

    void Start()
    {
        mController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Vector3 moveVector = Vector3.zero;

        moveVector.x = Input.GetAxis("Horizontal") * speed;
        moveVector.z = Input.GetAxis("Vertical") * speed;

        mController.Move(moveVector * Time.deltaTime);
    }
}