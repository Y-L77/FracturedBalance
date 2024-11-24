using System.Collections;
using UnityEngine;

public class ChickenScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveDistance = 5f; // Distance for each movement
    public float moveSpeed = 2f; // Speed of movement
    public GameObject player;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position; // Store the chicken's starting position
        StartCoroutine(RandomMovement());
    }


    IEnumerator RandomMovement()
    {
        while (true)
        {
            // Pick a random direction
            Vector3 direction = GetRandomDirection();

            // Calculate the target position
            Vector3 targetPosition = transform.position + direction * moveDistance;

            // Ensure the target position is within a 5-unit boundary of the starting position
            if (Vector3.Distance(startPosition, targetPosition) <= moveDistance)
            {
                // Rotate chicken if moving left or right
                if (direction == Vector3.right)
                    transform.rotation = Quaternion.Euler(0, 0, 0); // Face right
                else if (direction == Vector3.left)
                    transform.rotation = Quaternion.Euler(0, 180, 0); // Face left

                // Move to the target position smoothly
                yield return StartCoroutine(MoveToPosition(targetPosition));
            }
        }
    }

    Vector3 GetRandomDirection()
    {
        int randomDirection = Random.Range(0, 4); // Pick a random number from 0 to 3
        switch (randomDirection)
        {
            case 0: return Vector3.up;    // Move up
            case 1: return Vector3.down;  // Move down
            case 2: return Vector3.right; // Move right
            case 3: return Vector3.left;  // Move left
            default: return Vector3.zero; // Default case (shouldn't happen)
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float time = distance / moveSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Snap to the final position
    }

    private bool isPlayerColliding = false;

    void Update()
    {
        // Check if the player is colliding and the E key is pressed
        if (isPlayerColliding && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject, 0.1f); // Destroy the object
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = true; // Set flag to true when collision starts
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerColliding = false; // Reset flag when collision ends
        }
    }

}
