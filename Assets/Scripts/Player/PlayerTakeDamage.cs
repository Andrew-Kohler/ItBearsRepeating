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

    private void Update()
    {
        if(playerHealth.currentHealth <= 0)
        {
            Debug.Log("Take damage is down");
            enabled = false;
        }
    }

    public void TakeDamage(GameObject dmgSource)
    {
        damageFlash.CallDamageFlash();
        // Accounting for melee, ranged, and explosive sources of damage
        if (dmgSource.GetComponentInParent<EnemyAttack>() != null)
        {
            if (crouch.IsCrouching)
            {
                playerHealth.TakeDamage(dmgSource.GetComponentInParent<EnemyAttack>().DMG / 2);
            }
            else
            {
                
                if (dmgSource.GetComponentInParent<EnemyAttack>().DMG >= 10 && GameManager.Instance.isGameOver() == false)
                {
                    playerHealth.TakeDamageNoKillCheck(dmgSource.GetComponentInParent<EnemyAttack>().DMG);
                    playerMovement.TakeKnockback(dmgSource.GetComponentInParent<EnemyAttack>().Knockback);
                }
                else
                {
                    playerHealth.TakeDamage(dmgSource.GetComponentInParent<EnemyAttack>().DMG);
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
                if (dmgSource.GetComponent<Projectile>().damageValue >= 10 && GameManager.Instance.isGameOver() == false)
                {
                    playerHealth.TakeDamageNoKillCheck(dmgSource.GetComponent<Projectile>().damageValue);
                    playerMovement.TakeKnockback(dmgSource.GetComponent<Projectile>().knockback);
                }
                else
                {
                    playerHealth.TakeDamage(dmgSource.GetComponent<Projectile>().damageValue);
                }
            }
            
            //Destroy(dmgSource);
        }
        else if(dmgSource.GetComponent<Explosion>() != null)
        {
            if (crouch.IsCrouching)
            {
                playerHealth.TakeDamage(dmgSource.GetComponent<Explosion>().explosionDMG / 2);
            }
            else
            {
                
                if (dmgSource.GetComponent<Explosion>().explosionDMG >= 10 && GameManager.Instance.isGameOver() == false)
                {
                    playerHealth.TakeDamageNoKillCheck(dmgSource.GetComponent<Explosion>().explosionDMG);
                    playerMovement.TakeKnockback(dmgSource.GetComponent<Explosion>().explosionKnockback);
                }
                else
                {
                    playerHealth.TakeDamage(dmgSource.GetComponent<Explosion>().explosionDMG);
                }
            }
        }
    }
}
