using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KinematicCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float groundCheckDistance = 0.1f;

    private Rigidbody rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Handle player input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Move the character horizontally
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed;
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Jumping
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Check if the character is grounded
        isGrounded = CheckGrounded();
    }

    private bool CheckGrounded()
    {
        // Perform a raycast downward to check if the character is on the ground
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }
}
