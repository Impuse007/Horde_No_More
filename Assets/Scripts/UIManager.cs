using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // UI Manager will be responsible for managing the UI elements in the game, switching scenes with the correct UI elements.
    public LevelManager levelManager;
    public PlayerController playerController; // For the Player Die method // Might not be needed
    public WaveManager waveManager;
    
    public TMP_Text playerMoneyText;
    public enum switchUI
    {
        MainMenu,
        GameOver,
        GamePause,
        GamePlay,
        UpgradeMenu,
        ControlsMenu,
        ResultsMenu
    }
    
    [Header("UI Elements")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject upgradeMenuUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gamePauseUI;
    [SerializeField] GameObject gamePlayUI;
    [SerializeField] GameObject controlsMenuUI;
    [SerializeField] GameObject resultsMenuUI;
    
    public void Start()
    {
        levelManager = GetComponent<LevelManager>();
        ActivateUIBasedOnScene();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Update()
    {
        PausingGame();
        playerMoneyText.text = "Money: " + playerController.playerMoney;
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
        gamePauseUI.SetActive(false);
        gamePlayUI.SetActive(false);
        upgradeMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        resultsMenuUI.SetActive(false);
        
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
            case switchUI.ResultsMenu:
                resultsMenuUI.SetActive(true);
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
        
        Debug.Log("UI Activated based on scene: " + sceneName);
    }

    public void PausingGame()
    {
        if (SceneManager.GetActiveScene().name == "Level 1")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 1)
                {
                    Time.timeScale = 0;
                    SwitchUI(switchUI.GamePause);
                }
                else
                {
                    Time.timeScale = 1;
                    SwitchUI(switchUI.GamePlay);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
    
    public void UpGradeMenu() // G is capital in UpGradeMenu, change to UpgradeMenu
    {
        //SceneManager.LoadScene("Main Menu");
        SwitchUI(switchUI.UpgradeMenu);
        //playerController.playerSprite.enabled = false; // Might not need this look up in playerController on top of this script
    }
}
