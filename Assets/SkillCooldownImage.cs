using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCooldownImage : MonoBehaviour
{
    // Need images for the skills to show the Player which skill is on cooldown
    
    // This script will be responsible for updating the cooldown images and texts for the special attack and healing skills
    // Might do the Fireball maybe.
    // Dash will have a cooldown but I need it in here first. Dash is going to have a circle that fills up and not a square.
    public Image[] cooldownImages;
    public TextMeshProUGUI[] cooldownTexts;
    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController not found.");
        }
    }

    void Update()
    {
        if (playerController != null)
        {
            float[] cooldownTimes = { playerController.nextSpecialAttackTime, playerController.nextHealingTime, playerController.nextDashTime }; // Wave Attack first, and then Healing
            float[] cooldownDurations = { playerController.specialAttackCooldown, playerController.healingCooldown, playerController.dashCooldown }; // Wave Attack first, and then Healing

            for (int i = 0; i < cooldownImages.Length; i++)
            {
                float cooldownTime = cooldownTimes[i] - Time.time; // Cooldown time remaining for the current skill
                float cooldownDuration = cooldownDurations[i]; // Cooldown duration for the current skill
                cooldownTime = Mathf.Clamp(cooldownTime, 0, cooldownDuration); // Clamp the cooldown time to be between 0 and the cooldown duration
                cooldownTexts[i].text = Mathf.Ceil(cooldownTime).ToString(); // Display the cooldown time as an integer
                cooldownImages[i].fillAmount = Mathf.Clamp01(cooldownTime / cooldownDuration); // Update the fill amount of the cooldown image
            }
        }
    }
    
    /* BUGS:
     Cooldown counts to the negative numbers - FIXED
     */
}
