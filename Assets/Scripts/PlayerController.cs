using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    // Player Components
    HealthManager playerManager;
    // Player Movement Values
    [Header("Player Main Stats")]
    [HideInInspector] public float playerCurrentHealth;
    public float playerMaxHealth = 100;
    public TextMeshProUGUI healthText;
    public float speed = 5.0f;
    
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
    
    public void Start()
    {
        playerCurrentHealth = playerMaxHealth;
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
        
        healthText.text = playerCurrentHealth.ToString();
        //Debug.Log(dashCooldownTime);
    }
    
    public void PlayerMoves()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector2(horizontal, vertical);
        transform.Translate(movement * speed * Time.deltaTime);
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
    }
    
    public void TakeDamage(int damage)
    {
        playerCurrentHealth -= damage;
        
        if (playerCurrentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
}
