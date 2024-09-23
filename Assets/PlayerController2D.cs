using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    // Player Movement Variables
    [Header("Movement Variables")]
    public float moveSpeed = 5f;
    private Vector3 moveDirection;
    
    // Player Dash Variables
    [Header("Dash Variables")]
    public float dashSpeed = 10f;
    public float dashDistance = 5f;
    public float dashTime = 0.1f;
    public float dashCooldown = 1f;
    public float dashInvincibility = 0.5f;
    public bool dashInDirection = false;
    
    // Can you make the dash dash in the direction the player is facing? 
    
    
    
    
    
    public void Update()
    {
        MovePlayer();
        Dash();
    }
    
    public void MovePlayer()
    {
        // Player Movement
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveX, moveY, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed;
    }
    
    public void Dash()
    {
        // Player Dash
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DashMove());
        }
    }
    
    IEnumerator DashMove()
    {
        float dashTimeLeft = dashTime;
        float lastDash = Time.time;
        float dashCooldownLeft = dashCooldown;
        
        while (dashTimeLeft > 0)
        {
            // Player Dash in the direction they are facing
            if (dashInDirection)
            {
                moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
            }
            else
            {
                moveDirection = transform.forward;
            }
            
            transform.position += moveDirection * dashSpeed * Time.deltaTime;
            dashTimeLeft -= Time.deltaTime;
            
            // Dash Invincibility
            if (Time.time - lastDash >= dashInvincibility)
            {
                Physics2D.IgnoreLayerCollision(8, 9, false);
            }
            
            yield return null;
        }

        while (dashCooldownLeft > 0)
        {
            dashCooldownLeft -= Time.deltaTime;
            yield return null;
        }
    }
}
