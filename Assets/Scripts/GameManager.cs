using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelManager levelManager;
    public UIManager uiManager;
    public PlayerController playerController;
    public SkillTree skillTree;
    
    public int playerScore;
    public int highScore;
    public float timeInGame;
    public int waveNumber;
    public int kills;
    public int moneyEarned;
    
    public void Awake()
    {
        LoadGame();
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
        
    }
    
    public void LoadGame()
    {
        GameData data = Save.SaveSystem.LoadGame();
        if (data != null)
        {
            playerController.playerMoney = data.playerMoney;
            playerController.playerMaxHealth = data.playerMaxHealth;
            playerController.playerCurrentHealth = data.playerCurrentHealth;
            skillTree.unlockedSkills = new List<Skill>();
            foreach (var skillName in data.unlockedSkills)
            {
                Skill skill = skillTree.GetSkillByName(skillName);
                if (skill != null)
                {
                    skillTree.unlockedSkills.Add(skill);
                }
            }
        }
    }
}
