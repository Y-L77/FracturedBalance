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

    private bool isHungerAffectingHealth = false;
    private bool isThirstAffectingHealth = false;
    private bool isSmokeAffectingHealth = false;

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

        CheckHungerPenalty();
        CheckThirstPenalty();
        CheckSmokePenalty();

        if (healthValue <= 0)
        {
            Death();
        }

        if (touchingChicken && Input.GetKey(KeyCode.E))
        {
            chickenObject.SetActive(true);
            pickUpChicken.Play();
            holdingChicken = true; // later when I add campfire logic, set both false
        }

        if (holdingChicken && touchingFire)
        {
            if (Input.GetKey(KeyCode.E))
            {
                holdingChicken = false;
                chickenObject.SetActive(false);
                cookChicken.Play();
            }
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

    // Check health penalties for hunger
    void CheckHungerPenalty()
    {
        if (hungerValue <= 0 && !isHungerAffectingHealth)
        {
            StartCoroutine(DecreaseHealthFromHunger());
            isHungerAffectingHealth = true;
        }
        else if (hungerValue > 0 && isHungerAffectingHealth)
        {
            StopCoroutine(DecreaseHealthFromHunger());
            isHungerAffectingHealth = false;
        }
    }

    // Check health penalties for thirst
    void CheckThirstPenalty()
    {
        if (thirstValue <= 0 && !isThirstAffectingHealth)
        {
            StartCoroutine(DecreaseHealthFromThirst());
            isThirstAffectingHealth = true;
        }
        else if (thirstValue > 0 && isThirstAffectingHealth)
        {
            StopCoroutine(DecreaseHealthFromThirst());
            isThirstAffectingHealth = false;
        }
    }

    // Check health penalties for smoke
    void CheckSmokePenalty()
    {
        if (smokeValue <= 0 && !isSmokeAffectingHealth)
        {
            StartCoroutine(DecreaseHealthFromSmoke());
            isSmokeAffectingHealth = true;
        }
        else if (smokeValue > 0 && isSmokeAffectingHealth)
        {
            StopCoroutine(DecreaseHealthFromSmoke());
            isSmokeAffectingHealth = false;
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
        float healthPercentage = (float)healthValue / maxHealth;
        float rightValue = Mathf.Lerp(maxRightSide, minRightSide, healthPercentage);
        RectTransform healthBarRect = healthBarFill.GetComponent<RectTransform>();
        healthBarRect.offsetMax = new Vector2(rightValue, healthBarRect.offsetMax.y);
    }

    // Update the hunger bar UI
    void UpdateHungerBar()
    {
        float hungerPercentage = (float)hungerValue / maxHunger;
        float rightValue = Mathf.Lerp(maxRightSide, minRightSide, hungerPercentage);
        RectTransform hungerBarRect = hungerBarFill.GetComponent<RectTransform>();
        hungerBarRect.offsetMax = new Vector2(rightValue, hungerBarRect.offsetMax.y);
    }

    // Update the thirst bar UI
    void UpdateThirstBar()
    {
        float thirstPercentage = (float)thirstValue / maxThirst;
        float rightValue = Mathf.Lerp(maxRightSide, minRightSide, thirstPercentage);
        RectTransform thirstBarRect = thirstBarFill.GetComponent<RectTransform>();
        thirstBarRect.offsetMax = new Vector2(rightValue, thirstBarRect.offsetMax.y);
    }

    // Update the smoke bar UI
    void UpdateSmokeBar()
    {
        float smokePercentage = (float)smokeValue / maxSmoke;
        float rightValue = Mathf.Lerp(maxRightSide, minRightSide, smokePercentage);
        RectTransform smokeBarRect = smokeBarFill.GetComponent<RectTransform>();
        smokeBarRect.offsetMax = new Vector2(rightValue, smokeBarRect.offsetMax.y);
    }

    public bool touchingChicken = false;

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

    public bool touchingFire = false;
    public bool touchingChickenLeg = false;

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
