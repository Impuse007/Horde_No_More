using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class GameData 
{
    public List<string> unlockedSkills; // Store names of unlocked skills
    public int moneyFromPlayer; // Store player money

    public GameData(SkillTree skillTree, PlayerController playerController)
    {
        unlockedSkills = new List<string>();
        foreach (var skill in skillTree.unlockedSkills)
        {
            unlockedSkills.Add(skill.skillName);
        }
        moneyFromPlayer = playerController.playerMoney;
    }
}
