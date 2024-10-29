using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public int waveNumber;
    public List<GameObject> enemyPrefabs;
    public Transform[] spawnPoints;
    public float waveDelay;
    
    public int moneyDrop;
    public int enemyDamage;
    public int enemyHealth;
    public int enemySpeed;
}
