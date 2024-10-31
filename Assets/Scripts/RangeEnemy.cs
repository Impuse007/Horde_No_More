using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class RangeEnemy : EnemyBase
{
    public int rangeDamage = 10;
    public int rangeHealth = 50;
    public float moveSpeed = 2f;
    public float stopDistance = 5f;
    public float shootCooldown = 2f;
    public GameObject arrowPrefab;
    public Transform shootPoint;

    private Transform player;
    private float nextShootTime;

    private PlayerController playerController;
    private Rigidbody2D rb;

    public delegate void EnemyDealthHandler();
    public event EnemyDealthHandler OnEnemyDeath;

    void Start()
    {
        rangeHealth = enemyCurrentHealth;
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerController = playerObject.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogWarning("Player is not assigned.");
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            if (Time.time >= nextShootTime)
            {
                ShootArrow();
                nextShootTime = Time.time + shootCooldown;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 targetPosition = (Vector2)player.position - direction;
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
    }

    void ShootArrow()
    {
        Vector2 direction = (player.position - shootPoint.position).normalized;
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = direction * 10f;

        EnemyRangeArrow arrowScript = arrow.GetComponent<EnemyRangeArrow>();
        if (arrowScript != null)
        {
            arrowScript.arrowDamage = rangeDamage; // Set the arrow's damage to the enemy's damage value
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

    void Die()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }
}
