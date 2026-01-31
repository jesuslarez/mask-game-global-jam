using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public GameObject spawnPointsParent; // The parent GameObject containing spawn points as children
    public int numberOfEnemies = 3; // Total number of enemies to spawn

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Enemy prefab is not assigned!");
            return; 
        }

        if (spawnPointsParent == null)
        {
            Debug.LogError("Spawn points parent is not assigned!");
            return;
        }
        // Get all child GameObjects of the spawnPointsParent
        Transform[] spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>();

        // Convert the array of Transforms to a list of GameObjects, excluding the parent itself
        List<GameObject> availableSpawnPoints = new List<GameObject>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint != spawnPointsParent.transform) // Exclude the parent itself
            {
                availableSpawnPoints.Add(spawnPoint.gameObject);
            }
        }

        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogError("No spawn points found under the parent GameObject!");
            return;
        }

        // Ensure we don't spawn more enemies than available spawn points
        int enemiesToSpawn = Mathf.Min(numberOfEnemies, availableSpawnPoints.Count);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Select a random spawn point
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            GameObject spawnPoint = availableSpawnPoints[randomIndex];

            // Spawn the enemy at the selected spawn point
            Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);

            // Remove the spawn point from the list to prevent reuse
            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }
}