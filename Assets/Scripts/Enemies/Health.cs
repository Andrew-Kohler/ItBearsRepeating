using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 4;
    public float currentHealth; // Health is a float to allow a little more variety in DMG numbers, keep things interesting

    void Start()
    {
        currentHealth = maxHealth; 
    }

    public void TakeDamage(float HP)  // Decrease HP
    {
        currentHealth -= HP;
        if(currentHealth <= 0)
            Destroy(this.gameObject);
    }
    public void RestoreHealth(float HP)   // Restore HP (I don't think any enemies are going to have a healing factor, but I like to be safe)
    {
        currentHealth += HP;
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

}
