using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirBounce : MonoBehaviour
{
    private PlayerMovement move;
    private Rigidbody2D rb;
    private void Start()
    {
        move = GetComponentInParent<PlayerMovement>();      // Get Movement component
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // I ACCIDENTLY INVENTED A REALLY COOL MECHANIC, YOU CAN GO SO FAR BY AIR DASH BOUNCING
        // I have no idea how this will work on crowds, let's see
        // Ok, so this IS really cool
        // However, the point of the air dash is to throw the enemies backwards, not you forwards
        // So I am gonna have to nix it
        // If only there was some way to have it all, some kind of input that could be pressed or held to toggle how the air dash works that didn't detract from the game design
        // Because I NEED the air dash as a crowd control move
        // Ok, compromise is that the regular slashes are ALSO very silly in this regard
        // And I'm def keeping that
        // Hell yeah, advacned mechanics
        if (collision.gameObject.CompareTag("Enemy"))   // If we hit an enemy
        {
            if (!move.IsGrounded && !move.IsAirDashing)   // ...and if we aren't grounded
            {
                if (rb.velocity.y > 0) // If we're moving up, no need to negate downward v, just give a little push
                {
                    rb.AddForce(new Vector2(0, 5f), ForceMode2D.Impulse);
                }
                else // If we're falling, we do need to negate downward v in addition to getting a little push
                {
                    rb.AddForce(new Vector2(0, Mathf.Abs(rb.velocity.y) + 5f), ForceMode2D.Impulse);
                }


            }
        }
    }
}
