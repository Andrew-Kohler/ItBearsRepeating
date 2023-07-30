using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakeDamage : MonoBehaviour
{
    Health enemyHealth;
    EnemyMovement enemyMovement;
    DamageFlash damageFlash;
    ParticleSystem sparks;

    [SerializeField] private AudioClip hurtSound1;
    [SerializeField] private AudioClip hurtSound2;
    [SerializeField] private AudioClip hurtSound3;
    AudioSource audioS;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = GetComponent<Health>();  
        enemyMovement = GetComponent<EnemyMovement>();
        damageFlash = GetComponentInChildren<DamageFlash>();
        sparks = GetComponentInChildren<ParticleSystem>();  
        audioS = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Hitbox"))   // If we touch a player-controlled hitbox
        {
            damageFlash.CallDamageFlash();
            // try get component on PlayerAttack or ItemAttack, get the appropriate damage and knock nums from those
            if(collision.gameObject.GetComponentInParent<PlayerAttack>() != null)
            {
                sparks.Play();
                playHurtSound();
                enemyHealth.TakeDamage(collision.gameObject.GetComponentInParent<PlayerAttack>().DMG);
                if(enemyHealth.currentHealth <= 0)
                {
                    enemyMovement.TakeKnockback(collision.gameObject.GetComponentInParent<PlayerAttack>().Knockback * 2);
                    this.enabled = false;
                }
                else
                {
                    enemyMovement.TakeKnockback(collision.gameObject.GetComponentInParent<PlayerAttack>().Knockback);
                }
                
            }

        }
       else if(collision.CompareTag("Neutral Hitbox"))
        {
            damageFlash.CallDamageFlash();
            // try get component on Explosion
            if (collision.gameObject.GetComponentInParent<Explosion>() != null)
            {
                sparks.Play();
                playHurtSound();
                enemyHealth.TakeDamage(collision.gameObject.GetComponentInParent<Explosion>().explosionDMG);
                if (enemyHealth.currentHealth <= 0)
                {
                    enemyMovement.TakeKnockback(collision.gameObject.GetComponentInParent<Explosion>().explosionKnockback * 2);
                    this.enabled = false;
                }
                else
                {
                    enemyMovement.TakeKnockback(collision.gameObject.GetComponentInParent<Explosion>().explosionKnockback);
                }
                    
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player Hitbox"))   // If we touch a player-controlled smackable hitbox
        {
            sparks.Play();
            playHurtSound();
            damageFlash.CallDamageFlash();
            if (collision.gameObject.GetComponent<SmackableAttack>() != null)
            {
                enemyHealth.TakeDamage(collision.gameObject.GetComponent<SmackableAttack>().DMG);
                if(enemyHealth.currentHealth <= 0)
                {
                    enemyMovement.TakeKnockback(collision.gameObject.GetComponent<SmackableAttack>().Knockback * 2);
                    this.enabled = false;
                }
                else
                {
                    enemyMovement.TakeKnockback(collision.gameObject.GetComponent<SmackableAttack>().Knockback);
                }
                
            }
        }
    }

    private void playHurtSound()
    {
        int rand = Random.Range(0, 3);
        if(rand == 0)
        {
            audioS.PlayOneShot(hurtSound1);
        }
        else if (rand == 1)
        {
            audioS.PlayOneShot(hurtSound2);
        }
        else
        {
            audioS.PlayOneShot(hurtSound3);
        }
    }
}
