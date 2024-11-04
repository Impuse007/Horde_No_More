using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int waveNumber;
    public float waveDelay;
    public List<EnemySpawnInfo> enemySpawnInfos; // List of enemy spawn information
    public Transform[] spawnPoints;
    
    public int moneyDrop;
    public int enemyDamage;
    public int enemyHealth;
    public int enemySpeed;
    
    public int rangeDamage;
    public int rangeHealth;
    public int rangeSpeed;
    public int rangeMoneyDrop;
    
    public int bossDamage;
    public int bossHealth;
    public int bossSpeed;
    public int bossMoneyDrop;
    
    
    [System.Serializable]
    public class EnemySpawnInfo
    {
        public GameObject enemyPrefab;
        public int spawnCount;
    }
}
