using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class Fireball_Attack : MonoBehaviour
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
            if (other.CompareTag("Enemy") && !hasHit)
            {
                other.GetComponent<EnemyMelee>().TakeDamage(damage);
                Debug.Log(damage + " damage dealt to " + other.name);
                hasHit = true; // Set the flag to true
                Destroy(gameObject); // Destroy the fireball after hitting the enemy
            }
        }
    }
}