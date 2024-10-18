using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    private PlayerController playerController;
    private bool isAttacking = false;
    
    public event EnemyDealthHandler OnEnemyDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        player = GameObject.FindWithTag("Player");

        // Ensure playerHealthManager is assigned
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogWarning("Player is not assigned.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking)
        {
            EnemyMeleeMovement();
        }
        
        if (Time.time >= nextAttackTime)
        {
            EnemyMeleeAttack();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    public void EnemyMeleeAttack()
    {
        if (player == null || playerController == null)
        {
            Debug.LogWarning("Player or PlayerHealthManager is not assigned.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            playerController.TakeDamage(enemyDamage);
            Debug.Log("Player hit by enemy: " + gameObject.name);
            StartCoroutine(EndAttack());
        }
        
        transform.rotation = Quaternion.identity;
    }

    private IEnumerator EndAttack()
    {
        yield return new WaitForSeconds(attackCooldown); 
        isAttacking = false;
    }

    public void EnemyMeleeMovement()
    {
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned.");
            return;
        }

        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position - direction * 0.1f, speed * Time.deltaTime);
        transform.rotation = Quaternion.identity; // Lock rotation
    }
    
    public void TakeDamage(int damage)
    {
        enemyCurrentHealth -= damage;
        
        if (enemyCurrentHealth <= 0)
        {
            Die(); // Might change this to a different method
        }
    }

    public void Die() // Might be put in the enemy base class
    {
        OnEnemyDeath?.Invoke();
        GameObject.Destroy(gameObject);
        playerController.playerMoney += moneyDrop;
    }
}
