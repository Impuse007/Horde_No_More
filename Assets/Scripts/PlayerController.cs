using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Player Components
    HealthManager playerManager;
    public LevelManager levelManager; // This is not used in this script, Debugging purposes
    // Player Movement Values
    [Header("Player Main Stats")]
    public Slider playerHealthBar;
    public float playerMaxHealth = 100;
    public float speed = 5.0f;
    [HideInInspector] public float playerCurrentHealth;
    
    // Player Attack Values
    [Header("Player Attack Stats")]
    public Transform attackPoint;
    public int playerDamage = 10;
    public float basicAttackRange = 1.0f;
    public float basicAttackCooldown = 1.0f;
    public float nextAttackTime = 0.0f;
    public LayerMask enemyLayers;
    
    // Player Dash Values
    [Header("Player Dash Stats")]
    public float dashSpeed = 15.0f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 3.0f;
    public float playerInvincibilityTime = 1.0f;
    private float dashTime;
    private float dashCooldownTime;
    private bool isDashing;
    
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
    
    public void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        playerHealthBar.maxValue = playerMaxHealth;
        playerSprite = GetComponent<SpriteRenderer>();
    }
    
    public void Update()
    {
        PlayerMoves();
        
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

        playerHealthBar.value = (int)playerCurrentHealth;
        moneyText.text = "Money: " + playerMoney;
        
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
    
    private IEnumerator Dash()
    { 
        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 dashDirection = (worldPosition - (Vector2)transform.position).normalized;
        
        isDashing = true;
        float startTime = Time.time;
        
        while (Time.time < startTime + dashDuration)
        {
            transform.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
            yield return null;
        }
        
        isDashing = false;
        dashCooldownTime = dashCooldown;
    }

    public void BasicAttack()
    {
        // Attack in the direction of the mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 attackDirection = (mousePosition - (Vector2)transform.position).normalized;
        
        RaycastHit2D hit = Physics2D.Raycast(attackPoint.position, attackDirection, basicAttackRange, enemyLayers);
        Debug.DrawRay(attackPoint.position, attackDirection * basicAttackRange, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        {
            hit.collider.GetComponent<EnemyBase>().TakeDamage(playerDamage);
            Debug.Log("Hit enemy: " + hit.collider.name);
        }
        
        playerAnimator.SetTrigger("Attack");
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
        Destroy(gameObject);
        levelManager.MainMenu();
        Debug.Log(SceneManager.GetActiveScene().name);
    }
    
    RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance, LayerMask layer)
    {
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, distance, layer);
        Physics.Raycast(origin, direction, distance, layer);
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
