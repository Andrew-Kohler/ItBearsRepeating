using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    EnemyHealth enemyHealth;
    EnemyMovement enemyMovement;
    DamageFlash damageFlash;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponent<EnemyHealth>();  
        enemyMovement = GetComponent<EnemyMovement>();
        damageFlash = GetComponentInChildren<DamageFlash>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Hitbox"))   // If we touch a player-controlled hitbox
        {
            damageFlash.CallDamageFlash();
            // try get component on PlayerAttack or ItemAttack, get the appropriate damage and knock nums from those
            if(collision.gameObject.GetComponentInParent<PlayerAttack>() != null)
            {
                enemyHealth.TakeDamage(collision.gameObject.GetComponentInParent<PlayerAttack>().DMG);
                enemyMovement.TakeKnockback(collision.gameObject.GetComponentInParent<PlayerAttack>().Knockback);
            }
                
        }
    }
}