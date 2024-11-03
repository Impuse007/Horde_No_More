using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyRangeArrow : MonoBehaviour
    {
        public int arrowDamage; // Set the damage value for the arrow

        void OnTriggerEnter2D(Collider2D collision)
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(arrowDamage);
                Destroy(gameObject); // Destroy the arrow after it hits the player
            }
        }
    }
}