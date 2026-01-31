using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // El prefab del enemigo a generar
    public GameObject[] spawnPoints; // Array de GameObjects que representan los puntos de spawn
    public int numberOfEnemies = 3; // Número total de enemigos a generar

    private void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("¡El prefab del enemigo no está asignado!");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("¡No hay puntos de spawn asignados!");
            return;
        }

        // Convertir el array de spawnPoints a una lista para manipularlo fácilmente
        List<GameObject> availableSpawnPoints = new List<GameObject>(spawnPoints);

        // Asegurarse de no generar más enemigos que puntos de spawn disponibles
        int enemiesToSpawn = Mathf.Min(numberOfEnemies, availableSpawnPoints.Count);

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            // Seleccionar un punto de spawn aleatorio
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            GameObject spawnPoint = availableSpawnPoints[randomIndex];

            // Generar el enemigo en el punto de spawn seleccionado
            Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);

            // Eliminar el punto de spawn de la lista para que no se use de nuevo
            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }
}