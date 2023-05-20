using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    //This is whether or not the player can actually move
    [SerializeField] private bool disabled = false;
    // The speed at which the player moves
    [SerializeField] private float Speed = 2.7f;
    // The multiplier on speed for sprinting
    [SerializeField] private float SprintMult = 2.5f;
    private float SpeedMult = 1f;
    // Acceleration of the player. Smaller number = faster
    [SerializeField] private float Acceleration = .05f;
    // This means that you're 1D character is facing left by default (1D means you only face left or right)")]
    [SerializeField] private bool SpriteFacingRight = true;

    [Header("Jump Variables")]
    //Controls whether your player can jump or not
    [SerializeField] private bool canJump = true;
    //The force of your jump (Be sure to have your gravity set to 1 for side-scroller
    [SerializeField] private float JumpForce = 7f;
    //Number of jumps your player can do each time they touch the ground. (2 = Double jump)
    [SerializeField] private int NumberOfJumps = 1;
    // The multiplier at which you fall down (used for smooth movement) and it can't be below 1
    [SerializeField] private float FallMultiplier = 3f;

    //The force of the air dash
    [Header("Air Dash Variables")]
    [SerializeField] private float dashPower = 2f;
    private bool canAirDash = true;
    private bool isDashing = false;  // This bear is always dashing <3

    [Header("Raycast Jumping Variables")]
    // Will show a red ray drawn from center of your sprite, it should extend from your box collider to touch the ground. If it doesn't reach the ground, change rayLength until it does. If you cannot see it, click the Gizmos button in the top right of the Game Window.
    [SerializeField] private bool ShowDebugRaycast = false;
    // Select your ground layer so that the raycast can detect it
    [SerializeField] private LayerMask groundLayer;
    // The length of the ray used to detect the ground.
    [SerializeField] private float rayLength = .5f;

    [Header("Misc Variables")]
    [SerializeField] private float timeBetweenIdles = 7f;
    private float idleCountdown;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer rend;
    private Animator anim;

    private float HorizontalMovement;
    private float VerticalMovement;
    private Vector2 lastLookDirection;

    private bool isGrounded = false;    // If the player is grounded
    public bool IsGrounded => isGrounded;   // Getter for isGrounded
    private bool sprint = false;          // If the player is dashing
    

    private Vector3 currentVelocity = Vector3.zero;
    private int jumpsLeft; // How many jumps until the player can't jump anymore? reset when grounded.

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); //Find the Rigidbody component on the gameobject this script is attached to.
        col = GetComponent<Collider2D>(); //Get Collider component
        rend = GetComponent<SpriteRenderer>(); //Get Sprite Renderer Component
        anim = GetComponent<Animator>(); //Get Animator Component
        //playerAudio = GetComponent<PlayerAudio>();

        idleCountdown = timeBetweenIdles;
    }

    // Update is called once per frame
    void Update()
    {
        //check if the player is grounded
        isGrounded = Physics2D.BoxCast(col.bounds.center, col.bounds.size, 0.0f, Vector2.down, rayLength, groundLayer);
        anim.SetBool("isGrounded", isGrounded);

        if (!disabled) //If player movement is NOT disabled
        {
            idleCountdown -= Time.deltaTime;    // The countdown that lets us know the player has been inactive long enough for an anim

            if (ShowDebugRaycast)
                Debug.DrawRay(col.bounds.center, Vector2.down * rayLength, Color.red); //draws a ray showing ray length

            //Animation
            //anim.SetFloat("MoveHorizontal", HorizontalMovement);
            if (HorizontalMovement != 0) // || VerticalMovement != 0)
            {
                idleCountdown = timeBetweenIdles;   // Reset our idle timer
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

            if (!GetComponent<PlayerCrouch>().IsCrouching)  // We do nothing when we crouch
            {
                //Get horizontal and vertical input. See Project Settings > Input Manager
                HorizontalMovement = Input.GetAxisRaw("Horizontal");

                if (canJump && Input.GetButtonDown("Jump") && isGrounded) //If the player jumps and is grounded
                {
                    idleCountdown = timeBetweenIdles;
                    Jump();
                }
                else if (canJump && Input.GetButtonDown("Jump") && NumberOfJumps > 1 && jumpsLeft > 1) //Or if the player has jumps left
                {
                    Jump();
                    jumpsLeft--;
                }

                if (canAirDash && Input.GetButtonDown("Slash") && !isGrounded && sprint) // If the player does the air dash
                {
                    StartCoroutine(DoAirDash());
                }

                // If they are grounded, reset their jumps and air dash
                if (isGrounded)
                {
                    jumpsLeft = NumberOfJumps;
                }

                // Check if the player is sprinting
                if (Input.GetButton("Sprint"))
                {
                    idleCountdown = timeBetweenIdles;
                    sprint = true;
                }     
                else
                    sprint = false;
                anim.SetBool("isRunning", sprint);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }

            if (idleCountdown <= 0)
            {
                Debug.Log("Sniff");
                StartCoroutine(DoIdleTwo());
                idleCountdown = timeBetweenIdles;
            }
            
        }

        
    }

    private void FixedUpdate()
    {
        MoveSideScroller(HorizontalMovement); // Move like a sidescroller 
    }

    // Public Methods ----------------------------------------
    public void DisablePlayer(bool isDisabled)  // Disables the player whenever we need them to chill
    {
        disabled = isDisabled;
        if (disabled)
        {
            rb.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }
    public Vector2 GetLastLookDirection()
    {
        return lastLookDirection;
    }

    // Private Methods ----------------------------------------

    private void MoveSideScroller(float move)
    {
        if (move != 0)
        {
            lastLookDirection = new Vector2(move, 0);
        }

        if (sprint)
            SpeedMult = SprintMult;
        else
            SpeedMult = 1;

        if (!isDashing && !GetComponent<PlayerCrouch>().IsCrouching)    // If we aren't dashing or crouching
        {
            Vector3 targetVelocity = new Vector3(move * Speed * SpeedMult, rb.velocity.y); //Make target velocity how we want to move.

            rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref currentVelocity, Acceleration); //Use smooth damp to simulate acceleration.
        }

        //If your sprite has an idle where they are facing to the side, then you may need to uncomment this :)
        FlipCheck(move);

        if (!isGrounded) //if the player is in the air
        {
            if (rb.velocity.y < 0) //if player is falling
            {
                //Make gravity harsher so they fall faster.
                rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) //if player is jumping and holding jump button
            {
                //Make gravity less so they jump higher. Creates variable jump heights.
                rb.velocity += Vector2.up * Physics2D.gravity.y * (FallMultiplier - 1.5f) * Time.deltaTime;
            }

        }
    }

    private void Jump()
    {
        idleCountdown = timeBetweenIdles;
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
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    // Coroutines -----------------------------------------------
    IEnumerator DoAirDash()
    {
        disabled = true;
        canAirDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;

        rb.gravityScale = .2f;
        rb.velocity = new Vector2(rb.velocity.x * dashPower, 0); //Stop any previous vertical movement
        ///yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => isGrounded);

        rb.gravityScale = originalGravity;
        disabled = false;
        canAirDash = true;  // Reset air dash
        isDashing = false;
        yield return null;
    }

    IEnumerator DoIdleTwo()    // Purely for playing and exiting Idle Animation 2
    {
        anim.Play("Idle2");
        yield return new WaitForSeconds(2.583f);
        anim.Play("Idle");
    }

}
