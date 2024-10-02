using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    private HealthManager healthManager;
    
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = healthManager.maxHealth;
        healthBar.value = healthManager.currentHealth;
    }
    
    void Update()
    {
        healthBar.value = healthManager.currentHealth;
    }
}
