using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

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
    public TMP_Text notEnoughMoneyText;

    private Skill currentSkill;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        //unlockedSkills = new List<Skill>();

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
        
        Debug.Log("SkillTree started" + unlockedSkills.Count);
    }

    public void UnlockSkill(string skillName)
    {
        Skill skill = skills.Find(s => s.skillName == skillName);
        if (skill != null && !skill.isUnlocked)
        {
            if (playerController.playerMoney >= skill.cost)
            {
                playerController.playerMoney -= skill.cost;
                skill.Unlock(playerController);
                unlockedSkills.Add(skill);
                skill.UpdateSkillStatusText();
                GameObject skillGameObject = EventSystem.current.currentSelectedGameObject;
                if (skillGameObject != null)
                {
                    skillGameObject.SetActive(true);
                    HoveringOverSkills hoveringOverSkills = skillGameObject.GetComponent<HoveringOverSkills>();
                    if (hoveringOverSkills != null)
                    {
                        hoveringOverSkills.skillBrought = true;
                    }
                    Image skillImage = skillGameObject.GetComponent<Image>();
                    if (skillImage != null)
                    {
                        skillImage.color = Color.green;
                    }
                }
                SFXManager.instance.PlayEnvironmentSFX(2);
                Debug.Log("Skill unlocked: " + skillName);
                FindObjectOfType<GameManager>().SavingGame();
            }
            else
            {
                StartCoroutine(DisableNotEnoughMoneyText());
                Debug.LogWarning("Not enough money to unlock skill: " + skillName);
            }
        }
        else
        {
            Debug.LogWarning("Skill not found or already unlocked: " + skillName);
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
    }

    public void OnPointerExitSkill()
    {
        GameObject skillGameObject = EventSystem.current.currentSelectedGameObject;
        if (skillGameObject != null)
        {
            nameText.text = "";
            descText.text = "";
            costText.text = "";
        }
    }
    
    private IEnumerator DisableNotEnoughMoneyText()
    {
        notEnoughMoneyText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        notEnoughMoneyText.gameObject.SetActive(false);
    }
}
