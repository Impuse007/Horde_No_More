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
    public int dashIncreaseSpeed;
    public int dashDecreaseCooldown;
    public int weaponBasicDamageIncrease;
    public int weaponBasicSpeedIncrease;
    public int weaponBasicRangeIncrease;
    public int weaponBasicCooldownDecrease;
    public int weaponSpecialDamageIncrease;
    public int weaponSpecialSpeedIncrease;
    public int weaponSpecialRangeIncrease;
    public int weaponSpecialCooldownDecrease;
    public int playerHealingIncrease;
    public int playerHealingCooldownDecrease;
    public int playerEarningIncrease;
    
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
    
    public Skill(string name, string desc, int cost, int healthIncrease, int speedIncrease, int dashIncreaseSpeed, int weaponBasicDamageIncrease, int weaponBasicRangeIncrease, int weaponBasicSpeedIncrease, bool unlockSpecialAbility, int weaponSpecialDamageIncrease, int weaponSpecialSpeedIncrease, int weaponSpecialCooldownDecrease, bool unlockHealingAbility, int playerHealingIncrease, int playerHealingCooldownDecrease, int playerEarningIncrease, int dashDecreaseCooldown, int weaponBasicCooldownDecrease)
    {
        this.skillName = name;
        this.description = desc;
        this.cost = cost;
        this.isUnlocked = false;
        this.healthIncrease = healthIncrease;
        this.speedIncrease = speedIncrease;
        this.dashIncreaseSpeed = dashIncreaseSpeed;
        this.dashDecreaseCooldown = dashDecreaseCooldown;
        this.weaponBasicDamageIncrease = weaponBasicDamageIncrease;
        this.weaponBasicRangeIncrease = weaponBasicRangeIncrease;
        this.weaponBasicSpeedIncrease = weaponBasicSpeedIncrease;
        this.weaponBasicCooldownDecrease = weaponBasicCooldownDecrease;
        this.unlockSpecialAbility = unlockSpecialAbility;
        this.weaponSpecialDamageIncrease = weaponSpecialDamageIncrease;
        this.weaponSpecialSpeedIncrease = weaponSpecialSpeedIncrease;
        this.weaponSpecialCooldownDecrease = weaponSpecialCooldownDecrease;
        this.unlockHealingAbility = unlockHealingAbility;
        this.playerHealingIncrease = playerHealingIncrease;
        this.playerHealingCooldownDecrease = playerHealingCooldownDecrease;
        this.playerEarningIncrease = playerEarningIncrease;
    }

    public void Unlock(PlayerController playerController)
    {
        isUnlocked = true;
        playerController.playerMaxHealth += healthIncrease;
        playerController.speed += speedIncrease;
        playerController.dashSpeed += dashIncreaseSpeed;
        playerController.dashCooldown -= dashDecreaseCooldown;
        playerController.playerDamage += weaponBasicDamageIncrease;
        playerController.basicAttackSpeed += weaponBasicSpeedIncrease;
        playerController.basicAttackRange += weaponBasicRangeIncrease;
        playerController.basicAttackCooldown += weaponBasicCooldownDecrease;
        playerController.specialAttackDamage += weaponSpecialDamageIncrease;
        playerController.specialAttackSpeed += weaponSpecialSpeedIncrease;
        playerController.specialAttackCooldown -= weaponSpecialCooldownDecrease;
        playerController.healingAmount += playerHealingIncrease;
        playerController.healingCooldown -= playerHealingCooldownDecrease;
        playerController.playerMoney += playerEarningIncrease;
        
        if (unlockSpecialAbility)
        {
            playerController.isSpecialAttackUnlocked = true;
        }
        
        if (unlockHealingAbility)
        {
            playerController.isHealingUnlocked = true;
        }
        
        if (skillPrefab != null)
        {
            skillPrefab.SetActive(true);
        }
        
        UpdateSkillStatusText();
        Debug.Log(skillName + " has been unlocked! Health: " + healthIncrease + ", Speed: " + speedIncrease + ", Dash: " + dashIncreaseSpeed + ", Weapon Damage: " + weaponBasicDamageIncrease);
    }
    
    public void UpdateSkillStatusText()
    {
        if (skillStatusText != null)
        {
            skillStatusText.text = isUnlocked ? "Bought" : "Not Bought";
        }
    }
}
