using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public string nextSceneName; // name of the scene to load when player collides with another object
    public float moveSpeed; // speed at which the player moves
    public float rotateSpeed; // speed at which the player rotates
    public Transform cameraTransform; // reference to the camera transform
    public float cameraDistance; // distance between the camera and the player
    public float cameraSensitivity; // sensitivity of the mouse for camera control

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float cameraAngleX; // current angle of the camera around the x-axis

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform.position = transform.position - transform.forward * cameraDistance; // set the initial camera position
        cameraTransform.LookAt(transform.position); // make the camera look at the player
    }

    private void FixedUpdate()
    {
        // Get input from WASD keys
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calculate move direction based on input and camera orientation
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();
        moveDirection = forward * vertical + right * horizontal;

        // Move the player based on move direction and move speed
        rb.velocity = moveDirection.normalized * moveSpeed;

        // Rotate the player to face the move direction
        if (moveDirection.magnitude > 0.50f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);
        }

        // Control the camera with the mouse

        //float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity;
        //float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity;
        //cameraAngleX -= mouseY;
        //cameraAngleX = Mathf.Clamp(cameraAngleX, -90f, 90f);
        //cameraTransform.rotation = Quaternion.Euler(cameraAngleX, transform.eulerAngles.y + mouseX, 0f);

        // Move the camera with the player
        cameraTransform.position = transform.position - cameraTransform.forward * cameraDistance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle1")) // check if collided object has "Obstacle" tag
        {
            SceneManager.LoadScene("Level2"); // load the next scene
        }
        if (other.gameObject.CompareTag("Obstacle2")) // check if collided object has "Obstacle" tag
        {
            SceneManager.LoadScene("Level3"); // load the next scene
        }
        if (other.gameObject.CompareTag("Obstacle3")) // check if collided object has "Obstacle" tag
        {
            SceneManager.LoadScene("Level4"); // load the next scene
        }
        if (other.gameObject.CompareTag("Obstacle4")) // check if collided object has "Obstacle" tag
        {
            SceneManager.LoadScene("Level5"); // load the next scene
        }
      
    }
}

