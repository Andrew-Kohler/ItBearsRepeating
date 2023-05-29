using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum EnemyType { Sasha, Melee, Laser, Rocket};    // Determines movement behavior
    [Header("Movement Variables")]
    //This is whether or not the enemy is allowed to move
    [SerializeField] private bool disabled = false;
    public bool Disabled => disabled;
    // The speed at which the enemy moves
    [SerializeField] private float Speed = 2.7f;
    // Acceleration of the enemy. Smaller number = faster
    [SerializeField] private float Acceleration = .05f;
    // This means that you're 1D character is facing left by default (1D means you only face left or right)")]
    [SerializeField] private bool SpriteFacingRight = true;
    public bool RightFacing => SpriteFacingRight;

    [Header("Jump Variables")]
    [SerializeField] private bool isGrounded = false;    // If the player is grounded
    [SerializeField] private bool nearLedge = false;    // If the player is close enough to a ledge that jumping is warranted
    //Controls whether your player can jump or not
    [SerializeField] private bool canJump = true;
    //The force of your jump (Be sure to have your gravity set to 1 for side-scroller
    [SerializeField] private float JumpForce = 7f;
    //Number of jumps the enemy can do each time they touch the ground. (2 = Double jump)
    [SerializeField] private int NumberOfJumps = 1;
    // The multiplier at which you fall down (used for smooth movement) and it can't be below 1
    [SerializeField] private float FallMultiplier = 3f;

    [Header("Raycast Jumping Variables")]
    // Will show a red ray drawn from center of your sprite, it should extend from your box collider to touch the ground. If it doesn't reach the ground, change rayLength until it does. If you cannot see it, click the Gizmos button in the top right of the Game Window.
    [SerializeField] private bool ShowDebugRaycast = false;
    // Select your ground layer so that the raycast can detect it
    [SerializeField] private LayerMask groundLayer;
    // The length of the ray used to detect the ground.
    [SerializeField] private float rayLength = .5f;
    // The length of the ray used to detect a ledge
    [SerializeField] private float ledgeRayLength = 1f;

    [Header("Enemy-Specific Variables")]
    [SerializeField] private EnemyType enemyType = EnemyType.Melee;
    public EnemyType typeOfEnemy => enemyType;
    [SerializeField] private bool act = false;          // Whether the enemy's AI is enabled
    [SerializeField] private bool attack = false;       // Whether the enemy is attacking or not (determined by proximity to player)
    public bool isAttack => attack; 
    [SerializeField] private float DistanceToStartActing = 10f; // How close the player should be to trigger the enemy to start doing something
    [SerializeField] private float DistanceToApproachTo = 2f;   // How close the enemy should get to the player before attacking

    private float HorizontalMovement;
    private Vector2 lastLookDirection;

    PhysicsMaterial2D pMaterial;
    [SerializeField] PhysicsMaterial2D pMaterialBouncy;


    private Vector3 currentVelocity = Vector3.zero;

    private float jumpCooldown;
    private float jumpCooldownTime = .6f;   // To ensure that nobody gets stuck in an endless cycle of half-jumps

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer rend;
    private Animator anim;

    [SerializeField] private GameObject enemySprite;    // The enemy sprite
    private GameObject bear;    // The player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Find the Rigidbody component on the gameobject this script is attached to.
        col = GetComponent<Collider2D>(); //Get Collider component
        pMaterial = col.sharedMaterial;

        rend = enemySprite.GetComponent<SpriteRenderer>(); //Get Sprite Renderer Component
        anim = enemySprite.GetComponent<Animator>(); //Get Animator Component

        bear = GameObject.Find("Bear");   // Find the player (distance calcs will be important here)
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the enemy is grounded
        isGrounded = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0.0f, Vector2.down, rayLength, groundLayer);
        anim.SetBool("isGrounded", isGrounded);

        if (ShowDebugRaycast)
        {
            Debug.DrawRay(col.bounds.center, Vector2.down * rayLength, Color.red); // Draw the rays we're working with
            Debug.DrawRay(col.bounds.center, Vector2.left * ledgeRayLength, Color.green); 
            Debug.DrawRay(col.bounds.center, Vector2.right * ledgeRayLength, Color.blue); 
        }

           

        if (!disabled) //If player movement is NOT disabled
        {
            // If the player is in range and act is false
            //Debug.Log(Vector2.Distance(transform.position, bear.transform.position));
            if (!act && Vector2.Distance(transform.position, bear.transform.position) < DistanceToStartActing)
            {
                // Act is now true
                // This is what starts the enemy moving - once the player has come in range, enemies currently don't ever stop chasing
                act = true;
            }


            if (act) // If act is true
            {
                if (HorizontalMovement != 0) // || VerticalMovement != 0)
                {
                    anim.SetBool("isMovingH", true);
                    /*if (playerAudio && !playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
                    {
                        playerAudio.WalkSource.Play();
                    }*/
                }
                else
                {
                    anim.SetBool("isMovingH", false);
                    /*if (playerAudio && playerAudio.WalkSource.isPlaying && playerAudio.WalkSource.clip != null)
                    {
                        playerAudio.WalkSource.Stop();
                    }*/
                }

                if (Vector2.Distance(transform.position, bear.transform.position) > DistanceToApproachTo) // If move we still must
                {
                    jumpCooldown -= Time.deltaTime;
                    if (transform.position.x > bear.transform.position.x)    // Finding which direction to move
                    {
                        HorizontalMovement = -1;    // If we're right of the player
                    }
                    else
                    {
                        HorizontalMovement = 1; // If we're left of the player
                    }

                    if (SpriteFacingRight)
                    {
                        nearLedge = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0.0f, Vector2.right, ledgeRayLength, groundLayer);
                    }
                    else
                    {
                        nearLedge = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0.0f, Vector2.left, ledgeRayLength, groundLayer);
                    }
                   
                    if (canJump && isGrounded && nearLedge && jumpCooldown <= 0) //If the player jumps and is grounded
                    {
                        jumpCooldown = jumpCooldownTime;
                        isGrounded = false;
                        Jump();
                    }
                }
                else
                {
                    HorizontalMovement = 0;
                    attack = true;
                }

                

            }
        } // End of not-disabled behavior
        


    }

    private void FixedUpdate()
    {
        if (act && enemyType != EnemyType.Sasha)  // If act is true
        {
            MoveSideScroller(HorizontalMovement);
        }

    }

    // Public methods ----------------------------------------
    public void DisableEnemy(bool isDisabled)  // Disables the enemy movement (may prove useful for applying hitstun)
    {
        disabled = isDisabled;
        if (disabled)
        {
            HorizontalMovement = 0;
            rb.velocity = Vector2.zero;
            anim.SetBool("isMovingH", false);
        }
    }
    public void TakeKnockback(Vector3 knockback)
    {
        
        DisableEnemy(true);
        rb.AddForce(knockback, ForceMode2D.Impulse);
        StartCoroutine(DoHitstun(.5f));

    }

    // Private methods ----------------------------------------

    private void MoveSideScroller(float move)
    {
        if (move != 0)
        {
            lastLookDirection = new Vector2(move, 0);
        }

        if (!disabled)
        {
            Vector3 targetVelocity = new Vector3(move * Speed, rb.velocity.y); //Make target velocity how we want to move.
            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, Acceleration); //Use smooth damp to simulate acceleration.
        }
        
        FlipCheck(move);

        if (!isGrounded) //if the player is in the air
        {
            //Debug.Log(playerSprite.transform.rotation.eulerAngles.z);
            if (rb.velocity.y <= 0) //if player is falling
            {
                //Make gravity harsher so they fall faster.
                rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;

            }
            else if (rb.velocity.y > 0)//  && !Input.GetButton("Jump")) //if player is jumping and holding jump button
            {
            }

        }

    }


    private void Jump()
    {
        Debug.Log("Jump");
        rb.velocity = new Vector2(rb.velocity.x, 0); //Stop any previous vertical movement
        rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse); //Add force upwards as an impulse.
        //if (playerAudio && playerAudio != null)
        //{
        //  playerAudio.JumpSource.Play();
        //}
    }
    private void FlipCheck(float move)
    {
        //Flip the sprite so that they are facing the correct way when moving
        if (move > 0 && !SpriteFacingRight) //if moving to the right and the sprite is not facing the right.
        {
            Flip();
        }
        else if (move < 0 && SpriteFacingRight) //if moving to the left and the sprite is facing right
        {
            Flip();
        }
    }

    private void Flip()
    {
        SpriteFacingRight = !SpriteFacingRight; //flip whether the sprite is facing right
        anim.SetBool("isFacingRight", SpriteFacingRight);
        /*if (rend.flipX)
        {
            rend.flipX = false;
        }
        else
        {
            rend.flipX = true;
        }*/
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    // Coroutines ----------------------------------------

    IEnumerator DoHitstun(float stunVal)    // Hitstun lasts for a certain amt of time, but is only broken once the enemy is grounded again
    {
        col.sharedMaterial = pMaterialBouncy;
        yield return new WaitForSeconds(stunVal);
        yield return new WaitUntil(() => isGrounded);
        col.sharedMaterial = pMaterial;
        DisableEnemy(false);
    }

    //TODO
    //...actually, freezing the enemy until the combo is done wouldn't be fun at all. nevermind.
    // The enemies shouldn't be moving the beach ball about


}
