using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 4;
    public float currentHealth; // Health is a float to allow a little more variety in DMG numbers, keep things interesting

    [SerializeField] private ParticleSystem healFX; // For bear only, healing

    public delegate void OnPlayerDamage();
    public static event OnPlayerDamage onPlayerDamage;

    void Start()
    {
        currentHealth = maxHealth; 
    }

    public void TakeDamage(float HP)  // Decrease HP
    {
        currentHealth -= HP;
        if (GetComponent<PlayerMovement>() != null)
        {
            onPlayerDamage?.Invoke();
        }

        if (currentHealth <= 0)
        {
            if (GetComponent<PlayerMovement>() != null)
            {
                /*if (!GetComponent<PlayerMovement>().hitstun)
                {*/
                Debug.Log("Health is down");
                GetComponent<PlayerMovement>().Kill();
                enabled = false;
                //}
            }
            else if (GetComponent<EnemyMovement>() != null)
            {

                GetComponent<EnemyMovement>().Kill();
                this.enabled = false;
            }
            else if (GetComponent<TreeTakeDamage>() != null)
            {
                GetComponent<TreeTakeDamage>().Topple();
            }
            else if (GetComponent<ObjectTakeDamage>() != null && this.gameObject.name == "Time Machine")
            {
                GetComponent<ObjectTakeDamage>().BlowUpTimeMachine();
            }
            else if (GetComponent<SmackableGetHit>() != null)
            {
                if (!GetComponent<SmackableGetHit>().explodes)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
            
    }

    public void TakeDamageNoKillCheck(float HP)
    {
        currentHealth -= HP;
        if (GetComponent<PlayerMovement>() != null)
        {
            onPlayerDamage?.Invoke();
        }
    }

    public void RestoreHealth(float HP)   // Restore HP (I don't think any enemies are going to have a healing factor, but I like to be safe)
    {
        healFX.Play();
        currentHealth += HP;
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

}
