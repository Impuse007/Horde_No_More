using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player Components
    HealthManager playerManager;
    // Player Movement Values
    public float speed = 5.0f;
    
    // Player Dash Values
    public float dashSpeed = 15.0f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 3.0f;
    public float playerInvincibilityTime = 1.0f;
    private float dashTime;
    private float dashCooldownTime;
    private bool isDashing;
    
    
    // Player Health Values
    public int maxHealth = 100;
    
    // Player Attack Values
    public int damage = 10;
    
    public void Start()
    {
        //playerManager.currentHealth = maxHealth;
    }
    
    public void Update()
    {
        PlayerMoves();
        BasicAttack();
        
        if (Input.GetKeyDown(KeyCode.Space) && dashCooldownTime <= 0)
        {
            StartCoroutine(Dash());
        }
        
        if (dashCooldownTime > 0)
        {
            dashCooldownTime -= Time.deltaTime;
        }
        
        Debug.Log(dashCooldownTime);
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Deal damage to the enemy
            playerManager.TakeDamage(damage);
        }
    }
}
