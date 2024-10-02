using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    HealthManager _enemyManager;
    public int maxHealth = 100;
    public int currentHealth;
    public int enemyDamage = 10;
    public float speed = 5.0f;
    public float attackRange = 1.0f;
    public float attackCooldown = 1.0f;
    
    // GameObjects;
    public GameObject player;
    public GameObject enemy;

    public void Start()
    {
        _enemyManager.currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        damage = enemyDamage;
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        // Needs a dying animation
    }
}
