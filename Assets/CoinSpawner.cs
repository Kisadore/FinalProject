using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    [Header("Coin Prefab")]
    public GameObject coinPrefab;
    
    [Header("Spawn Locations")]
    public Transform[] easyModeSpawnPoints; // Ship spawn points
    public Transform[] hardModeSpawnPoints; // Underwater spawn points
    
    [Header("Spawn Settings")]
    public bool useTransformPositions = true; // Use Transform positions or manual positions
    public Vector3[] easyModePositions; // Manual positions for easy mode
    public Vector3[] hardModePositions; // Manual positions for hard mode
    
    private List<GameObject> spawnedCoins = new List<GameObject>();
    
    void Start()
    {
        // Wait a bit for GameManager to be ready
        StartCoroutine(SpawnCoinsWithDelay());
    }
    
    IEnumerator SpawnCoinsWithDelay()
    {
        // Wait for GameManager to be initialized
        yield return new WaitForSeconds(0.5f);
        
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found! Cannot spawn coins.");
            yield break;
        }
        
        SpawnCoins();
    }
    
    public void SpawnCoins()
    {
        // Clear any existing coins
        ClearCoins();
        
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }
        
        // Determine which spawn points to use based on difficulty
        if (GameManager.Instance.currentDifficulty == GameManager.GameDifficulty.Easy)
        {
            SpawnCoinsAtLocations(easyModeSpawnPoints, easyModePositions);
        }
        else
        {
            SpawnCoinsAtLocations(hardModeSpawnPoints, hardModePositions);
        }
        
        Debug.Log($"Spawned {spawnedCoins.Count} coins for {GameManager.Instance.currentDifficulty} mode");
    }
    
    void SpawnCoinsAtLocations(Transform[] spawnPoints, Vector3[] positions)
    {
        if (coinPrefab == null)
        {
            Debug.LogError("Coin prefab not assigned!");
            return;
        }
        
        // Use transform positions if available and enabled
        if (useTransformPositions && spawnPoints != null && spawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in spawnPoints)
            {
                if (spawnPoint != null)
                {
                    GameObject coin = Instantiate(coinPrefab, spawnPoint.position, spawnPoint.rotation);
                    spawnedCoins.Add(coin);
                }
            }
        }
        // Otherwise use manual positions
        else if (positions != null && positions.Length > 0)
        {
            foreach (Vector3 position in positions)
            {
                GameObject coin = Instantiate(coinPrefab, position, Quaternion.identity);
                spawnedCoins.Add(coin);
            }
        }
        else
        {
            Debug.LogWarning("No spawn locations defined for coins!");
        }
    }
    
    public void ClearCoins()
    {
        foreach (GameObject coin in spawnedCoins)
        {
            if (coin != null)
            {
                Destroy(coin);
            }
        }
        spawnedCoins.Clear();
    }
    
    // Helper method to visualize spawn points in the editor
    void OnDrawGizmos()
    {
        // Draw easy mode spawn points
        if (easyModeSpawnPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in easyModeSpawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.5f);
                }
            }
        }
        
        if (easyModePositions != null)
        {
            Gizmos.color = Color.green;
            foreach (Vector3 pos in easyModePositions)
            {
                Gizmos.DrawWireSphere(pos, 0.5f);
            }
        }
        
        // Draw hard mode spawn points
        if (hardModeSpawnPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (Transform point in hardModeSpawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.5f);
                }
            }
        }
        
        if (hardModePositions != null)
        {
            Gizmos.color = Color.red;
            foreach (Vector3 pos in hardModePositions)
            {
                Gizmos.DrawWireSphere(pos, 0.5f);
            }
        }
    }
}