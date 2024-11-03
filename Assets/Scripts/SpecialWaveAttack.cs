using UnityEngine;

namespace DefaultNamespace
{
    public class SpecialWaveAttack : MonoBehaviour
    {
        PlayerController playerController;
        public int damage;
        private bool hasHit = false;
        public float knockbackForce = 1000f;
        
        public void Start()
        {
            playerController = Object.FindObjectOfType(typeof(PlayerController)) as PlayerController;
            damage = playerController.specialAttackDamage;
        }
        
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                //Rigidbody2D enemyRb = other.GetComponent<Rigidbody2D>();
                //if (enemyRb != null)
                //{
                //    Vector2 knockbackDirection = (other.transform.position + transform.position); // Calculate the knockback direction
                //    enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse); // Apply the knockback force
                //    Debug.Log("Knockback applied to " + other.name); 
                //}
                //else
                //{
                //    Debug.LogWarning("No Rigidbody2D found on " + other.name);
                //}
                
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
            }
        }
    }
}