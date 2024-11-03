using System.Collections;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    private PlayerController playerController;
    public SpriteRenderer SpriteTransform;
    private bool isAttacking = false;
    private bool isDead = false;
    private Rigidbody2D rb;

    public Animator animator;

    public event EnemyDealthHandler OnEnemyDeath;

    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();

        // Ensure playerController is assigned
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
    void FixedUpdate()
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
            Debug.LogWarning("Player or PlayerController is not assigned.");
            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= attackRange)
        {
            isAttacking = true;
            Debug.Log("Triggering attack animation");
            animator.SetTrigger("Attack");
            StartCoroutine(PerformAttack());
        }

        transform.rotation = Quaternion.identity;
    }

    private IEnumerator PerformAttack()
    {
        // Wait for the attack animation to reach the point where damage should be dealt
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 2);

        if (player != null && playerController != null)
        {
            playerController.TakeDamage(enemyDamage);
            Debug.Log("Player hit by enemy: " + gameObject.name);
        }

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 2);
        isAttacking = false;
    }

    public void EnemyMeleeMovement()
    {
        animator.ResetTrigger("Attack");
        
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned.");
            return;
        }

        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 targetPosition = player.transform.position - direction;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
        if (direction.x > 0)
        {
           SpriteTransform.flipX = false;
        }
        else if (direction.x < 0)
        {
            SpriteTransform.flipX = true;
        }
        transform.rotation = Quaternion.identity; // Lock rotation
    }

    public void TakeDamage(int damage)
    {
        enemyCurrentHealth -= damage;
        StartCoroutine(FlashRed());

        if (enemyCurrentHealth <= 0 && !isDead)
        {
            isDead = true;
            Die(); // Might change this to a different method
        }
    }

    public void Die() // Might be put in the enemy base class
    {
        OnEnemyDeath?.Invoke();
        GameObject.Destroy(gameObject);
        playerController.playerMoney += moneyDrop;
    }

    public IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = SpriteTransform.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
}
