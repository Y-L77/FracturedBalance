using System.Collections;
using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    public float spawnRate = 3f; // Time in seconds between each spawn
    public int maxChickens = 10; // Maximum number of chickens
    public GameObject chicken; // Reference to the chicken prefab

    private void Start()
    {
        // Start spawning chickens at regular intervals
        StartCoroutine(SpawnChickens());
    }

    private IEnumerator SpawnChickens()
    {
        while (true)
        {
            // Wait for the specified spawn rate
            yield return new WaitForSeconds(spawnRate);

            // Get the current count of chickens
            int chickenCount = GameObject.FindGameObjectsWithTag("chicken").Length;

            // Spawn a chicken only if the current number is less than the max allowed
            if (chickenCount < maxChickens)
            {
                Instantiate(chicken, transform.position, Quaternion.identity); // Spawn the chicken at the spawner's position
            }
        }
    }
}
