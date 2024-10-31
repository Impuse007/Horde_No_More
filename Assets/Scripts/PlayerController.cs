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
    
    // Player Movement Values
    [Header("Player Main Stats")]
    public Slider playerHealthBar;
    public float playerMaxHealth = 100;
    public float speed = 5.0f;
    [HideInInspector] public float playerCurrentHealth;
    
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
    public LayerMask enemyLayersSpecial;
    
    // Player Dash Values
    [Header("Player Dash Stats")]
    public float dashSpeed = 15.0f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 3.0f;
    public float playerInvincibilityTime = 1.0f;
    private float dashTime;
    private float dashCooldownTime; // Sets the cooldown time for the dash when the game is starting
    private bool isDashing;
    public TextMeshProUGUI dashCooldownText;
    
    // Player Flash Values
    [Header("Player Flash Stats")]
    public SpriteRenderer playerSprite;
    public int numberOfFlashes = 3;
    public float flashDuration = 0.1f;
    
    // Money Values
    [Header("Player Money Stats")]
    public int playerMoney = 0;
    public TextMeshProUGUI moneyText;
    
    [Header("Player Animations")]
    public Animator playerAnimator;
    
    [Header("Player Debugging")]
    public TMP_Text playerHealthText;
    
    [Header("Skill Manager")]
    public SkillTree skillTree;
    
    public void Awake()
    {
        playerCurrentHealth = playerMaxHealth;
        playerHealthBar.maxValue = playerMaxHealth;
        playerSprite = GetComponent<SpriteRenderer>();
    }
    
    public void Update()
    {
        PlayerMoves();
        playerHealthText.text = playerCurrentHealth + "/" + playerMaxHealth;
        
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTime <= 0)
        {
            StartCoroutine(Dash());
        }
        
        if (dashCooldownTime > 0)
        {
            dashCooldownTime -= Time.deltaTime;
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
                nextSpecialAttackTime = Time.time + 1f / specialAttackCooldown;
                Debug.Log("Special Attacked");
            }
        }

        playerHealthBar.value = (int)playerCurrentHealth;
        moneyText.text = "Money: " + playerMoney;
        dashCooldownText.text = "Dash Cooldown: " + dashCooldownTime.ToString("F1");
        
        //Debug.Log(dashCooldownTime);
    }
    
    public void PlayerMoves()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector2(horizontal, vertical);
        transform.Translate(movement * speed * Time.deltaTime);
        
        float speedValue = movement.magnitude;
        playerAnimator.SetFloat("Movement", speedValue);
        playerSprite.flipX = Input.mousePosition.x < Screen.width / 2;
    }
    
    private IEnumerator Dash() // Bugged as the Player can double dash when pressing space bar quickly enough.
    { 
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 dashDirection = (worldPosition - (Vector2)transform.position).normalized;
        
        isDashing = true;
        float startTime = Time.time;
        
        while (Time.time < startTime + dashDuration) // dashDuration could be bugged with the timer of the dash
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        
        isDashing = false;
        dashCooldownTime = dashCooldown;
    }

    public void BasicAttack() // Using Mouse0 to attack 
    {
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

        // Attack in the direction of the mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)transform.position).normalized;

        GameObject specialAttack = Instantiate(specialAttackPrefab, specialAttackPoint.position, Quaternion.identity);
        Rigidbody2D specialAttackRb = specialAttack.GetComponent<Rigidbody2D>();

        if (specialAttackRb != null)
        {
            specialAttackRb.velocity = attackDirection * specialAttackSpeed;
            specialAttackRb.gravityScale = 0; // Ensure the special attack does not fall down
            specialAttackRb.isKinematic = true; // Ensure the special attack does not affect the player's physics
        }

        StartCoroutine(DestorySpecialAttackAfterRange(specialAttack, specialAttackPoint.position, specialAttackRange));
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
        playerCurrentHealth -= damage;
        
        if (playerCurrentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(FlashPlayer());
        }
    }
    
    void Die()
    {
        uiManager.SwitchUI(UIManager.switchUI.GameOver);
        playerSprite.enabled = false;
        Time.timeScale = 0;
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
    
    public void AddMoney(int money)
    {
        playerMoney += money;
    }
}
