using System.Collections;
using UnityEngine;

public class firescript : MonoBehaviour
{
    public GameObject chickenObject; // The chicken object to display
    public GameObject chickenLeg;    // The chicken leg object to display after 3 seconds
    public PlayerData playerData;
    public GameObject eToInteract;

    public bool touchingPlayer;


    private void Update()
    {
        if (touchingPlayer && Input.GetKeyDown(KeyCode.E) && playerData.holdingChicken)
        {
            Debug.Log("player touched and hit e and has a chicken");
            chickenObject.SetActive(true); // Display the chicken object
            StartCoroutine(GiveChickenLeg()); // Start coroutine to give chicken leg


            if(playerData.holdingChicken && touchingPlayer)
            {
                eToInteract.SetActive(true);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;

        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
        }
    }
    IEnumerator GiveChickenLeg()
    {
        yield return new WaitForSeconds(5); // Wait for 3 seconds

        // Switch the chicken object off and enable the chicken leg
        chickenObject.SetActive(false);
        chickenLeg.SetActive(true);
    }
}
