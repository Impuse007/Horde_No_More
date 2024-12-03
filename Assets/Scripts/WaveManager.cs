using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    PlayerController player;
    EnemyBase enemyBase;
    UIManager uiManager;
    GameManager gameManager;
    Results_Screen resultsScreen;
    
    // Lighting
    public Light2D directionalLight;
    public Color originalColor;
    
    public List<Wave> waves; // List of waves
    public TextMeshProUGUI persistentWaveText; // UI Text to display persistent wave number
    public TextMeshProUGUI waveText; // UI Text to display wave number
    public TextMeshProUGUI enemiesLeft; // UI Text to display number of enemies left
    public int currentWaveIndex = 0; // Current wave index
    private bool isSpawning = false; // Flag to check if spawning is in progress
    public int enemiesAlive = 0; // Number of enemies currently alive

    void Start()
    {
        waveText = GameObject.Find("UI Manager/Canvas/Gameplay/WaveText")?.GetComponent<TextMeshProUGUI>();
        persistentWaveText = GameObject.Find("UI Manager/Canvas/Gameplay/PersistentWaveText")?.GetComponent<TextMeshProUGUI>();
        enemiesLeft = GameObject.Find("UI Manager/Canvas/Gameplay/EnemiesLeft")?.GetComponent<TextMeshProUGUI>();

        if (waveText == null)
        {
            Debug.LogError("WaveText not found. Ensure there is a TextMeshProUGUI component named 'WaveText' in the scene.");
            return;
        }
        StartCoroutine(SpawnWaves());

        gameManager = GameObject.Find("GameManager")?.GetComponent<GameManager>();
        uiManager = GameObject.Find("GameManager/UI Manager")?.GetComponent<UIManager>();

        // Ensure the light color is set to the original color at the start
        if (directionalLight != null)
        {
            originalColor = directionalLight.color;
            directionalLight.color = originalColor;
            Debug.Log("Light Found");
        }
    }
    
    private void Update()
    {
        enemiesLeft.text = "Enemies Left: " + enemiesAlive;
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];
            isSpawning = true;
            persistentWaveText.text = "Wave " + currentWave.waveNumber + "/30";
            enemiesAlive = 0;

            if (currentWaveIndex == 14)
            {
                SFXManager.instance.PlayMusic(2); // Play the boss music
            }
            else if (currentWaveIndex == 29)
            {
                SFXManager.instance.PlayMusic(3); // Play the final boss music
            }
            else if (currentWaveIndex != 14 && currentWaveIndex != 29)
            {
                SFXManager.instance.PlayMusic(1);
            }

            if (currentWaveIndex == 15)
            {
                directionalLight.color = new Color(0.47f, 0.47f, 0.47f, 1f);
            }
            else if (currentWaveIndex < 15)
            {
                directionalLight.color = new Color(1f, 1f, 1f, 1f);
            }
            
            WaveTextBig(); // Could be a bug with this method
            waveText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            waveText.gameObject.SetActive(false);


            foreach (Wave.EnemySpawnInfo enemySpawnInfo in currentWave.enemySpawnInfos)
            {
                for (int i = 0; i < enemySpawnInfo.spawnCount; i++)
                {
                    SpawnEnemy(enemySpawnInfo.enemyPrefab, currentWave.spawnPoints, currentWave);
                    enemiesAlive++;
                }
            }

            isSpawning = false;

            while (enemiesAlive > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(currentWave.waveDelay);
            SFXManager.instance.PlayWaveSFX(0); // Play the wave complete sound effect
            currentWaveIndex++;
            gameManager.AddWave();
            if (currentWaveIndex >= waves.Count)
            {
                WinGame();
            }
        }
    }

    private void SpawnEnemy(GameObject enemyPrefab, Transform[] spawnPoints, Wave currentWave)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned.");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        
        EnemyMelee enemyMelee = enemy.GetComponent<EnemyMelee>();
        if (enemyMelee != null)
        {
            enemyMelee.moneyDrop = currentWave.moneyDrop;
            enemyMelee.enemyDamage = currentWave.enemyDamage;
            enemyMelee.speed = currentWave.enemySpeed;
            enemyMelee.enemyMaxHealth = currentWave.enemyHealth;
            enemy.GetComponent<EnemyMelee>().OnEnemyDeath += HandleEnemyDeath;
            Debug.Log(enemyMelee.enemyCurrentHealth);
        }
        
        RangeEnemy rangeEnemy = enemy.GetComponent<RangeEnemy>();
        if (rangeEnemy != null)
        {
            rangeEnemy.moneyRangeDrop = currentWave.rangeMoneyDrop;
            rangeEnemy.rangeDamage = currentWave.rangeDamage;
            rangeEnemy.moveSpeed = currentWave.rangeSpeed;
            rangeEnemy.rangeHealth = currentWave.rangeHealth;
            enemy.GetComponent<RangeEnemy>().OnEnemyDeath += HandleEnemyDeath;
            Debug.Log(rangeEnemy.rangeCurrentHealth);
        }
        
        EnemyBoss enemyBoss = enemy.GetComponent<EnemyBoss>();
        if (enemyBoss != null)
        {
            enemyBoss.moneyBossDrop = currentWave.bossMoneyDrop;
            enemyBoss.bossDamage = currentWave.bossDamage;
            enemyBoss.moveBossSpeed = currentWave.bossSpeed;
            enemyBoss.bossHealth = currentWave.bossHealth;
            enemy.GetComponent<EnemyBoss>().OnEnemyDeath += HandleEnemyDeath;
            Debug.Log(enemyBoss.bossCurrentHealth);
        }
        
        GameObject player = GameObject.FindWithTag("Player");
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;
        gameManager.AddKill();

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
        waveText.text = "Wave Incoming!";
        waveText.gameObject.SetActive(true);
        StartCoroutine(HideWaveTextAfterDelay(3f));
        persistentWaveText.text = "Wave " + currentWave.waveNumber + "/30";

        enemiesAlive = 0;

        foreach (Wave.EnemySpawnInfo enemySpawnInfo in currentWave.enemySpawnInfos)
        {
            for (int i = 0; i < enemySpawnInfo.spawnCount; i++)
            {
                SpawnEnemy(enemySpawnInfo.enemyPrefab, currentWave.spawnPoints, currentWave);
                enemiesAlive++;
            }
        }
    }

    private IEnumerator HideWaveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        waveText.gameObject.SetActive(false);
    }
    
    public void WinGame()
    {
        uiManager.SwitchUI(UIManager.switchUI.ResultsMenu);
        resultsScreen.ResultScreenTween();
        Debug.Log("You win!");
    }
    
    public IEnumerator WaveTextBig()
    {
        waveText.text = "Wave Incoming!";
        waveText.fontSize = 50;
        yield return new WaitForSeconds(1f);
        waveText.fontSize = 36;
    }
}
