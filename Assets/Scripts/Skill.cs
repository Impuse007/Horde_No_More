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
    public int weaponDamageIncrease;
    
    public GameObject skillPrefab;
    
    // Index of the next skill in the skill tree
    [HideInInspector]
    public Skill nextSkill;

    public Skill(string name, string desc, int cost, int healthIncrease, int speedIncrease, int dashIncrease, int weaponDamageIncrease)
    {
        this.skillName = name;
        this.description = desc;
        this.cost = cost;
        this.isUnlocked = false;
        this.healthIncrease = healthIncrease;
        this.speedIncrease = speedIncrease;
        this.dashIncrease = dashIncrease;
        this.weaponDamageIncrease = weaponDamageIncrease;
    }

    public void Unlock(PlayerController playerController)
    {
        isUnlocked = true;
        playerController.playerMaxHealth += healthIncrease;
        playerController.speed += speedIncrease;
        playerController.dashSpeed += dashIncrease;
        playerController.playerDamage += weaponDamageIncrease;
        Debug.Log(skillName + " has been unlocked! Health: " + healthIncrease + ", Speed: " + speedIncrease + ", Dash: " + dashIncrease + ", Weapon Damage: " + weaponDamageIncrease);
    }
}
