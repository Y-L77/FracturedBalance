using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eatChicken : MonoBehaviour
{

    public bool touchingPlayer1;
    public GameObject eToInteractChicken;

    private void Update()
    {
        if(touchingPlayer1 && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer1 = true;
            eToInteractChicken.SetActive(true);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchingPlayer1 = false;
            eToInteractChicken.SetActive(false);
        }
    }
}
