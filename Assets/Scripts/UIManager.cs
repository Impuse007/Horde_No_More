using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // UI Manager will be responsible for managing the UI elements in the game, switching scenes with the correct UI elements.
    public LevelManager levelManager;
    public PlayerController playerController; // For the Player Die method // Might not be needed
    public WaveManager waveManager;
    public Results_Screen resultsScreen;
    
    public TMP_Text playerMoneyText;
    
    // Results Screen
    public TMP_Text winText;
    public TMP_Text loseText;
    
    // Upgrade Stats Text
    public TMP_Text playerHealthText;
    public TMP_Text playerDamageText;
    public TMP_Text playerSpeedText;
    public TMP_Text playerSpecialAttackText;
    public TMP_Text playerHealingText;
    public TMP_Text playerDashSpeedText;
    
    public enum switchUI
    {
        MainMenu,
        GameOver,
        GamePause,
        GamePlay,
        UpgradeMenu,
        ControlsMenu,
        ResultsMenu,
        OptionsMenu
    }
    
    [Header("UI Elements")]
    [SerializeField] GameObject mainMenuUI;
    [SerializeField] GameObject upgradeMenuUI;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject gamePauseUI;
    [SerializeField] GameObject gamePlayUI;
    [SerializeField] GameObject controlsMenuUI;
    [SerializeField] GameObject resultsMenuUI;
    [SerializeField] GameObject optionsMenuUI;
    
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
        playerDamageText.text = "Damage: " + playerController.playerDamage;
        playerHealthText.text = "Health: " + playerController.playerMaxHealth;
        playerSpeedText.text = "Speed: " + playerController.speed;
        playerSpecialAttackText.text = "Special Attack: " + playerController.specialAttackDamage;
        playerHealingText.text = "Healing: " + playerController.healingAmount;
        playerDashSpeedText.text = "Dash Speed: " + playerController.dashSpeed;
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
        optionsMenuUI.SetActive(false);
        
        switch (ui)
        {
            case switchUI.MainMenu:
                mainMenuUI.SetActive(true);
                Time.timeScale = 1; // Might not need this look up in playerController on top of this script
                playerController.playerSprite.enabled = true; // Might not need this look up in playerController on top of this script
                playerController.playerSprite.color = new Color(1, 1, 1, 1); // Might not need this look up in playerController on top of this script
                break;
            case switchUI.GameOver:
                gameOverUI.SetActive(true);
                resultsScreen.ResultScreenTween();
                ShowLoseText();
                break;
            case switchUI.GamePause:
                gamePauseUI.SetActive(true);
                break;
            case switchUI.OptionsMenu:
                optionsMenuUI.SetActive(true);
                break;
            case switchUI.GamePlay:
                gamePlayUI.SetActive(true);
                playerController.playerSprite.enabled = true; // Might not need this look up in playerController on top of this script
                break;
            case switchUI.UpgradeMenu:
                upgradeMenuUI.SetActive(true);
                break;
            case switchUI.ControlsMenu:
                controlsMenuUI.SetActive(true);
                break;
            case switchUI.ResultsMenu:
                resultsMenuUI.SetActive(true);
                resultsScreen.ResultScreenTween();
                ShowWinText();
                Time.timeScale = 0;
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
                    playerController.playerSprite.flipX = false;
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
        SwitchUI(switchUI.UpgradeMenu);
    }
    
    public void OptionsMenu()
    {
        SwitchUI(switchUI.OptionsMenu);
    }
    
    public void ShowWinText()
    {
        winText.gameObject.SetActive(true);
        loseText.gameObject.SetActive(false);
    }
    
    public void ShowLoseText()
    {
        loseText.gameObject.SetActive(true);
        winText.gameObject.SetActive(false);
    }
    
    public void MainMenu()
    {
        mainMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }
}
