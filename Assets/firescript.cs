using System.Collections;
using UnityEngine;

public class firescript : MonoBehaviour
{
    public GameObject chickenObject; // The chicken object to display
    public GameObject chickenLeg;    // The chicken leg object to display after 5 seconds
    public PlayerData playerData;
    public GameObject eToInteract;

    private bool touchingPlayer;

    private void Update()
    {
        // Check for interaction
        if (touchingPlayer && Input.GetKeyDown(KeyCode.E) && playerData.holdingChicken)
        {
            Debug.Log("Player interacted with fire and is holding a chicken.");
            chickenObject.SetActive(true); // Display the chicken object
            StartCoroutine(GiveChickenLeg()); // Start coroutine to handle chicken leg
            eToInteract.SetActive(false); // Hide interaction prompt
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
            if (playerData.holdingChicken) eToInteract.SetActive(true); // Show interaction prompt
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
            eToInteract.SetActive(false); // Hide interaction prompt
        }
    }

    private IEnumerator GiveChickenLeg()
    {
        yield return new WaitForSeconds(5); // Wait for 5 seconds

        // Check if the objects are still valid before modifying them
        if (chickenObject != null)
        {
            chickenObject.SetActive(false);
        }
        if (chickenLeg != null)
        {
            chickenLeg.SetActive(true);
        }
    }
}
