using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{
    public GameObject howToPlayPanel;
    public GameObject controlsPanel;
    public GameObject gameplayPanel;
    public GameObject abilitiesPanel;
    public GameObject mainMenuPanel;
    
    public GameObject mainMenuButton;
    public GameObject playButton;
    
    public void HowToPlayButton()
    {
        howToPlayPanel.SetActive(true);
        controlsPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        abilitiesPanel.SetActive(false);
        mainMenuButton.SetActive(true);
        mainMenuPanel.SetActive(false);
        playButton.SetActive(false);
    }
    
    public void ControlsButton()
    {
        controlsPanel.SetActive(true);
        gameplayPanel.SetActive(false);
        abilitiesPanel.SetActive(false);
    }
    
    public void GameplayButton()
    {
        controlsPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        abilitiesPanel.SetActive(false);
    }
    
    public void AbilitiesButton()
    {
        controlsPanel.SetActive(false);
        gameplayPanel.SetActive(false);
        abilitiesPanel.SetActive(true);
    }
    
    public void MainMenuButton()
    {
        howToPlayPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    
    public void NewGameButton()
    {
        howToPlayPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        mainMenuButton.SetActive(false);
        playButton.SetActive(true);
    }
    
    
}
