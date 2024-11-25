using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player Components
    [Header("Player Components")]
    HealthManager playerManager;
    public LevelManager levelManager;
    public UIManager uiManager;
    private Rigidbody2D rb;
    public GameObject arrowCursor;
    
    // Player Movement Values
    [Header("Player Main Stats")]
    public Slider playerHealthBar;
    public float playerMaxHealth = 75;
    public float playerCurrentHealth;
    public float speed = 5.0f;
    
    // Player Attack Values
    [Header("Player Basic Attack Stats")]
    public Transform basicAttackPoint;
    public GameObject basicAttackPrefab;
    public int playerDamage = 10;
    public float basicAttackRange = 1.0f; // Might be used for future reference
    public float basicAttackCooldown = 1.0f;
    public float nextAttackTime = 0.0f;
    public float basicAttackSpeed = 10.0f;
    public LayerMask enemyLayers;
    
    [Header("Player Special Attack Stats")]
    public bool isSpecialAttackUnlocked;
    public Transform specialAttackPoint;
    public GameObject specialAttackPrefab;
    public int specialAttackDamage = 20;
    public float specialAttackRange = 1.0f;
    public float specialAttackCooldown = 5.0f;
    public float nextSpecialAttackTime = 0.0f;
    public float specialAttackSpeed = 10.0f;
    public TMP_Text specialAttackCooldownText; // Moved in another script
    public LayerMask enemyLayersSpecial;
    
    [Header("Player Healing Stats")]
    public bool isHealingUnlocked;
    public int healingAmount = 20;
    public float healingCooldown = 5.0f;
    public float nextHealingTime = 0.0f;
    public TMP_Text healingCooldownText; // Moved in another script
    public ParticleSystem healingEffect;
    
    // Player Dash Values
    [Header("Player Dash Stats")]
    public float dashSpeed = 15.0f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 3.0f;
    public float nextDashTime = 0.0f;
    private float dashTime;
    private float dashCooldownTime;
    private bool isDashing;
    public ParticleSystem dashEffect;
    public TextMeshProUGUI dashCooldownText;
    
    // Player Flash Values
    [Header("Player Flash Stats")]
    public SpriteRenderer playerSprite;
    public int numberOfFlashes = 3;
    public float flashDuration = 0.1f;
    public bool isInvincible;
    public float invincibleDuration = 2.0f;
    
    // Money Values
    [Header("Player Money Stats")]
    public int playerMoney = 1000;
    public TextMeshProUGUI moneyText;
    
    [Header("Player Animations")]
    public Animator playerAnimator;
    
    [Header("Player Debugging")]
    public TMP_Text playerHealthText;
    
    [Header("Skill Manager")]
    public SkillTree skillTree;
    
    public void Awake()
    {
        playerManager = GetComponent<HealthManager>();
        rb = GetComponent<Rigidbody2D>();
        playerHealthBar.maxValue = playerMaxHealth;
        playerHealthBar.value = playerCurrentHealth;
        playerCurrentHealth = playerMaxHealth;
        playerSprite = GetComponent<SpriteRenderer>();
        
        if (playerManager != null)
        {
            playerManager.maxHealth = (int)playerMaxHealth;
            playerManager.currentHealth = (int)playerCurrentHealth;
        }
    }
    
    public void Update()
    {
        PlayerMoves();
        CursorArrow();
        
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextDashTime)
        {
            StartCoroutine(Dash());
            nextDashTime = Time.time + dashCooldown;
            //StartCoroutine(PlayDashEffect());
        }
        
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                BasicAttack();
                nextAttackTime = Time.time + 1f / basicAttackCooldown;
                Debug.Log("Attacked");
            }
        }
        
        if (Time.time >= nextSpecialAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                SpecialAttack();
                //nextSpecialAttackTime = Time.time + specialAttackCooldown;
                Debug.Log("Special Attacked");
            }
        }
        
        if (Time.time >= nextHealingTime)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Heal();
                Debug.Log("Healed");
            }
        }
        
        playerHealthBar.maxValue = playerMaxHealth;
        playerHealthBar.value = (int)playerCurrentHealth;
        moneyText.text = "Money: " + playerMoney;
        //dashCooldownText.text = "Dash Cooldown: " + Mathf.Max(0, dashCooldownTime).ToString("F1");
        // Don't need this anymore or the text in the values up top
        //healingCooldownText.text = "Healing Cooldown: " + Mathf.Max(0, nextHealingTime - Time.time).ToString("F1"); 
        //specialAttackCooldownText.text = "Special Attack Cooldown: " + Mathf.Max(0, nextSpecialAttackTime - Time.time).ToString("F1");
    }

    public void LateUpdate()
    {
        playerHealthBar.value = (int)playerCurrentHealth; // Hopefully this will update the health bar after new game is started
    }

    public void PlayerMoves()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical) * speed;
        rb.velocity = movement; // Use Rigidbody2D for movement

        float speedValue = movement.magnitude;
        playerAnimator.SetFloat("Movement", speedValue);
        playerSprite.flipX = Input.mousePosition.x < Screen.width / 2;
    }
    
    private IEnumerator Dash() // Changed based off feedback to instead of cursor the dash works with the Player direction
    {
        Vector2 movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        if (movementDirection == Vector2.zero)
        {
            yield break; // if there is no movement from the Player
        }

        isDashing = true;
        dashCooldownTime = dashCooldown;
        float startTime = Time.time;
        
        dashEffect.Play();

        while (Time.time < startTime + dashDuration)
        {
            transform.Translate(movementDirection * dashSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        
        dashEffect.Stop();

        isDashing = false;
    }

    public void BasicAttack() // Using Mouse0 to attack 
    {
        SFXManager.instance.PlayPlayerSFX(0);
        // Attack in the direction of the mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)transform.position).normalized;

        GameObject basicAttack = Instantiate(basicAttackPrefab, basicAttackPoint.position, Quaternion.identity);
        basicAttack.GetComponent<Rigidbody2D>().velocity = attackDirection * basicAttackSpeed;
        
        StartCoroutine(DestroyBasicAttackAfterRange(basicAttack, basicAttackPoint.position, basicAttackRange));
    }
    
    public void SpecialAttack() // Using Mouse1 to attack
    {
        if (!isSpecialAttackUnlocked)
        {
            Debug.Log("Special Attack is not unlocked yet.");
            return;
        }

        if (Time.time < nextSpecialAttackTime)
        {
            Debug.Log("Special Attack is on cooldown.");
            return;
        }

        // Attack in the direction of the mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)transform.position).normalized;
        
        SFXManager.instance.PlayPlayerSFX(2);
        GameObject specialAttack = Instantiate(specialAttackPrefab, specialAttackPoint.position, Quaternion.identity);
        Rigidbody2D specialAttackRb = specialAttack.GetComponent<Rigidbody2D>();

        if (specialAttackRb != null)
        {
            specialAttackRb.velocity = attackDirection * specialAttackSpeed;
            specialAttackRb.gravityScale = 0; // Ensure the special attack does not fall down
            specialAttackRb.isKinematic = true; // Ensure the special attack does not affect the player's physics
        }

        nextSpecialAttackTime = Time.time + specialAttackCooldown; // Set the next allowed special attack time
        StartCoroutine(DestorySpecialAttackAfterRange(specialAttack, specialAttackPoint.position, specialAttackRange));
    }
    
    private void Heal()
    {
        if (!isHealingUnlocked)
        {
            Debug.Log("Healing is not unlocked yet.");
            return;
        }

        if (Time.time < nextHealingTime)
        {
            Debug.Log("Healing is on cooldown.");
            return;
        }

        playerCurrentHealth += healingAmount;
        nextHealingTime = Time.time + healingCooldown;
        healingEffect.Play();
        SFXManager.instance.PlayPlayerSFX(4);
        StartCoroutine(StopHealingEffectAfterDuration(healingEffect.main.duration));
    }
    
    private IEnumerator DestroyBasicAttackAfterRange(GameObject basicAttack, Vector2 startPosition, float range)
    {
        while (basicAttack != null && Vector2.Distance(startPosition, basicAttack.transform.position) < range)
        {
            yield return null;
        }
        if (basicAttack != null)
        {
            Destroy(basicAttack);
        }
    }
    
    private IEnumerator DestorySpecialAttackAfterRange(GameObject specialAttack, Vector2 startPosition, float range)
    {
        while (specialAttack != null && Vector2.Distance(startPosition, specialAttack.transform.position) < range)
        {
            yield return null;
        }
        if (specialAttack != null)
        {
            Destroy(specialAttack);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
                enemyRb.AddForce(knockbackDirection * specialAttackSpeed, ForceMode2D.Impulse);
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        if (isInvincible) return; // If the player is invincible, do not take damage

        playerCurrentHealth -= damage;
        SFXManager.instance.PlayEnemySFX(0);

        if (playerCurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashPlayer());
            StartCoroutine(InvincibilityCoroutine());
        }
    }
    
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }
    
    void Die()
    {
        uiManager.SwitchUI(UIManager.switchUI.GameOver);
        SFXManager.instance.PlayPlayerSFX(5);
        playerSprite.enabled = false;
        Time.timeScale = 0;
        FindObjectOfType<GameManager>().SavingGame();
    }
    
    RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, LayerMask layer)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layer);
        //Physics.Raycast(origin, direction, distance, layer);
        return hit;
    }
    
    private IEnumerator FlashPlayer()
    {
        for (int i = 0; i < numberOfFlashes; i++)
        {
            playerSprite.color = new Color(1, 0, 0, 0.5f); // Red color with half transparency
            yield return new WaitForSeconds(flashDuration);
            playerSprite.color = Color.white; // Reset to original color
            yield return new WaitForSeconds(flashDuration);
        }
    }
    
    private IEnumerator StopHealingEffectAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        healingEffect.Stop();
    }
    
    public void AddMoney(int money)
    {
        playerMoney += money;
    }
    
    private IEnumerator PlayDashEffect()
    {
        dashEffect.Play();
        yield return new WaitForSeconds(0.5f);
        dashEffect.Stop();
    }
    
    private void CursorArrow()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - (Vector2)transform.position).normalized;
        arrowCursor.transform.position = transform.position + (Vector3)direction;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowCursor.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90)); // Adjust the angle to make the arrow point upwards
    }
}
