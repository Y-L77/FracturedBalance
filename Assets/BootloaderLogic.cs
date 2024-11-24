using UnityEngine;
using UnityEngine.UI;

public class BootloaderLogic : MonoBehaviour
{
    public PlayerData playerData;
    public GameObject Bootloader;
    public Button NoAddiction; // Reference to the Button
    public Button FoodAddiction;
    public GameObject hungerbar;
    public GameObject veryhungerbar;
    public Button SmokeAddiction;
    public AudioSource pressButton;

    private void Start()
    {
        Bootloader.SetActive(true);
        if (NoAddiction != null)
        {
            // Add a listener to the button to call a method when clicked
            NoAddiction.onClick.AddListener(StartNoAddiction);
        }
        if(FoodAddiction != null)
        {
            FoodAddiction.onClick.AddListener(StartFoodAddiction);
        }
        if(SmokeAddiction != null)
        {
            SmokeAddiction.onClick.AddListener(StartSmokeAddiction);
        }
    }

    void StartSmokeAddiction()
    {
        playerData.healthValue = 100;
        playerData.thirstValue = 100;
        playerData.hungerValue = 100;
        playerData.smokeValue = 100;
        playerData.smokeDecreaseInterval = 2;
        veryhungerbar.SetActive(false);
        hungerbar.SetActive(true);
        playerData.hungerHealthPenaltyRate = 2;
        Bootloader.SetActive(false);
        pressButton.Play();
    }

    private void StartNoAddiction()
    {
        playerData.healthValue = 100;
        playerData.thirstValue = 100;
        playerData.hungerValue = 100;
        playerData.smokeValue = 100;
        playerData.smokeDecreaseInterval = 5;
        veryhungerbar.SetActive(false);
        hungerbar.SetActive(true);
        playerData.hungerHealthPenaltyRate = 2;
        Bootloader.SetActive(false);
        pressButton.Play();
    }
    private void StartFoodAddiction()
    {
        playerData.healthValue = 100;
        playerData.thirstValue = 100;
        playerData.maxHunger = 50;
        playerData.smokeDecreaseInterval = 5;
        playerData.smokeValue= 100;
        playerData.hungerValue = 50;
        playerData.hungerHealthPenaltyRate = 4;
        veryhungerbar.SetActive(true);
        hungerbar.SetActive(false);
        Bootloader.SetActive(false);
        pressButton.Play();
    }
}
