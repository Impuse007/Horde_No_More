using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    HealthManager _enemyManager;
    PlayerController _playerController;
    public int enemyMaxHealth = 100;
    public int enemyCurrentHealth;
    public int enemyDamage = 10;
    public float speed = 5.0f;
    public float attackRange = 1.0f;
    public float attackCooldown = 1.0f;
    
    // GameObjects;
    public GameObject player;
    public GameObject enemy;

    public void Start()
    {
        _enemyManager.currentHealth = enemyCurrentHealth;
        _enemyManager.maxHealth = enemyMaxHealth;
        
    }
    
    public void TakeDamage(int damage)
    {
        enemyCurrentHealth -= damage;
        
        if (enemyCurrentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Die()
    {
        GameObject.Destroy(enemy);
    }
}
