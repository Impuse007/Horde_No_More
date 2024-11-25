using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMelee : EnemyBase
{
    private static readonly int Death = Animator.StringToHash("Death");
    private PlayerController playerController;
    public SpriteRenderer SpriteTransform;
    private bool isAttacking = false;
    private bool isDead = false;
    private Rigidbody2D rb;
    public Slider healthBar;

    public Animator animator;
    public MoneyDrop MoneyDrop;
    public WaveManager waveManager;

    public event EnemyDealthHandler OnEnemyDeath;
    
    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        player = GameObject.FindWithTag("Player");
        MoneyDrop = FindObjectOfType<MoneyDrop>();
        rb = GetComponent<Rigidbody2D>();
        
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
        else
        {
            Debug.LogWarning("Player is not assigned.");
        }
    }
    
    void FixedUpdate()
    {
        if (isDead)
        {
            return;
        }
        
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
        if (isDead)
        {
            return;
        }
        
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
        if (isDead)
        {
            return;
        }
        animator.ResetTrigger("Attack");
        
        if (player == null)
        {
            Debug.LogWarning("Player is not assigned.");
            return;
        }

        // Move towards player and stop at a certain distance
        int maxDistance = 3;
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 targetPosition = player.transform.position - direction;
        Vector3 newPosition = Vector3.MoveTowards(transform.position, targetPosition, maxDistance * Time.fixedDeltaTime);
        rb.MovePosition(newPosition);
        
        if (direction.x > 0) // Flip sprite based on direction
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
        if (isDead)
        {
            return;
        }
        
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
        DropMoneyAndGiveToPlayer();
        playerController.playerMoney += moneyDrop;
        StartCoroutine(DeadAnimation());
    }
    
    private void DropMoneyAndGiveToPlayer()
    {
        MoneyDrop.DropMoney(transform.position);
    }

    public IEnumerator FlashRed()
    {
        SpriteRenderer spriteRenderer = SpriteTransform.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }
    
    private IEnumerator DeadAnimation()
    {
        animator.ResetTrigger("Attack");
        animator.SetTrigger(Death);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.simulated = false;
        healthBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
