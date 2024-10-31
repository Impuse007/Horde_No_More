﻿using UnityEngine;

namespace DefaultNamespace
{
    public class SpecialWaveAttack : MonoBehaviour
    {
        PlayerController playerController;
        public int damage;
        private bool hasHit = false;
        
        public void Start()
        {
            playerController = Object.FindObjectOfType(typeof(PlayerController)) as PlayerController;
            damage = playerController.playerDamage;
        }
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyMelee enemyMelee = other.GetComponent<EnemyMelee>();
                if (enemyMelee != null)
                {
                    enemyMelee.TakeDamage(damage); // Deal damage to the melee enemy
                    Debug.Log(damage + " damage dealt to " + other.name);
                }

                RangeEnemy rangeEnemy = other.GetComponent<RangeEnemy>();
                if (rangeEnemy != null)
                {
                    rangeEnemy.TakeDamage(damage); // Deal damage to the range enemy
                    Debug.Log(damage + " damage dealt to " + other.name);
                }

                hasHit = true; // Set the flag to true
                Destroy(gameObject); // Destroy the fireball after hitting the enemy
            }
        }
    }
}