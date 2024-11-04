using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DefaultNamespace;
using Save;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelManager levelManager;
    public UIManager uiManager;
    public PlayerController playerController;
    public SkillTree skillTree;
    public Results_Screen resultsScreen;
    
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
        
        if (!File.Exists(Save.SaveSystem._savePath))
        {
            NewGame();
            Debug.Log("New Save File Created");
        }
        else
        {
            LoadGame(playerController, skillTree);
        }
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
                }
            }
        }
    }
    
    public void NewGame()
    {
        playerController.playerMoney = 0;
        playerController.playerMaxHealth = 75;
        skillTree.unlockedSkills = new List<Skill>();
        foreach (var skill in skillTree.skills)
        {
            skill.isUnlocked = false;
            //if (skill.skillPrefab != null)
            //{
            //    skill.skillPrefab.SetActive(false);
            //}
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
