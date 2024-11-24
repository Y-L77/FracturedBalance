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
    private bool isDead = false;

    public GameObject DeathScreen;
    public GameObject bootLoader;

    public bool touchingChicken = false;
    public bool touchingFire = false;
    public bool touchingChickenLeg = false;

    void Start()
    {
        transform.position = new Vector3(-3, 0, 0);

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

        if (healthValue <= 0 && !isDead)
        {
            Death();
        }

        // Chicken interaction logic
        if (touchingChicken && Input.GetKeyDown(KeyCode.E))
        {
            holdingChicken = true;
            chickenObject.SetActive(true);
            pickUpChicken.Play();
        }

        if (holdingChicken && touchingFire && Input.GetKeyDown(KeyCode.E))
        {
            holdingChicken = false;
            chickenObject.SetActive(false);
            cookChicken.Play();
        }

        if (touchingChickenLeg && Input.GetKeyDown(KeyCode.E))
        {
            hungerValue = Mathf.Min(maxHunger, hungerValue + 20); // Prevent exceeding max
            eatChicken.Play();
        }
    }

    void Death()
    {
        isDead = true;

        // Reset player stats
        healthValue = maxHealth;
        thirstValue = maxThirst;
        hungerValue = maxHunger;
        smokeValue = maxSmoke;

        // Reset settings
        smokeDecreaseInterval = 5f;
        hungerHealthPenaltyRate = 2;

        // Show death screen and handle reload
        DeathScreen.SetActive(true);
        StartCoroutine(HandleDeath());
    }

    IEnumerator HandleDeath()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Show boot loader and hide death screen
        bootLoader.SetActive(true);
        DeathScreen.SetActive(false);
        isDead = false; // Allow for future deaths
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

    IEnumerator DecreaseHunger()
    {
        while (true)
        {
            yield return new WaitForSeconds(hungerDecreaseInterval);
            hungerValue = Mathf.Max(0, hungerValue - hungerDecreaseRate);
        }
    }

    IEnumerator DecreaseThirst()
    {
        while (true)
        {
            yield return new WaitForSeconds(thirstDecreaseInterval);
            thirstValue = Mathf.Max(0, thirstValue - thirstDecreaseRate);
        }
    }

    IEnumerator DecreaseSmoke()
    {
        while (true)
        {
            yield return new WaitForSeconds(smokeDecreaseInterval);
            smokeValue = Mathf.Max(0, smokeValue - smokeDecreaseRate);
        }
    }

    IEnumerator DecreaseHealthFromHunger()
    {
        while (true)
        {
            yield return new WaitForSeconds(penaltyInterval);
            healthValue = Mathf.Max(0, healthValue - hungerHealthPenaltyRate);
        }
    }

    IEnumerator DecreaseHealthFromThirst()
    {
        while (true)
        {
            yield return new WaitForSeconds(penaltyInterval);
            healthValue = Mathf.Max(0, healthValue - thirstHealthPenaltyRate);
        }
    }

    IEnumerator DecreaseHealthFromSmoke()
    {
        while (true)
        {
            yield return new WaitForSeconds(penaltyInterval);
            healthValue = Mathf.Max(0, healthValue - smokeHealthPenaltyRate);
        }
    }

    void UpdateHealthBar() => UpdateBar(healthBarFill, healthValue, maxHealth);

    void UpdateHungerBar() => UpdateBar(hungerBarFill, hungerValue, maxHunger);

    void UpdateThirstBar() => UpdateBar(thirstBarFill, thirstValue, maxThirst);

    void UpdateSmokeBar() => UpdateBar(smokeBarFill, smokeValue, maxSmoke);

    void UpdateBar(GameObject barFill, int currentValue, int maxValue)
    {
        float percentage = (float)currentValue / maxValue;
        float rightValue = Mathf.Lerp(maxRightSide, minRightSide, percentage);
        RectTransform barRect = barFill.GetComponent<RectTransform>();
        barRect.offsetMax = new Vector2(rightValue, barRect.offsetMax.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chicken"))
        {
            touchingChicken = true;
            eToInteract.SetActive(true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("chicken"))
        {
            touchingChicken = false;
            eToInteract.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
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

    void OnTriggerExit2D(Collider2D collision)
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
