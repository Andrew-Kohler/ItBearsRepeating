using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.gameObject.GetComponentInParent<EnemyAttack>().DamagePlayerMelee(collision);
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Uwu");
            //this.gameObject.GetComponentInParent<EnemyAttack>().DamagePlayerMelee(collision);
        }
        Debug.Log("Aha");
    }*/
    
}
