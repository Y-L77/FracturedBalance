using System.Collections;
using UnityEngine;

public class wellScript : MonoBehaviour
{
    public bool touchingPlayer;
    public GameObject pressEtoInteractWithWell;
    public PlayerData playerData;
    public AudioSource drinkWater;

    private bool isRefilling = false;

    private void Update()
    {
        if (touchingPlayer && Input.GetKeyDown(KeyCode.E) && !isRefilling)
        {
            StartCoroutine(RefillThirst());
        }

        if (!Input.GetKey(KeyCode.E) && isRefilling)
        {
            StopCoroutine(RefillThirst());
            isRefilling = false;
        }
    }

    private IEnumerator RefillThirst()
    {
        isRefilling = true;
        while (touchingPlayer && Input.GetKey(KeyCode.E))
        {
            playerData.thirstValue += 3; // Increase thirst value by 4
            drinkWater.Play();
            yield return new WaitForSeconds(1f); // Wait 1 second
        }
        isRefilling = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = true;
            pressEtoInteractWithWell.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer = false;
            pressEtoInteractWithWell.SetActive(false);
            isRefilling = false;
            StopCoroutine(RefillThirst());
        }
    }
}
