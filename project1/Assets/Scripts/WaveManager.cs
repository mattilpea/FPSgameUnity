using System.Collections;
using UnityEngine;
using TMPro;
public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Wave Settings")]
    public float spawnInterval = 5f;  // Time between spawning waves of 5 enemies
    public GameObject enemyPrefab;     // Enemy prefab
    public Transform[] spawnPoints;    // Spawn points for enemies
    [SerializeField] public TextMeshProUGUI currentWave;
    private int waveCount = 0;
    private float enemyPerWave = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentWave.text =  "Current Wave: " +  waveCount.ToString();

        StartCoroutine(SpawnEnemiesEveryInterval());  // Start spawning enemies at intervals
    }

    // Coroutine to spawn 5 enemies every spawnInterval seconds
    private IEnumerator SpawnEnemiesEveryInterval()
    {
        while (true)  // Keep spawning enemies in a loop
        {
            SpawnEnemies();  // Spawn 5 enemies
            currentWave.text = "Current Wave: " + waveCount.ToString();
            yield return new WaitForSeconds(spawnInterval);  // Wait for the specified interval
        }
    }

    // Spawn 5 enemies at random spawn points
    private void SpawnEnemies()
    {
        for (int i = 0; i < Mathf.Ceil(enemyPerWave); i++)  // Spawn 5 enemies
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);  // Get a random spawn point
            Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);  // Spawn the enemy
        }
        enemyPerWave *= 1.2f;
        waveCount += 1;
    }
}
