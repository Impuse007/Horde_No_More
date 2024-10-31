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
    public int weaponSpecialDamageIncrease;
    public int weaponSpecialSpeedIncrease;
    public int weaponSpecialRangeIncrease;
    public int weaponSpecialCooldownDecrease;
    public int playerHealingIncrease;
    public int playerHealingCooldownDecrease;
    
    // Ability to unlock the skill
    public bool unlockSpecialAbility;
    public bool unlockHealingAbility;
    
    public GameObject skillPrefab;
    public TMP_Text skillStatusText;
    
    // Index of the next skill in the skill tree
    [HideInInspector]
    public Skill nextSkill;

    public void Start()
    {
        UpdateSkillStatusText();
    }
    
    public Skill(string name, string desc, int cost, int healthIncrease, int speedIncrease, int dashIncrease, int weaponBasicDamageIncrease, int weaponBasicRangeIncrease, int weaponBasicSpeedIncrease, bool unlockSpecialAbility, int weaponSpecialDamageIncrease, int weaponSpecialSpeedIncrease, int weaponSpecialCooldownDecrease, bool unlockHealingAbility, int playerHealingIncrease, int playerHealingCooldownDecrease)
    {
        this.skillName = name;
        this.description = desc;
        this.cost = cost;
        this.isUnlocked = false;
        this.healthIncrease = healthIncrease;
        this.speedIncrease = speedIncrease;
        this.dashIncrease = dashIncrease;
        this.weaponBasicDamageIncrease = weaponBasicDamageIncrease;
        this.weaponBasicRangeIncrease = weaponBasicRangeIncrease;
        this.weaponBasicSpeedIncrease = weaponBasicSpeedIncrease;
        this.unlockSpecialAbility = unlockSpecialAbility;
        this.weaponSpecialDamageIncrease = weaponSpecialDamageIncrease;
        this.weaponSpecialSpeedIncrease = weaponSpecialSpeedIncrease;
        this.weaponSpecialCooldownDecrease = weaponSpecialCooldownDecrease;
        this.unlockHealingAbility = unlockHealingAbility;
        this.playerHealingIncrease = playerHealingIncrease;
        this.playerHealingCooldownDecrease = playerHealingCooldownDecrease;
    }

    public void Unlock(PlayerController playerController)
    {
        isUnlocked = true;
        playerController.playerMaxHealth += healthIncrease;
        playerController.speed += speedIncrease;
        playerController.dashSpeed += dashIncrease;
        playerController.playerDamage += weaponBasicDamageIncrease;
        playerController.basicAttackSpeed += weaponBasicSpeedIncrease;
        playerController.basicAttackRange += weaponBasicRangeIncrease;
        playerController.specialAttackDamage += weaponSpecialDamageIncrease;
        playerController.specialAttackSpeed += weaponSpecialSpeedIncrease;
        playerController.specialAttackCooldown -= weaponSpecialCooldownDecrease;
        playerController.healingAmount += playerHealingIncrease;
        playerController.healingCooldown -= playerHealingCooldownDecrease;
        
        if (unlockSpecialAbility)
        {
            playerController.isSpecialAttackUnlocked = true;
        }
        
        if (unlockHealingAbility)
        {
            playerController.isHealingUnlocked = true;
        }
        
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
