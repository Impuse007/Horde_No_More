using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            LoadGame(playerController,skillTree);
            Debug.Log(skillTree.unlockedSkills.Count + skillTree.skills.Count);
        }
    }
    
    void SetButtonAlphaValue(float alphaValue)
    {
        Color color = loadGameButton.GetComponent<Image>().color;
        color.a = alphaValue / 255f; // Convert alpha to the range of 0 to 1
        loadGameButton.GetComponent<Image>().color = color;
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
        SaveSystem.SaveGame(skillTree , playerController);
    }

    public static void LoadGame(PlayerController player, SkillTree skillTree)
    {
        GameData data = SaveSystem.LoadGame();
        if (data != null)
        {
            // Create a set of currently unlocked skills for quick lookup
            HashSet<string> currentUnlockedSkills = new HashSet<string>(skillTree.unlockedSkills.Select(s => s.skillName));

            foreach (string skillName in data.unlockedSkills)
            {
                // Only add and unlock the skill if it is not already unlocked
                if (!currentUnlockedSkills.Contains(skillName))
                {
                    Skill skill = skillTree.skills.Find(s => s.skillName == skillName);
                    if (skill != null)
                    {
                        skill.Unlock(player); // Ensure player is not null
                        skillTree.unlockedSkills.Add(skill);
                        Debug.Log("Skill Unlocked: " + skill.skillName);

                        if (skill.skillPrefab != null)
                        {
                            GameObject upgradeInstance = Instantiate(skill.skillPrefab);
                            upgradeInstance.SetActive(true);
                        }
                    }
                }
                player.playerMoney = data.moneyFromPlayer;
            }
        }
        Debug.Log("Game Loaded" + skillTree.unlockedSkills.Count);
    }
    
    public void NewGame()
    {
        playerController.playerMoney = 0;
        playerController.playerMaxHealth = 25;
        playerController.speed = 3;
        playerController.isHealingUnlocked = false;
        playerController.isSpecialAttackUnlocked = false;
        playerController.dashSpeed = 15.0f;
        playerController.dashCooldown = 8.0f;
        playerController.healingAmount = 20;
        playerController.healingCooldown = 20.0f;
        playerController.specialAttackDamage = 15;
        playerController.specialAttackRange = 10.0f;
        playerController.specialAttackCooldown = 10.0f;
        playerController.playerDamage = 5;
        playerController.basicAttackRange = 4.0f; 
        playerController.basicAttackCooldown = 2.5f;
        playerController.basicAttackSpeed = 6.0f;
        waveNumber = 0;
        kills = 0;
        moneyEarned = 0;
        timeInGame = 0;
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
