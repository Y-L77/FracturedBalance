using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public bool holdingChicken;
    public GameObject chickenObject;
    public GameObject eToInteract;
    public AudioSource pickUpChicken;
    public AudioSource cookChicken;
    public AudioSource eatChicken;

    [Header("UI Settings")]
    public float maxRightSide = 3.4f; // Health bar's right value when full
    public float minRightSide = 300f; // Health bar's right value when empty
    public GameObject healthBarFill;
    public GameObject hungerBarFill;
    public GameObject thirstBarFill;
    public GameObject smokeBarFill;

    [Header("Player Stats")]
    public int healthValue;
    public int maxHealth = 100;

    public int hungerValue;
    public int maxHunger = 100;

    public int thirstValue;
    public int maxThirst = 100;

    public int smokeValue;
    public int maxSmoke = 100;

    [Header("Hunger and Thirst Settings")]
    public float hungerDecreaseInterval = 3f;
    public int hungerDecreaseRate = 3;

    public float thirstDecreaseInterval = 3f;
    public int thirstDecreaseRate = 1;

    [Header("Smoke Settings")]
    public float smokeDecreaseInterval = 4f;
    public int smokeDecreaseRate = 2;

    [Header("Health Penalty Settings")]
    public float penaltyInterval = 2f;
    public int hungerHealthPenaltyRate = 2;
    public int thirstHealthPenaltyRate = 1;
    public int smokeHealthPenaltyRate = 3;

    private Coroutine hungerPenaltyCoroutine;
    private Coroutine thirstPenaltyCoroutine;
    private Coroutine smokePenaltyCoroutine;

    void Start()
    {
        gameObject.transform.position = new Vector3(-3, 0, 0);

        // Initialize player stats
        healthValue = maxHealth;
        hungerValue = maxHunger;
        thirstValue = maxThirst;
        smokeValue = maxSmoke;

        // Start coroutines
        StartCoroutine(DecreaseHunger());
        StartCoroutine(DecreaseThirst());
        StartCoroutine(DecreaseSmoke());
    }

    void Update()
    {
        UpdateHealthBar();
        UpdateHungerBar();
        UpdateThirstBar();
        UpdateSmokeBar();

        CheckPenalties();

        if (healthValue <= 0)
        {
            Death();
        }

        // Chicken interaction logic
        if (touchingChicken && Input.GetKey(KeyCode.E))
        {
            chickenObject.SetActive(true);
<<<<<<< HEAD
            holdingChicken = true; // Set to false when used in future logic
=======
            pickUpChicken.Play();
            holdingChicken = true; // later when I add campfire logic, set both false
>>>>>>> soundeffects
        }

        if (holdingChicken && touchingFire && Input.GetKey(KeyCode.E))
        {
<<<<<<< HEAD
            holdingChicken = false;
            chickenObject.SetActive(false);
=======
            if (Input.GetKey(KeyCode.E))
            {
                holdingChicken = false;
                chickenObject.SetActive(false);
                cookChicken.Play();
            }
>>>>>>> soundeffects
        }

        if (touchingChickenLeg && Input.GetKey(KeyCode.E))
        {
            hungerValue = Mathf.Min(maxHunger, hungerValue + 20); // Prevent exceeding max
            eatChicken.Play() ;
        }
    }

    void Death()
    {
        Debug.Log("Player has died!");
        Time.timeScale = 0; // Freeze the game
    }

    void CheckPenalties()
    {
        // Hunger
        if (hungerValue <= 0 && hungerPenaltyCoroutine == null)
        {
            hungerPenaltyCoroutine = StartCoroutine(DecreaseHealthFromHunger());
        }
        else if (hungerValue > 0 && hungerPenaltyCoroutine != null)
        {
            StopCoroutine(hungerPenaltyCoroutine);
            hungerPenaltyCoroutine = null;
        }

        // Thirst
        if (thirstValue <= 0 && thirstPenaltyCoroutine == null)
        {
            thirstPenaltyCoroutine = StartCoroutine(DecreaseHealthFromThirst());
        }
        else if (thirstValue > 0 && thirstPenaltyCoroutine != null)
        {
            StopCoroutine(thirstPenaltyCoroutine);
            thirstPenaltyCoroutine = null;
        }

        // Smoke
        if (smokeValue <= 0 && smokePenaltyCoroutine == null)
        {
            smokePenaltyCoroutine = StartCoroutine(DecreaseHealthFromSmoke());
        }
        else if (smokeValue > 0 && smokePenaltyCoroutine != null)
        {
            StopCoroutine(smokePenaltyCoroutine);
            smokePenaltyCoroutine = null;
        }
    }

    // Hunger coroutine
    IEnumerator DecreaseHunger()
    {
        while (true)
        {
            yield return new WaitForSeconds(hungerDecreaseInterval);
            hungerValue = Mathf.Max(0, hungerValue - hungerDecreaseRate);
        }
    }

    // Thirst coroutine
    IEnumerator DecreaseThirst()
    {
        while (true)
        {
            yield return new WaitForSeconds(thirstDecreaseInterval);
            thirstValue = Mathf.Max(0, thirstValue - thirstDecreaseRate);
        }
    }

    // Smoke coroutine
    IEnumerator DecreaseSmoke()
    {
        while (true)
        {
            yield return new WaitForSeconds(smokeDecreaseInterval);
            smokeValue = Mathf.Max(0, smokeValue - smokeDecreaseRate);
        }
    }

    // Health penalty due to hunger
    IEnumerator DecreaseHealthFromHunger()
    {
        while (true)
        {
            yield return new WaitForSeconds(penaltyInterval);
            healthValue = Mathf.Max(0, healthValue - hungerHealthPenaltyRate);
        }
    }

    // Health penalty due to thirst
    IEnumerator DecreaseHealthFromThirst()
    {
        while (true)
        {
            yield return new WaitForSeconds(penaltyInterval);
            healthValue = Mathf.Max(0, healthValue - thirstHealthPenaltyRate);
        }
    }

    // Health penalty due to smoke
    IEnumerator DecreaseHealthFromSmoke()
    {
        while (true)
        {
            yield return new WaitForSeconds(penaltyInterval);
            healthValue = Mathf.Max(0, healthValue - smokeHealthPenaltyRate);
        }
    }

    // Update the health bar UI
    void UpdateHealthBar()
    {
        UpdateBar(healthBarFill, healthValue, maxHealth);
    }

    // Update the hunger bar UI
    void UpdateHungerBar()
    {
        UpdateBar(hungerBarFill, hungerValue, maxHunger);
    }

    // Update the thirst bar UI
    void UpdateThirstBar()
    {
        UpdateBar(thirstBarFill, thirstValue, maxThirst);
    }

    // Update the smoke bar UI
    void UpdateSmokeBar()
    {
        UpdateBar(smokeBarFill, smokeValue, maxSmoke);
    }

    void UpdateBar(GameObject barFill, int currentValue, int maxValue)
    {
        float percentage = (float)currentValue / maxValue;
        float rightValue = Mathf.Lerp(maxRightSide, minRightSide, percentage);
        RectTransform barRect = barFill.GetComponent<RectTransform>();
        barRect.offsetMax = new Vector2(rightValue, barRect.offsetMax.y);
    }

    public bool touchingChicken = false;
    public bool touchingFire = false;
    public bool touchingChickenLeg = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chicken"))
        {
            touchingChicken = true;
            eToInteract.SetActive(true);
        }
    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chicken"))
        {
            touchingChicken = false;
            eToInteract.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fire"))
        {
            touchingFire = true;
            eToInteract.SetActive(holdingChicken);
        }

        if (collision.gameObject.CompareTag("chickenleg"))
        {
            touchingChickenLeg = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("fire"))
        {
            touchingFire = false;
            eToInteract.SetActive(false);
        }

        if (collision.gameObject.CompareTag("chickenleg"))
        {
            touchingChickenLeg = false;
        }
    }
}
