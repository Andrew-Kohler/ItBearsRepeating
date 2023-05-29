using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject slashbox;
    [SerializeField] GameObject playerSprite;

    private Collider2D slashCol;
    private SpriteRenderer slashRend;
    private Animator anim;
    private Animator slashAnim;
    private PlayerMovement move;

    // Knockback is passed in the form of a vector that's used to apply a force

    [Header("Slash 1 Stats")]
    [SerializeField] private float slash1GroundDMG = 1f;
    [SerializeField] private float slash1AirDMG = 1f;
    [SerializeField] private Vector3 slash1GroundKnockback = new Vector3(1f, 1f, 0f);
    [SerializeField] private Vector3 slash1AirKnockback = new Vector3(1f, 1f, 0f);
    [Header("Slash 2 Stats")]
    [SerializeField] private float slash2GroundDMG = 1f;
    [SerializeField] private float slash2AirDMG = 1f;
    [SerializeField] private Vector3 slash2GroundKnockback = new Vector3(1f, 1f, 0f);
    [SerializeField] private Vector3 slash2AirKnockback = new Vector3(1f, 1f, 0f);
    [Header("Slash 3 Stats")]
    [SerializeField] private float slash3GroundDMG = 1f;
    [SerializeField] private float slash3AirDMG = 1f;
    [SerializeField] private Vector3 slash3GroundKnockback = new Vector3(1f, 1f, 0f);
    [SerializeField] private Vector3 slash3AirKnockback = new Vector3(1f, 1f, 0f);
    [Header("Air Dash Stats")]
    [SerializeField] private float airDashDMG = 1f;
    [SerializeField] private Vector3 airDashKnockback = new Vector3(1f, 1f, 0f);

    private float currentDMG;           // These are the values that we can change internally in this class and then pass along to other classes that need them
    private Vector3 currentKnockback;
    public float DMG => currentDMG;
    public Vector3 Knockback => currentKnockback;

    private bool activeCoroutine;
    public bool IsSlashing => activeCoroutine;  // Used to halt grounded movement in PlayerMovement (grounded slashes mean you aren't moving)

    private bool slash1Active;  // Booleans that let us know which slash is active
    private bool slash2Active;
    private bool slash3Active;
    private bool airDashActive; 
    public bool IsAirDashActive => airDashActive;

    private bool slash1Done;
    private bool slash2Done;
    private bool slash3Done;

    private bool reset; // Helps to trigger the reset when the combo window expires

    private bool onCooldown;    // If we are on cooldown and should not be allowed to slash

    [SerializeField] private float comboWindowTime = 1f; // The amount of time you have before the combo is reset
    [SerializeField] private float resetCountdown = 0f;      // Acts as a timer

    void Start()
    {
        playerSprite = GameObject.Find("Bear Sprite");

        slashCol = slashbox.GetComponent<Collider2D>();      //Get slashbox collider component
        slashRend = slashbox.GetComponent<SpriteRenderer>(); //Get slashbox sprite renderer Component
        slashAnim = slashbox.GetComponent<Animator>();      // Get slashbox animator component
        anim = playerSprite.GetComponent<Animator>();            //Get Animator component

        move = GetComponent<PlayerMovement>();      // Get Movement component

        slashCol.enabled = false;
        airDashActive = false;

        // Have another script on the slashbox that deals with what happens when someone is within the collider
    }

    // Update is called once per frame
    void Update()
    {
        if(resetCountdown > 0f) // Tick the combo reset timer down
        {
            resetCountdown -= Time.deltaTime;
        }
        else if(resetCountdown <= 0f && !reset) // If it goes all the way, reset the combo!
        {
            reset = true;
            StartCoroutine(DoPostComboDelay());
        }
        
        if (!GetComponent<PlayerCrouch>().IsCrouching && !move.Hitstun)  // We do nothing when we crouch, and especially not when disabled
        {
            if (Input.GetButtonDown("Slash") && !onCooldown && !airDashActive)    // If we try to slash and aren't on cooldown
            {
                if (move.IsAirDashing) // If the player does the air dash
                {
                    StartCoroutine(DoAirDashAttack());
                }
                else
                {
                    if (!slash1Active && !slash1Done)                  // If slash 1 isn't active
                    {
                        StartCoroutine(DoSlash1());
                    }
                    else if (slash1Done && !slash2Active && !slash2Done && resetCountdown > 0)    // If slash 2 isn't active, slash 1 is done, and we're inside our window
                    {
                        StartCoroutine(DoSlash2());
                    }
                    else if (slash2Done && !slash3Active && resetCountdown > 0)    // If slash 3 isn't active, slash 2 is done, and we're inside our window
                    {
                        StartCoroutine(DoSlash3());
                    }
                }         

            }
            else if (!activeCoroutine)  // If we aren't slashing
            {
                slashCol.enabled = false;
                slashRend.color = Color.clear;
            }
            else if (airDashActive)
            {
                playerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    // Coroutines ---------------------------------------
    IEnumerator DoSlash1()
    {
        activeCoroutine = true;
        slash1Active = true;

        if (move.IsGrounded)
        {
            currentDMG = slash1GroundDMG;
            currentKnockback = slash1GroundKnockback;
        }
        else
        {
            currentDMG = slash1AirDMG;
            currentKnockback = slash1AirKnockback;
        }

        if (!move.RightFacing)  // Account for direction
        {
            currentKnockback.x = currentKnockback.x * -1;
        }

        //Debug.Log("Slash 1");

        slashCol.enabled = true;
        slashRend.color = Color.white;
        slashAnim.Play("Slash1", 0, 0);
        if (move.IsGrounded)    // Play correct bear animation
        {
            anim.Play("Slash1Ground", 0, 0);
        }
        else
        {
            anim.Play("Slash1Air", 0, 0);
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, 1);
        }
        
        yield return new WaitForSeconds(.2f);

        if (move.IsGrounded)
        {
            anim.Play("Idle");
        }
        else
        {
            anim.Play("Jump");
        }
        activeCoroutine = false;
        slash1Active = false;
        slash1Done = true;

        resetCountdown = comboWindowTime; 
        reset = false;

        yield return null;
    }

    IEnumerator DoSlash2()
    {
        activeCoroutine = true;
        slash2Active = true;

        if (move.IsGrounded)
        {
            currentDMG = slash2GroundDMG;
            currentKnockback = slash2GroundKnockback;
        }
        else
        {
            currentDMG = slash2AirDMG;
            currentKnockback = slash2AirKnockback;
        }

        if (!move.RightFacing)  // Account for direction
        {
            currentKnockback.x = currentKnockback.x * -1;
        }

        //Debug.Log("Slash 2");
        slashCol.enabled = true;
        slashRend.color = Color.white;
        slashAnim.Play("Slash2");
        if (move.IsGrounded)    // Play correct bear animation
        {
            anim.Play("Slash2Ground", 0, 0);
        }
        else
        {
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, 1);
        }
        yield return new WaitForSeconds(.2f);

        if (move.IsGrounded)
        {
            anim.Play("Idle");
        }
        else
        {
            anim.Play("Jump");
        }

        activeCoroutine = false;
        slash2Active = false;
        slash2Done = true;

        resetCountdown = comboWindowTime;
        reset = false;

        yield return null;
    }

    IEnumerator DoSlash3()
    {
        activeCoroutine = true;
        slash3Active = true;

        if (move.IsGrounded)    // Setting DMG and Knockback
        {
            currentDMG = slash3GroundDMG;
            currentKnockback = slash3GroundKnockback;
        }
        else
        {
            currentDMG = slash3AirDMG;
            currentKnockback = slash3AirKnockback;
        }

        if (!move.RightFacing)  // Account for direction
        {
            currentKnockback.x = currentKnockback.x * -1;
        }

        if (move.IsGrounded)
        {
            anim.Play("Slash3Ground", 0, 0);
            yield return new WaitForSeconds(.5f);
        }
        

        //Debug.Log("Slash 3");
        slashCol.enabled = true;
        slashRend.color = Color.white;
        slashAnim.Play("Slash3");
        if (move.IsGrounded)    // Play correct bear animation
        {
            //anim.Play("Slash1Ground", 0, 0);
        }
        else
        {
            playerSprite.transform.rotation = Quaternion.Euler(0, 0, 1);
        }
        yield return new WaitForSeconds(.3f);

        if (move.IsGrounded)
        {
            anim.Play("Idle");
        }
        else
        {
            anim.Play("Jump");
        }

        activeCoroutine = false;
        slash3Active = false;
        slash3Done = true;
        // Once this is done, reset all the bools and put a cooldown timer on
        StartCoroutine(DoPostComboDelay());
    }

    IEnumerator DoPostComboDelay()
    {
        onCooldown = true;
        slash1Done = false;
        slash2Done = false;
        slash3Done = false;
        slashCol.enabled = false;
        yield return new WaitForSeconds(.1f);
        onCooldown = false;
        yield return null;
    }

    IEnumerator DoAirDashAttack()
    {
        activeCoroutine = true; // IT"S SOMETHING TO DO WITH HAVING THE COROUTINE ACTIVE FOR SO LONG
        // That's it. That's gotta be it. When slashing, I screw with some of the grav stuff
        // As long as I wasn't screwing with it for too long, it didn't matter
        // But since I'm slashing for so long, we are getting FUNKADELIC
        // Keeping this comment until I'm done tuning and retooling so I know what's up
        airDashActive = true;

        currentDMG = airDashDMG;
        currentKnockback = airDashKnockback;

        if (!move.RightFacing)  // Account for direction
        {
            currentKnockback.x = currentKnockback.x * -1;
        }

        //Debug.Log("Air Dash");
        slashCol.enabled = true;
        slashRend.color = Color.white;
        slashAnim.Play("AirDash", 0, 0);
        anim.Play("SlashAirDash", 0, 0);
        //yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => !move.IsAirDashing);  // We keep going until we hit the dirt

        anim.Play("Idle", 0, 0);
        activeCoroutine = false;
        airDashActive = false;
        slashAnim.StopPlayback();
        yield return null;
    }
}
