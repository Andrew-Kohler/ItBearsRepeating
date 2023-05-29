using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    Health playerHealth;
    PlayerMovement playerMovement;
    DamageFlash damageFlash;
    void Start()
    {
        playerHealth = GetComponent<Health>();
        playerMovement = GetComponent<PlayerMovement>();
        damageFlash = GetComponentInChildren<DamageFlash>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy Hitbox"))   // If we touch an enemy-controlled hitbox
        {
            damageFlash.CallDamageFlash();
            // Accounting for melee and ranged sources of damage
            if (collision.gameObject.GetComponentInParent<EnemyAttack>() != null)
            {
                playerHealth.TakeDamage(collision.gameObject.GetComponentInParent<EnemyAttack>().DMG);
                if(collision.gameObject.GetComponentInParent<EnemyAttack>().DMG >= 10)
                {
                    playerMovement.TakeKnockback(collision.gameObject.GetComponentInParent<EnemyAttack>().Knockback);
                }
            }
            else if(collision.gameObject.GetComponent<Projectile>() != null)
            {
                playerHealth.TakeDamage(collision.gameObject.GetComponent<Projectile>().damageValue);
                if (collision.gameObject.GetComponent<Projectile>().damageValue >= 10)
                {
                    playerMovement.TakeKnockback(collision.gameObject.GetComponent<Projectile>().knockback);
                }
                Destroy(collision.gameObject);
            }

        }
    }
}
