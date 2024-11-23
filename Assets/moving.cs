using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public GameObject side;
    public GameObject front;
    public GameObject back;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Optional Settings")]
    public bool allowDiagonalMovement = true; // Toggle for precise grid-style movement

    private Rigidbody2D rb;
    private Vector2 movement;

    // Variable to store the last active view
    private GameObject lastActiveView;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastActiveView = front; // Default to front if nothing has been activated yet
    }

    void Update()
    {
        // Get input from WASD or arrow keys
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Handle diagonal movement option
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

        // Activate the correct view based on input
        HandleCharacterView(moveX, moveY);
    }

    void FixedUpdate()
    {
        // Apply movement to the Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }

    void HandleCharacterView(float moveX, float moveY)
    {
        // If there's no movement, keep the last active view
        if (moveX == 0 && moveY == 0)
        {
            // Do nothing, keep the last active view
            return;
        }

        if (moveY > 0) // Moving up (W)
        {
            SetActiveView(front);
        }
        else if (moveY < 0) // Moving down (S)
        {
            SetActiveView(back);
        }
        else if (moveX != 0) // Moving sideways (A or D)
        {
            SetActiveView(side);

            // Rotate the side view based on whether moving left (A) or right (D)
            if (moveX > 0) // Moving right (D)
            {
                side.transform.rotation = Quaternion.Euler(0, 0, 0); // No rotation (facing right)
            }
            else if (moveX < 0) // Moving left (A)
            {
                side.transform.rotation = Quaternion.Euler(0, 180, 0); // Rotate 180 degrees (facing left)
            }
        }
    }

    void SetActiveView(GameObject view)
    {
        // Deactivate all views
        front.SetActive(false);
        back.SetActive(false);
        side.SetActive(false);

        // Activate the selected view and update the last active view
        view.SetActive(true);
        lastActiveView = view;
    }
}
