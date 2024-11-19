using UnityEngine;

public class Jump : MonoBehaviour
{
    Rigidbody myRigidbody; // Cambié el nombre de la variable
    public float jumpStrength = 2;
    public event System.Action Jumped;

    [SerializeField, Tooltip("Prevents jumping when the transform is in mid-air.")]
    GroundCheck groundCheck;

    void Reset()
    {
        // Try to get groundCheck.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Awake()
    {
        // Get rigidbody.
        myRigidbody = GetComponent<Rigidbody>(); // Actualicé la referencia aquí
    }

    void LateUpdate()
    {
        // Jump when the Jump button is pressed and we are on the ground.
        if (Input.GetButtonDown("Jump") && (!groundCheck || groundCheck.isGrounded))
        {
            myRigidbody.AddForce(Vector3.up * 100 * jumpStrength); // Actualicé la referencia aquí
            Jumped?.Invoke();
        }
    }
}
