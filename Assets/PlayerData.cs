using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public float maxRightSide = 3.4f; // Health bar's right value when full
    public float minRightSide = 300f; // Health bar's right value when empty

    public GameObject healthBarFill;
    public GameObject hungerBarFill;
    public GameObject thirstBarFill;


    public int healthValue;
    public int maxHealth = 100;

    public int hungerValue;
    public int maxHunger = 100;

    public int thirstValue;
    public int maxThirst = 100;

    void Start()
    {
        healthValue = maxHealth; // Initialize health
        hungerValue = maxHunger;
        thirstValue = maxThirst;

        StartCoroutine(DecreaseHunger());
        StartCoroutine(DecreaseThirst());



    }

    void Update()
    {
        UpdateHealthBar();
        updateHungerBar();
        updateThirstBar();

        if (healthValue >= 0 || hungerValue >= 0 || thirstValue >= 0)
        {
            death();
        }
    }

    void death()
    {

    }
    // Method to update the health bar UI
    void UpdateHealthBar()
    {
        // Calculate the health percentage
        float healthPercentage = (float)healthValue / maxHealth;

        // Map the health percentage to the right value (reverse logic)
        float rightValue = Mathf.Lerp(maxRightSide, minRightSide, healthPercentage);

        // Update the health bar's right position
        RectTransform healthBarRect = healthBarFill.GetComponent<RectTransform>();
        healthBarRect.offsetMax = new Vector2(rightValue, healthBarRect.offsetMax.y);
    }
    void updateHungerBar()
    {
        float hungerPercentage = (float)hungerValue / maxHunger;
        float rightValue = Mathf.Lerp(maxRightSide, minRightSide, hungerPercentage);
        RectTransform hungerBarRect = hungerBarFill.GetComponent<RectTransform>();
        hungerBarRect.offsetMax = new Vector2(rightValue, hungerBarRect.offsetMax.y);

    }

    void updateThirstBar()
    {
        float ThirstPercentage = (float)thirstValue / maxThirst;
        float rightValue = Mathf.Lerp(maxRightSide, minRightSide, ThirstPercentage);
        RectTransform thirstBarRect = thirstBarFill.GetComponent<RectTransform>();
        thirstBarRect.offsetMax = new Vector2(rightValue, thirstBarRect.offsetMax.y);
    }


    public float decreaseInterval = 3f;
    public int hungerDecreaseRate = 3;
    IEnumerator DecreaseHunger()
    {
        while(true) // Ensure hunger doesn't go below 0
        {
            yield return new WaitForSeconds(decreaseInterval); // Wait for the set interval
            hungerValue -= hungerDecreaseRate; // Decrease hunger
        }
    }



    public float decreaseIntervalThirst = 3f;
    public int thirstDecreaseRate = 1;
    IEnumerator DecreaseThirst()
    {
        while (true) // Ensure hunger doesn't go below 0
        {
            yield return new WaitForSeconds(decreaseIntervalThirst); // Wait for the set interval
            thirstValue -= thirstDecreaseRate;
        }
    }
}
