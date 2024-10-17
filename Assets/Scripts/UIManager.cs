using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // UI Manager will be responsible for managing the UI elements in the game, switching scenes with the correct UI elements.
    LevelManager levelManager;
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
                break;
            case switchUI.GameOver:
                gameOverUI.SetActive(true);
                break;
            case switchUI.GameWin:
                gameWinUI.SetActive(true);
                break;
            case switchUI.GamePause:
                gameOverUI.SetActive(true);
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
    
    
    
    
    
    
}
