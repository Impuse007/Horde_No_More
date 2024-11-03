using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTree : MonoBehaviour
{
    public static SkillTree skillTree;
    public void Awake() => skillTree = this;

    public List<Skill> skills;
    public List<Skill> unlockedSkills;
    private PlayerController playerController;
    
    public TMP_Text nameText;
    public TMP_Text descText;
    public TMP_Text costText;
    
    private Skill currentSkill;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        unlockedSkills = new List<Skill>();
    }

    public void UnlockSkill(string skillName)
    {
        Skill skill = skills.Find(s => s.skillName == skillName);
        if (skill != null && !skill.isUnlocked && playerController.playerMoney >= skill.cost)
        {
            playerController.playerMoney -= skill.cost;
            skill.Unlock(playerController);
            unlockedSkills.Add(skill);
            skill.UpdateSkillStatusText();
            FindObjectOfType<GameManager>().SavingGame();
            if (skill.skillPrefab != null)
            {
                skill.skillPrefab.SetActive(true);
                //skill.nextSkill.UpdateSkillStatusText();
                Debug.Log("Skill unlocked: " + skillName);
            }
        }
        else
        {
            Debug.LogWarning("Skill not found, already unlocked, or not enough money: " + skillName);
        }
    }

    public void SetSelectedSkill(string skillName)
    {
        currentSkill = skills.Find(s => s.skillName == skillName);
        if (currentSkill != null)
        {
            nameText.text = currentSkill.skillName;
            descText.text = currentSkill.description;
            costText.text = "Cost: " + currentSkill.cost;
        }
        else
        {
            Debug.LogWarning("Skill not found: " + skillName);
        }
    }
    
    public void UnlockSelectedSkill()
    {
        if (currentSkill != null)
        {
            UnlockSkill(currentSkill.skillName);
        }
        else
        {
            Debug.LogWarning("No skill selected!");
        }
    }
    
    public Skill GetSkillByName(string skillName)
    {
        foreach (Skill skill in unlockedSkills)
        {
            if (skill.skillName == skillName)
            {
                return skill;
            }
        }
        return null;
    }
}
