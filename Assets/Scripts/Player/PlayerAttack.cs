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

    [Header("Slash 1 Stats")]
    [SerializeField] private float slash1GroundDMG = 1f;
    [SerializeField] private float slash1AirDMG = 1f;
    [SerializeField] private float slash1GroundKnockback = .2f;
    [SerializeField] private float slash1AirKnockback = .2f;
    [Header("Slash 2 Stats")]
    [SerializeField] private float slash2GroundDMG = 1f;
    [SerializeField] private float slash2AirDMG = 1f;
    [SerializeField] private float slash2GroundKnockback = .2f;
    [SerializeField] private float slash2AirKnockback = .2f;
    [Header("Slash 3 Stats")]
    [SerializeField] private float slash3GroundDMG = 1f;
    [SerializeField] private float slash3AirDMG = 1f;
    [SerializeField] private float slash3GroundKnockback = .2f;
    [SerializeField] private float slash3AirKnockback = .2f;
    [Header("Air Dash Stats")]
    [SerializeField] private float airDashDMG = 1f;
    [SerializeField] private float airDashKnockback = 1f;

    private float currentDMG;           // These are the values that we can change internally in this class and then pass along to other classes that need them
    private float currentKnockback;

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
        
        if (!GetComponent<PlayerCrouch>().IsCrouching)  // We do nothing when we crouch
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
                
                /*if (!move.IsGrounded && move.CanAirDash && move.IsSprinting) // Air dash
                {
                    StartCoroutine(DoAirDash());
                }
                else    // Regular slashing
                {*/

                //}


            }
            else if (!activeCoroutine)  // If we aren't slashing
            {
                slashCol.enabled = false;
                slashRend.color = Color.clear;
            }
        }
    }

    // Coroutines ---------------------------------------
    IEnumerator DoSlash1()
    {
        activeCoroutine = true;
        slash1Active = true;

        Debug.Log("Slash 1");
        slashCol.enabled = true;
        slashRend.color = Color.white;
        slashAnim.Play("Slash1");
        yield return new WaitForSeconds(.2f);

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

        Debug.Log("Slash 2");
        slashCol.enabled = true;
        slashRend.color = Color.white;
        slashAnim.Play("Slash2");
        yield return new WaitForSeconds(.2f);

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

        Debug.Log("Slash 3");
        slashCol.enabled = true;
        slashRend.color = Color.white;
        slashAnim.Play("Slash3");
        yield return new WaitForSeconds(.2f);

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
        airDashActive = true;
        // Basically just activate the hitbox and play the animation until we touch grass
        // The only thing left that I require is the touch grass variable and zoom animation
        Debug.Log("Air Dash");
        slashCol.enabled = true;
        slashRend.color = Color.white;
        slashAnim.Play("AirDash");
        //yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => !move.IsAirDashing);  // We keep going until we hit the dirt


        activeCoroutine = false;
        airDashActive = false;
        slashAnim.StopPlayback();
        yield return null;
    }
}
