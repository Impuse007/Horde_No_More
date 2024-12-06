using Save;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject Player;
    public GameManager gameManager;
    public SFXManager sfxManager;
    
    public void StartGame(string levelName)
    {
        Application.LoadLevel(levelName);
        Player.SetActive(true);
        gameManager.waveNumber = 0;
        gameManager.kills = 0;
        gameManager.moneyEarned = 0;
        gameManager.timeInGame = 0;
        Player.transform.position = new Vector3(0, 0, 0);
        playerController.playerCurrentHealth = playerController.playerMaxHealth;
        playerController.isInvincible = false;
        UpdateHealthBar();
        gameManager.ResetResults();
        Time.timeScale = 1;
    }
    
    public void UpdateHealthBar()
    {
        playerController.playerHealthBar.value = playerController.playerCurrentHealth;
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        gameManager.SavingGame();
        Player.SetActive(false);
    }
    
    public void QuitGame()
    {
        gameManager.SavingGame();
        Application.Quit();
    }
}
