using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirBounce : MonoBehaviour
{
    private PlayerMovement move;
    private PlayerAttack attack;
    private Rigidbody2D rb;

    [SerializeField] private float airBounceForce = 5f;

    private void Start()
    {
        move = GetComponentInParent<PlayerMovement>();      // Get Movement component
        attack = GetComponentInParent<PlayerAttack>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))   // If we hit an enemy
        {
            if (!move.IsGrounded && !move.IsAirDashing && !collision.gameObject.GetComponent<EnemyMovement>().IsGrounded)   // ...and if we aren't grounded or air dashing and the enemy isn't grounded
            {
                if (rb.velocity.y > 0) // If we're moving up, no need to negate downward v, just give a little push
                {
                    rb.AddForce(new Vector2(0, airBounceForce), ForceMode2D.Impulse);
                }
                else // If we're falling, we do need to negate downward v in addition to getting a little push
                {
                    rb.AddForce(new Vector2(0, Mathf.Abs(rb.velocity.y) + airBounceForce), ForceMode2D.Impulse);
                }

            }
        }

    }
}
