using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    PlayerController player;
    EnemyBase enemyBase;
    UIManager uiManager;
    public List<Wave> waves; // List of waves
    public TextMeshProUGUI waveText; // UI Text to display wave number
    public int currentWaveIndex = 0; // Current wave index
    private bool isSpawning = false; // Flag to check if spawning is in progress
    public int enemiesAlive = 0; // Number of enemies currently alive

    void Start()
    {
        waveText = GameObject.Find("UI Manager/Canvas/Gameplay/WaveText")?.GetComponent<TextMeshProUGUI>();

        if (waveText == null)
        {
            Debug.LogError("WaveText not found. Ensure there is a TextMeshProUGUI component named 'WaveText' in the scene.");
            return;
        }
        StartCoroutine(SpawnWaves());
        
        uiManager = GameObject.Find("GameManager/UI Manager")?.GetComponent<UIManager>();
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];
            isSpawning = true;

            waveText.text = "Wave " + currentWave.waveNumber;
            waveText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f); // Display the text for 2 seconds
            waveText.gameObject.SetActive(false);

            enemiesAlive = currentWave.enemyPrefabs.Count;

            foreach (GameObject enemyPrefab in currentWave.enemyPrefabs)
            {
                SpawnEnemy(enemyPrefab, currentWave.spawnPoints);
            }

            isSpawning = false;

            while (enemiesAlive > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(currentWave.waveDelay);

            currentWaveIndex++;
            
            if (currentWaveIndex >= waves.Count)
            {
                WinGame();
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab, Transform[] spawnPoints)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.GetComponent<EnemyMelee>().OnEnemyDeath += HandleEnemyDeath; // Subscribe to the enemy death event
        GameObject player = GameObject.FindWithTag("Player");
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
        {
            currentWaveIndex++;
            StartWave();
        }
    }

    private void StartWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("All waves completed!");
            return;
        }

        Wave currentWave = waves[currentWaveIndex];
        waveText.text = "Wave " + currentWave.waveNumber;
        waveText.gameObject.SetActive(true);
        StartCoroutine(HideWaveTextAfterDelay(2f)); // Hide the text after 2 seconds

        enemiesAlive = currentWave.enemyPrefabs.Count;

        foreach (GameObject enemyPrefab in currentWave.enemyPrefabs)
        {
            SpawnEnemy(enemyPrefab, currentWave.spawnPoints);
        }
    }

    private IEnumerator HideWaveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        waveText.gameObject.SetActive(false);
    }
    
    public void WinGame()
    {
        uiManager.SwitchUI(UIManager.switchUI.GameWin);
        Debug.Log("You win!");
    }
}
