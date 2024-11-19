using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

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

        // Set the alpha for the skill image to 0.5
        foreach (Skill skill in skills)
        {
            GameObject skillGameObject = EventSystem.current.currentSelectedGameObject;
            if (skillGameObject != null)
            {
                Image skillImage = skillGameObject.GetComponent<Image>();
                if (skillImage != null)
                {
                    Color color = skillImage.color;
                    color.a = 0.5f;
                    skillImage.color = color;
                }
            }
        }
    }

    public void UnlockSkill(string skillName) // You can buy the skill again
    {
        Skill skill = skills.Find(s => s.skillName == skillName);
        if (skill != null && !skill.isUnlocked && playerController.playerMoney >= skill.cost)
        {
            playerController.playerMoney -= skill.cost;
            skill.Unlock(playerController);
            unlockedSkills.Add(skill);
            skill.UpdateSkillStatusText();
            GameObject skillGameObject = EventSystem.current.currentSelectedGameObject;
            if (skillGameObject != null)
            {
                skillGameObject.SetActive(true);
                Image skillImage = skillGameObject.GetComponent<Image>();
                if (skillImage != null)
                {
                    skillImage.color = Color.green;
                }
            }
            Debug.Log("Skill unlocked: " + skillName);
            FindObjectOfType<GameManager>().SavingGame();
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

    public void OnPointerEnterSkill(string skillName)
    {
        GameObject skillGameObject = EventSystem.current.currentSelectedGameObject;
        Skill skill = skills.Find(s => s.skillName == skillName);
        if (skill != null)
        {
            nameText.text = skill.skillName;
            descText.text = skill.description;
            costText.text = "Cost: " + skill.cost;
        }
        if (skillGameObject != null)
        {
            
            Image skillImage = skillGameObject.GetComponent<Image>();
            if (skillImage != null)
            {
                Color color = skillImage.color;
                color.a = 1f;
                skillImage.color = color;
            }
            Debug.Log("Skill hovered: " + skillGameObject.name);
        }
    }

    public void OnPointerExitSkill()
    {
        GameObject skillGameObject = EventSystem.current.currentSelectedGameObject;
        if (skillGameObject != null)
        {
            nameText.text = "";
            descText.text = "";
            costText.text = "";
            
            // Reset the skill GameObject's alpha
            Image skillImage = skillGameObject.GetComponent<Image>();
            if (skillImage != null)
            {
                Color color = skillImage.color;
                color.a = 0.5f; // Make it semi-transparent
                skillImage.color = color;
            }
            Debug.Log("Skill hover exited: " + skillGameObject.name);
        }
    }
}
