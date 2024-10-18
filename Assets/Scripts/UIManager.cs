using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // UI Manager will be responsible for managing the UI elements in the game, switching scenes with the correct UI elements.
    public LevelManager levelManager;
    public PlayerController playerController; // For the Player Die method // Might not be needed
    public WaveManager waveManager;
    public enum switchUI
    {
        MainMenu,
        GameOver,
        GameWin,
        GamePause,
        GamePlay,
        UpgradeMenu,
        ControlsMenu
    }
    
    [Header("UI Elements")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject upgradeMenuUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gameWinUI;
    [SerializeField] GameObject gamePauseUI;
    [SerializeField] GameObject gamePlayUI;
    [SerializeField] GameObject controlsMenuUI;
    
    public void Start()
    {
        levelManager = GetComponent<LevelManager>();
        ActivateUIBasedOnScene();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Update()
    {
        PauseingGame();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ActivateUIBasedOnScene();
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SwitchUI(switchUI ui)
    {
        mainMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        gameWinUI.SetActive(false);
        gamePauseUI.SetActive(false);
        gamePlayUI.SetActive(false);
        upgradeMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        
        switch (ui)
        {
            case switchUI.MainMenu:
                mainMenuUI.SetActive(true);
                Time.timeScale = 1; // Might not need this look up in playerController on top of this script
                playerController.playerCurrentHealth = playerController.playerMaxHealth; // Might not need this look up in playerController on top of this script
                playerController.playerSprite.enabled = true; // Might not need this look up in playerController on top of this script
                playerController.playerSprite.color = new Color(1, 1, 1, 1); // Might not need this look up in playerController on top of this script
                break;
            case switchUI.GameOver:
                gameOverUI.SetActive(true);
                break;
            case switchUI.GameWin:
                gameWinUI.SetActive(true);
                break;
            case switchUI.GamePause:
                gamePauseUI.SetActive(true);
                break;
            case switchUI.GamePlay:
                gamePlayUI.SetActive(true);
                break;
            case switchUI.UpgradeMenu:
                upgradeMenuUI.SetActive(true);
                break;
            case switchUI.ControlsMenu:
                controlsMenuUI.SetActive(true);
                break;
                
        }
    }

    public void ActivateUIBasedOnScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        if (sceneName == "Main Menu")
        {
            SwitchUI(switchUI.MainMenu);
        }
        else if (sceneName == "Level 1")
        {
            SwitchUI(switchUI.GamePlay);
        }
        else if (sceneName == "Game Win")
        {
            SwitchUI(switchUI.GameWin);
        }
        
        Debug.Log("UI Activated based on scene: " + sceneName);
    }

    public void PauseingGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                SwitchUI(switchUI.GamePlay);
            }
            else
            {
                Time.timeScale = 0;
                SwitchUI(switchUI.GamePause);
            }
        }
    }
    
    public void UpGradeMenu()
    {
        SwitchUI(switchUI.UpgradeMenu);
        playerController.playerSprite.enabled = false; // Might not need this look up in playerController on top of this script
    }
}
