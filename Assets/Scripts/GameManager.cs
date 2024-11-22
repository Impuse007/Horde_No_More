using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using Save;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LevelManager levelManager;
    public UIManager uiManager;
    public PlayerController playerController;
    public SkillTree skillTree; 
    Skill unSkill;
    public Results_Screen resultsScreen;
    public SFXManager SfxManager;
    
    public Button loadGameButton;
    public ColorBlock colorBlock;
    
    public int playerScore;
    public int highScore;
    public float timeInGame;
    public int waveNumber;
    public int kills;
    public int moneyEarned;
    
    public void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        skillTree = FindObjectOfType<SkillTree>();
        unSkill = skillTree.skills.Find(s => s.skillName == "Unlock Special Ability");
        
        if (!File.Exists(Save.SaveSystem._savePath))
        {
            NewGame();
            loadGameButton.interactable = false;
            Debug.Log("New Save File Created");
        }
        else
        {
            SetButtonAlphaValue(255);  
            loadGameButton.interactable = true;
            LoadGame(playerController, skillTree);
        }
        
        CheckLevel();
    }
    
    void SetButtonAlphaValue(float alphaValue)
    {
        Color color = loadGameButton.GetComponent<Image>().color;
        color.a = alphaValue / 255f; // Convert alpha to the range of 0 to 1
        loadGameButton.GetComponent<Image>().color = color;
    }
    
    void CheckLevel()
    {
        string currentLevel = SceneManager.GetActiveScene().name;
        if (currentLevel == "MainMenu")
        {
            SfxManager.PlayMusic(0); // Play the main menu music
        }
        else
        {
            SfxManager.PlayMusic(1); // Play the gameplay music
        }
        //if (currentLevel == "Level1")
        //{
        //    playerController.gameObject.SetActive(true);
        //}
        //else
        //{
        //    playerController.gameObject.SetActive(false);
        //}
    }

    public void Update()
    {
        AddTime();
    }
    
    public void AddScore(int score)
    {
        playerScore += score;
    }
    
    public void AddTime()
    {
        timeInGame += Time.deltaTime;
    }
    
    public void AddWave()
    {
        waveNumber++;
    }
    
    public void AddKill()
    {
        kills += 1;
        return;
    }
    
    public void AddMoney(int money)
    {
        moneyEarned += money;
    }
    
    public void SavingGame()
    {
        Save.SaveSystem.SaveGame(playerController, skillTree);
    }
    
    public static void LoadGame(PlayerController player, SkillTree skillTree)
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            player.playerMoney = data.playerMoney;
            player.playerMaxHealth = data.playerMaxHealth;
            player.playerCurrentHealth = data.playerCurrentHealth;

            foreach (string skillName in data.unlockedSkills)
            {
                Skill skill = skillTree.skills.Find(s => s.skillName == skillName);
                if (skill != null)
                {
                    skill.Unlock(player);
                    skillTree.unlockedSkills.Add(skill);

                    // Instantiate the upgrade prefab if it exists
                    if (skill.skillPrefab != null)
                    {
                        GameObject upgradeInstance = Instantiate(skill.skillPrefab);
                        upgradeInstance.SetActive(true);
                    }
                }
            }
        }
    }
    
    public void NewGame()
    {
        playerController.playerMoney = 0;
        playerController.playerMaxHealth = 75;
        playerController.speed = 5;
        playerController.isHealingUnlocked = false;
        playerController.isSpecialAttackUnlocked = false;
        playerController.dashSpeed = 15.0f;
        playerController.dashCooldown = 3.0f;
        playerController.healingAmount = 20;
        playerController.healingCooldown = 15.0f;
        playerController.specialAttackDamage = 15;
        playerController.specialAttackRange = 10.0f;
        playerController.specialAttackCooldown = 5.0f;
        playerController.playerDamage = 20;
        playerController.basicAttackRange = 3.0f; // Might be used for fut
        playerController.basicAttackCooldown = 2.0f;
        skillTree.unlockedSkills = new List<Skill>();
        foreach (var skill in skillTree.skills)
        {
            skill.isUnlocked = false;
            if (skill.skillPrefab != null)
            {
                skill.skillPrefab.SetActive(false);
            }
            skill.UpdateSkillStatusText();
        }
        SavingGame(); 
        ResetResults();
        
    }

    public void PlayerWon()
    {
        uiManager.ShowWinText();
        SavingGame();
    }
    
    public void PlayerLost()
    {
        uiManager.ShowLoseText();
        SavingGame();
    }
    
    public void ResetResults()
    {
        timeInGame = 0;
        waveNumber = 0;
        kills = 0;
        moneyEarned = 0;
    }
}
