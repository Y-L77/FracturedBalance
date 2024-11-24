using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public bool holdingChicken;
    public GameObject chickenObject;
    public GameObject eToInteract;

    [Header("UI Settings")]
    public float maxRightSide = 3.4f; // Health bar's right value when full
    public float minRightSide = 300f; // Health bar's right value when empty
    public GameObject healthBarFill;
    public GameObject hungerBarFill;
    public GameObject thirstBarFill;

    [Header("Player Stats")]
    public int healthValue;
    public int maxHealth = 100;

    public int hungerValue;
    public int maxHunger = 100;

    public int thirstValue;
    public int maxThirst = 100;

    [Header("Hunger and Thirst Settings")]
    public float hungerDecreaseInterval = 3f;
    public int hungerDecreaseRate = 3;

    public float thirstDecreaseInterval = 3f;
    public int thirstDecreaseRate = 1;

    [Header("Health Penalty Settings")]
    public float penaltyInterval = 2f;
    public int hungerHealthPenaltyRate = 2;
    public int thirstHealthPenaltyRate = 1;

    private bool isHungerAffectingHealth = false;
    private bool isThirstAffectingHealth = false;

    void Start()
    {
        // Initialize player stats
        healthValue = maxHealth;
        hungerValue = maxHunger;
        thirstValue = maxThirst;

        // Start hunger and thirst decrease coroutines
        StartCoroutine(DecreaseHunger());
        StartCoroutine(DecreaseThirst());
    }

    void Update()
    {
        UpdateHealthBar();
        UpdateHungerBar();
        UpdateThirstBar();

        CheckHungerPenalty();
        CheckThirstPenalty();

        // Trigger death if health drops to 0 or below
        if (healthValue <= 0)
        {
            Death();
        }

        if(touchingChicken && Input.GetKey(KeyCode.E))
        {
            chickenObject.SetActive(true);
            holdingChicken = true; //later when i add campfire logic under this set both false
        }

        if(holdingChicken && touchingFire)
        {
            if(holdingChicken && touchingFire && Input.GetKey(KeyCode.E))
            {
                holdingChicken = false;
                chickenObject.SetActive(false);
            }
        }
        if(touchingChickenLeg && Input.GetKey(KeyCode.E))
        {
            //play sound
            hungerValue += 20;
        }

    }

    void Death()
    {
        // Example: Stop the game and show a "Game Over" screen
        Debug.Log("Player has died!");
        Time.timeScale = 0; // Freeze the game
    }

    // Check and handle health penalties for hunger
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

    // Check and handle health penalties for thirst
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

    // Coroutine to decrease hunger periodically
    IEnumerator DecreaseHunger()
    {
        while (true)
        {
            yield return new WaitForSeconds(hungerDecreaseInterval);
            hungerValue = Mathf.Max(0, hungerValue - hungerDecreaseRate); // Prevent negative values
        }
    }

    // Coroutine to decrease thirst periodically
    IEnumerator DecreaseThirst()
    {
        while (true)
        {
            yield return new WaitForSeconds(thirstDecreaseInterval);
            thirstValue = Mathf.Max(0, thirstValue - thirstDecreaseRate); // Prevent negative values
        }
    }

    // Coroutine for decreasing health due to hunger
    IEnumerator DecreaseHealthFromHunger()
    {
        while (true)
        {
            yield return new WaitForSeconds(penaltyInterval);
            healthValue = Mathf.Max(0, healthValue - hungerHealthPenaltyRate); // Prevent negative values
        }
    }

    // Coroutine for decreasing health due to thirst
    IEnumerator DecreaseHealthFromThirst()
    {
        while (true)
        {
            yield return new WaitForSeconds(penaltyInterval);
            healthValue = Mathf.Max(0, healthValue - thirstHealthPenaltyRate); // Prevent negative values
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
