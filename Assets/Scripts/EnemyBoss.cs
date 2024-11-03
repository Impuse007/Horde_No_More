using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefaultNamespace;

public class EnemyBoss : EnemyBase
{
    EnemyBase enemyBase;

    public int bossDamage = 10;
    public int bossHealth = 50;
    public int bossCurrentHealth;
    public float moveBossSpeed = 2f;
    public float stopDistance = 5f;
    public float shootCooldown = 2f;
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public Transform spriteTransform;

    public Slider healthBar;

    private Transform player;
    private float nextShootTime;

    private PlayerController playerController;
    private Rigidbody2D rb;

    public delegate void EnemyDealthHandler();
    public event EnemyDealthHandler OnEnemyDeath;

    void Start()
    {
        bossCurrentHealth = bossHealth;
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
        
        if (healthBar != null)
        {
            healthBar.maxValue = bossHealth;
            healthBar.value = bossCurrentHealth;
        }
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
        
        Vector2 direction = (player.position - transform.position).normalized;
        if (direction.x > 0)
        {
            spriteTransform.localScale = new Vector3(Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
        }
        else if (direction.x < 0)
        {
            spriteTransform.localScale = new Vector3(-Mathf.Abs(spriteTransform.localScale.x), spriteTransform.localScale.y, spriteTransform.localScale.z);
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        Vector2 targetPosition = (Vector2)player.position - direction;
        Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveBossSpeed * Time.fixedDeltaTime);
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
            arrowScript.arrowDamage = bossDamage;
        }
    }

    public void TakeDamage(int damage)
    {
        bossCurrentHealth -= damage;
        if (bossCurrentHealth <= 0)
        {
            Die();
        }

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = bossCurrentHealth;
        }
        
        StartCoroutine(FlashRed());
    }

    void Die()
    {
        OnEnemyDeath?.Invoke();
        Destroy(gameObject);
    }
    
    private IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
