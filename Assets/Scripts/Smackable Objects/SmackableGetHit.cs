using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmackableGetHit : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D col;

    DamageFlash damageFlash;
    Health smackableHealth;
    SmackableAnim smackableAnim;

    [Header("Raycast Collision Check Variables")]
    // Will show a red ray drawn from center of your sprite, it should extend from your box collider to touch the ground. If it doesn't reach the ground, change rayLength until it does. If you cannot see it, click the Gizmos button in the top right of the Game Window.
    [SerializeField] private bool ShowDebugRaycast = false;
    // Select your ground layer so that the raycast can detect it
    [SerializeField] private LayerMask groundLayer;
    // The length of the ray used to detect the ground.
    [SerializeField] private float rayLength = .5f;

    public bool left;  // Contact booleans for the different sides
    public bool right;
    public bool top;
    public bool bottom;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Find the Rigidbody component on the gameobject this script is attached to.
        col = GetComponent<Collider2D>(); //Get Collider component

        smackableHealth = GetComponent<Health>();
        damageFlash = GetComponentInChildren<DamageFlash>();
        smackableAnim = GetComponentInChildren<SmackableAnim>();

    }

    private void Update()
    {
        bottom = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0.0f, Vector2.down, rayLength, groundLayer);
        top = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0.0f, Vector2.up, rayLength, groundLayer);
        left = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0.0f, Vector2.left, rayLength, groundLayer);
        right = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0.0f, Vector2.right, rayLength, groundLayer);

        if (ShowDebugRaycast)
        {
            Debug.DrawRay(col.bounds.center, Vector2.down * rayLength, Color.red); //draws a ray showing ray length
            Debug.DrawRay(col.bounds.center, Vector2.up * rayLength, Color.blue); //draws a ray showing ray length
            Debug.DrawRay(col.bounds.center, Vector2.left * rayLength, Color.green); //draws a ray showing ray length
            Debug.DrawRay(col.bounds.center, Vector2.right * rayLength, Color.red); //draws a ray showing ray length
        }
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {  
        if (collision.CompareTag("Player Hitbox"))   // If we touch a player-controlled hitbox
        {
            gameObject.tag = "Player Hitbox";
            gameObject.layer = LayerMask.NameToLayer("Smackable");
            damageFlash.CallDamageFlash();
            if (GetHitDirection(collision))
            {
                smackableAnim.changeRotationSpeed(1);
            }
            else
            {
                smackableAnim.changeRotationSpeed(2);
            }
            // try get component on PlayerAttack or EnemyAttack, get the appropriate damage and knock nums from those
            if (collision.gameObject.GetComponentInParent<PlayerAttack>() != null)
            {
                smackableHealth.TakeDamage(1);//collision.gameObject.GetComponentInParent<PlayerAttack>().DMG);
                TakeKnockback(collision.gameObject.GetComponentInParent<PlayerAttack>().Knockback);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (this.gameObject.tag == "Player Hitbox") // If the object is now a threat
        {
            if (collision.gameObject.CompareTag("Terrain"))
            {
                smackableHealth.TakeDamage(1); // Whenever we hit terrain, take a point of damage
                if(((left) || (right)) && !bottom)
                {
                    smackableAnim.swapRotationDirection();
                    //Bounce(true);
                }
                /*else if (top || bottom)
                {
                   // Bounce(false);
                }*/
            }
            else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Breakable"))
            {
                smackableHealth.TakeDamage(99); // If we hit an enemy or breakable rn, auto-break
            }
        }
    }

    public void TakeKnockback(Vector3 knockback)
    {
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(knockback * 4f, ForceMode2D.Impulse);
    }

    private bool GetHitDirection(Collider2D collision)
    {
        bool right = false;
        if (collision.gameObject.transform.position.x < gameObject.transform.position.x)
        {
            right = true;
        }
        return right;

    }
}
