using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Optional Settings")]
    public bool allowDiagonalMovement = true; // Toggle for precise grid-style movement

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (!allowDiagonalMovement)
        {
            // Prioritize single-axis movement for grid-like feel
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
                moveY = 0;
            else
                moveX = 0;
        }

        // Normalize movement vector to maintain consistent speed
        movement = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
}
