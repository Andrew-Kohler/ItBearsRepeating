using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    Health playerHealth;
    PlayerMovement playerMovement;
    PlayerCrouch crouch;
    DamageFlash damageFlash;

    private void OnEnable() // Listen for the oof ouch owie event
    {
        
    }

    private void OnDisable()
    {
        
    }

    void Start()
    {
        playerHealth = GetComponent<Health>();
        playerMovement = GetComponent<PlayerMovement>();
        crouch = GetComponent<PlayerCrouch>();
        damageFlash = GetComponentInChildren<DamageFlash>();
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
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
    }*/

    public void TakeDamage(GameObject dmgSource)
    {
        damageFlash.CallDamageFlash();
        // Accounting for melee and ranged sources of damage
        if (dmgSource.GetComponentInParent<EnemyAttack>() != null)
        {
            if (crouch.IsCrouching)
            {
                playerHealth.TakeDamage(dmgSource.GetComponentInParent<EnemyAttack>().DMG / 2);
            }
            else
            {
                playerHealth.TakeDamage(dmgSource.GetComponentInParent<EnemyAttack>().DMG);
                if (dmgSource.GetComponentInParent<EnemyAttack>().DMG >= 10)
                {
                    playerMovement.TakeKnockback(dmgSource.GetComponentInParent<EnemyAttack>().Knockback);
                }
            }
            
        }
        else if (dmgSource.GetComponent<Projectile>() != null)
        {
            if (crouch.IsCrouching)
            {
                playerHealth.TakeDamage(dmgSource.GetComponent<Projectile>().damageValue / 2);
            }
            else
            {
                playerHealth.TakeDamage(dmgSource.GetComponent<Projectile>().damageValue);
                if (dmgSource.GetComponent<Projectile>().damageValue >= 10)
                {
                    playerMovement.TakeKnockback(dmgSource.GetComponent<Projectile>().knockback);
                }
            }
            
            //Destroy(dmgSource);
        }
    }
}
