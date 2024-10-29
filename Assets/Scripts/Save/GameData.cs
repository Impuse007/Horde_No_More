using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GameData 
{
    public int playerMoney;
    public float playerMaxHealth;
    public float playerCurrentHealth;
    public List<string> unlockedSkills;
    
    public GameData(PlayerController player, SkillTree skillTree)
    {
        playerMoney = player.playerMoney;
        playerMaxHealth = player.playerMaxHealth;
        playerCurrentHealth = player.playerCurrentHealth;
        unlockedSkills = new List<string>();
        foreach (var skill in skillTree.unlockedSkills)
        {
            unlockedSkills.Add(skill.skillName);
        }
    }
        
}
