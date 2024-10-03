using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : EnemyBase
{
    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMeleeMovement();
    }
    
    public void EnemyMeleeAttack()
    {
        // Deal damage to the player
        return;
    }
    
    public void EnemyMeleeMovement()
    {
        enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, player.transform.position, speed * Time.deltaTime);
    }
    
    
}
