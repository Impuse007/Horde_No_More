using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class Fireball_Attack : MonoBehaviour
    {
        public PlayerController playerController;
        public ParticleSystem fireball;
        public int damage;
        private bool hasHit = false;
        
        public void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
            damage = playerController.playerDamage;
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Fireball hit: " + other.name);
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
                EnemyBoss enemyBoss = other.GetComponent<EnemyBoss>();
                if (enemyBoss != null)
                {
                    enemyBoss.TakeDamage(damage); // Deal damage to the boss enemy
                    Debug.Log(damage + " damage dealt to " + other.name);
                }

                hasHit = true; // Set the flag to true
                SFXManager.instance.PlayPlayerSFX(1); // Play the enemy hit sound effect
                ParticleSystem instantiatedFireball = Instantiate(fireball, transform.position, Quaternion.identity);
                instantiatedFireball.Play();
                Destroy(gameObject); // Destroy the fireball after hitting the enemy
                StopCoroutine(StopParticleAfterSeconds(instantiatedFireball, 1f));
                
            }
        }
        
        private IEnumerator StopParticleAfterSeconds(ParticleSystem particle, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            particle.Stop();
        }
    }
}