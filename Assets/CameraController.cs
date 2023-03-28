using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotateSpeed = 5.0f;

    void Update()
    {
        //float horizontal = Input.GetAxis("Horizontal") * rotateSpeed;
        //float vertical = Input.GetAxis("Vertical") * moveSpeed;

       // transform.Translate(horizontal, 0, vertical);

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        }
    }
}
