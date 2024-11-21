using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class RangeEnemy : EnemyBase
{
    EnemyBase enemyBase;
    public MoneyDrop MoneyDrop;

    public int rangeDamage = 10;
    public int rangeHealth = 50;
    public int rangeCurrentHealth;
    public float moveSpeed = 2f;
    public float stopDistance = 5f;
    public float shootCooldown = 2f;
    public int moneyRangeDrop;
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public Transform spriteTransform;

    public Slider healthBar;

    private Transform player;
    private float nextShootTime;
    private bool isShooting = false;

    private PlayerController playerController;
    private Rigidbody2D rb;

    public delegate void EnemyDealthHandler();
    public event EnemyDealthHandler OnEnemyDeath;

    void Start()
    {
        rangeCurrentHealth = rangeHealth;
        GameObject playerObject = GameObject.FindWithTag("Player");
        MoneyDrop = FindObjectOfType<MoneyDrop>();

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
            healthBar.maxValue = rangeHealth;
            healthBar.value = rangeCurrentHealth;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stopDistance && !isShooting)
        {
            MoveTowardsPlayer();
        }
        else if (distanceToPlayer == stopDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            if (Time.time >= nextShootTime)
            {
                StartCoroutine(ShootArrow());
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
        float distanceToTarget = Vector2.Distance(rb.position, targetPosition);

        // Only move if the distance to the target is greater than a small threshold
        if (distanceToTarget > stopDistance + 0.5f && Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPosition);
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator ShootArrow()
    {
        isShooting = true;

        Vector2 direction = (player.position - shootPoint.position).normalized;
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
        arrow.GetComponent<Rigidbody2D>().velocity = direction * 10f;

        // Rotate the arrow to face the direction of the shot
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        EnemyRangeArrow arrowScript = arrow.GetComponent<EnemyRangeArrow>();
        if (arrowScript != null)
        {
            arrowScript.arrowDamage = rangeDamage;
        }

        yield return new WaitForSeconds(0.1f); // Adjust the wait time as needed

        isShooting = false;
    }

    public void TakeDamage(int damage)
    {
        rangeCurrentHealth -= damage;
        if (rangeCurrentHealth <= 0)
        {
            Die();
        }

        // Update the health bar
        if (healthBar != null)
        {
            healthBar.value = rangeCurrentHealth;
        }

        StartCoroutine(FlashRed());
    }

    void Die()
    {
        OnEnemyDeath?.Invoke();
        DropMoneyAndGiveToPlayer();
        playerController.playerMoney += moneyRangeDrop;
        Destroy(gameObject);
    }
    
    private void DropMoneyAndGiveToPlayer()
    {
        MoneyDrop.DropMoney(transform.position);
    }

    private IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
