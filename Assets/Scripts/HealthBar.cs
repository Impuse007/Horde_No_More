using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    public Slider healthBar;
    private HealthManager healthManager;
    private EnemyBase enemyBase;
    
    void Start()
    {
        healthManager = GetComponent<HealthManager>();
        enemyBase = GetComponent<EnemyBase>();
        healthBar = GetComponentInChildren<Slider>();
        healthBar.maxValue = enemyBase.enemyMaxHealth;
        healthBar.value = enemyBase.enemyCurrentHealth;
    }
    
    void Update()
    {
        healthBar.value = enemyBase.enemyCurrentHealth;
    }
}
