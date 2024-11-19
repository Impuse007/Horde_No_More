using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCooldownImage : MonoBehaviour
{
    public Image cooldownImage;
    public TextMeshProUGUI cooldownText;
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
            float cooldownTime = playerController.nextSpecialAttackTime - Time.time; // Time remaining until the next special attack
            float cooldownDuration = playerController.specialAttackCooldown; // Cooldown duration of the special attack
            cooldownTime = Mathf.Clamp(cooldownTime, 0, cooldownDuration); // Clamp the cooldown time between 0 and the cooldown duration
            cooldownText.text = Mathf.Ceil(cooldownTime).ToString(); // Display the cooldown time as an integer, BUGGED counts down from 0 to negative infinity
            cooldownImage.fillAmount = Mathf.Clamp01(cooldownTime / cooldownDuration);
        }
    }
}
