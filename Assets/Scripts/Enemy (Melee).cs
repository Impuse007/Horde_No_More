using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;

        // Ensure player is assigned
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }

        // Ensure playerHealthManager is assigned
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMeleeMovement();
        
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
            playerController.TakeDamage(enemyDamage);
            Debug.Log("Player hit by enemy: " + gameObject.name);
        }
    }

    public void EnemyMeleeMovement()
    {
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned.");
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
