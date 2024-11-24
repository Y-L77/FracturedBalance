using System.Collections;
using UnityEngine;

public class playersmoke : MonoBehaviour
{
    public GameObject EtoSmoke;
    public bool isTouchingPlayer;
    public GameObject cigarette;
    public PlayerData playerData;
    public AudioSource smoking;

    private void Update()
    {
        if (isTouchingPlayer && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(SmokeCigarette()); // Fixed method invocation and syntax
            smoking.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = true;
            cigarette.SetActive(true);
            EtoSmoke.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isTouchingPlayer = false;
            EtoSmoke.SetActive(false);
            cigarette.SetActive(false) ;
        }
    }

    public IEnumerator SmokeCigarette()
    {
        if (playerData.smokeValue < playerData.maxSmoke)
        {
            yield return new WaitForSeconds(3); // Wait for 3 seconds
            playerData.smokeValue = Mathf.Min(playerData.smokeValue + 80, playerData.maxSmoke); // Prevent exceeding maxSmoke
        }
    }
}
