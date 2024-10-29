using TMPro;
using UnityEngine;

[System.Serializable]
public class Skill
{
    public string skillName;
    public string description;
    public int cost;
    public bool isUnlocked;
    
    // Values to increase
    public int healthIncrease;
    public int speedIncrease;
    public int dashIncrease;
    public int weaponBasicDamageIncrease;
    public int weaponBasicSpeedIncrease;
    public int weaponBasicRangeIncrease;
    
    public GameObject skillPrefab;
    public TMP_Text skillStatusText;
    
    // Index of the next skill in the skill tree
    [HideInInspector]
    public Skill nextSkill;

    public void Start()
    {
        UpdateSkillStatusText();
    }
    
    public Skill(string name, string desc, int cost, int healthIncrease, int speedIncrease, int dashIncrease, int weaponBasicDamageIncrease)
    {
        this.skillName = name;
        this.description = desc;
        this.cost = cost;
        this.isUnlocked = false;
        this.healthIncrease = healthIncrease;
        this.speedIncrease = speedIncrease;
        this.dashIncrease = dashIncrease;
        this.weaponBasicDamageIncrease = weaponBasicDamageIncrease;
    }

    public void Unlock(PlayerController playerController)
    {
        isUnlocked = true;
        playerController.playerMaxHealth += healthIncrease;
        playerController.speed += speedIncrease;
        playerController.dashSpeed += dashIncrease;
        playerController.playerDamage += weaponBasicDamageIncrease;
        UpdateSkillStatusText();
        Debug.Log(skillName + " has been unlocked! Health: " + healthIncrease + ", Speed: " + speedIncrease + ", Dash: " + dashIncrease + ", Weapon Damage: " + weaponBasicDamageIncrease);
    }
    
    public void UpdateSkillStatusText()
    {
        if (skillStatusText != null)
        {
            skillStatusText.text = isUnlocked ? "Bought" : "Not Bought";
        }
    }
}
