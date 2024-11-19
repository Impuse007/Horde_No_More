using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

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
        AddEventTriggers();
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
            if (skill.skillPrefab != null)
            {
                skill.skillPrefab.SetActive(true);
                Debug.Log("Skill unlocked: " + skillName);
            }
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

    private void AddEventTriggers()
    {
        foreach (Skill skill in skills)
        {
            if (skill.skillPrefab != null)
            {
                EventTrigger trigger = skill.skillPrefab.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = skill.skillPrefab.gameObject.AddComponent<EventTrigger>();
                }

                EventTrigger.Entry entryEnter = new EventTrigger.Entry();
                entryEnter.eventID = EventTriggerType.PointerEnter;
                entryEnter.callback.AddListener((eventData) => { OnPointerEnterSkill(skill.skillName); });
                trigger.triggers.Add(entryEnter);

                EventTrigger.Entry entryExit = new EventTrigger.Entry();
                entryExit.eventID = EventTriggerType.PointerExit;
                entryExit.callback.AddListener((eventData) => { OnPointerExitSkill(); });
                trigger.triggers.Add(entryExit);

                EventTrigger.Entry entryClick = new EventTrigger.Entry();
                entryClick.eventID = EventTriggerType.PointerClick;
                entryClick.callback.AddListener((eventData) => { UnlockSkill(skill.skillName); });
                trigger.triggers.Add(entryClick);

                EventTrigger.Entry entryDown = new EventTrigger.Entry();
                entryDown.eventID = EventTriggerType.PointerDown;
                entryDown.callback.AddListener((eventData) => { UnlockSkill(skill.skillName); });
                trigger.triggers.Add(entryDown);
            }
        }
    }

    public void OnPointerEnterSkill(string skillName)
    {
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
        nameText.text = "";
        descText.text = "";
        costText.text = "";
    }
}
