using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    HealthManager _enemyManager;
    public PlayerController _playerController;
    WaveManager _waveManager;
    public int enemyMaxHealth = 100;
    public int enemyCurrentHealth;
    public int enemyDamage = 10;
    public float speed = 5.0f;
    public float attackRange = 1.0f;
    public float attackCooldown = 1.0f;
    public float nextAttackTime = 0.0f;
    
    public delegate void EnemyDealthHandler();
    public event EnemyDealthHandler OnEnemyDeath;
    
    public int moneyDrop = 10;
    
    // GameObjects;
    public GameObject player;
    public GameObject enemy;

    public void Start()
    {
        _enemyManager.currentHealth = enemyCurrentHealth;
        _enemyManager.maxHealth = enemyMaxHealth;
        player = GameObject.FindWithTag("Player");
       if (player != null)
       {
           _playerController = player.GetComponent<PlayerController>();
       }
       else
       {
           Debug.LogWarning("Player is not assigned.");
       }
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
        OnEnemyDeath?.Invoke();
        if (_playerController != null)
        {
            _playerController.playerMoney += moneyDrop;
        }
        GameObject.Destroy(enemy);
    }
}
