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

    public GameObject healingObject;
    public GameObject waveObject;

    void Start()
    {
        if (playerController == null)
        {
            Debug.Log("PlayerController not found.");
        }

        playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (playerController == null)
    {
        playerController = FindObjectOfType<PlayerController>();
        if (playerController == null)
        {
            return;
        }
    }

        if (playerController != null)
        {
            float[] cooldownTimes =
            {
                playerController.nextSpecialAttackTime, playerController.nextHealingTime, playerController.nextDashTime
            };
            float[] cooldownDurations =
            {
                playerController.specialAttackCooldown, playerController.healingCooldown, playerController.dashCooldown
            };
            bool[] skillUnlocked =
            {
                playerController.isSpecialAttackUnlocked, playerController.isHealingUnlocked, true
            }; // Dash is always unlocked

            int minLength = Mathf.Min(cooldownImages.Length, cooldownTexts.Length, skillUnlocked.Length);

            for (int i = 0; i < minLength; i++)
            {
                if (skillUnlocked[i])
                {
                    float cooldownTime = cooldownTimes[i] - Time.time;
                    float cooldownDuration = cooldownDurations[i];
                    cooldownTime = Mathf.Clamp(cooldownTime, 0, cooldownDuration);
                    cooldownTexts[i].text = Mathf.Ceil(cooldownTime).ToString();
                    cooldownImages[i].fillAmount = Mathf.Clamp01(cooldownTime / cooldownDuration);
                    cooldownImages[i].gameObject.SetActive(true);
                    cooldownTexts[i].gameObject.SetActive(true);

                    Color imageColor = cooldownImages[i].color;
                    imageColor.a = cooldownTime > 0 ? 0.5f : 1f; // Set alpha to 0.5 if on cooldown, otherwise 1
                    cooldownImages[i].color = imageColor;

                    if (i == 1) // Assuming index 1 is for healing skill
                    {
                        healingObject.SetActive(true);
                    }
                    else if (i == 0) // Assuming index 0 is for wave skill
                    {
                        waveObject.SetActive(true);
                    }
                }
                else
                {
                    cooldownImages[i].gameObject.SetActive(false);
                    cooldownTexts[i].gameObject.SetActive(false);

                    if (i == 1) // Assuming index 1 is for healing skill
                    {
                        healingObject.SetActive(false);
                    }
                    else if (i == 0) // Assuming index 0 is for wave skill
                    {
                        waveObject.SetActive(false);
                    }
                }
            }
        }
    }
}
