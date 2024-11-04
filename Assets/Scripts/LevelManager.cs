using Save;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        Player.transform.position = new Vector3(0, 0, 0);
        playerController.playerCurrentHealth = playerController.playerMaxHealth;
        playerController.isInvincible = false;
        UpdateHealthBar();
        gameManager.ResetResults();
        Time.timeScale = 1;
        sfxManager.PlayGamePlayMusic();
    }
    
    public void UpdateHealthBar()
    {
        playerController.playerHealthBar.value = playerController.playerCurrentHealth;
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        SaveSystem.SaveGame(playerController, playerController.skillTree);
        Player.SetActive(false);
        sfxManager.PlayMainMenuMusic();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
