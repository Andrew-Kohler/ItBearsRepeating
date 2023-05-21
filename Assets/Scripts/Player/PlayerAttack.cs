using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject slashbox;

    private Collider2D slashCol;
    private SpriteRenderer slashRend;
    private Animator anim;

    private bool activeCoroutine;
    public bool IsSlashing => activeCoroutine;  // Used to halt grounded movement in PlayerMovement (grounded slashes mean you aren't moving)

    [SerializeField] private bool slash1Active;  // Booleans that let us know which slash is active
    [SerializeField] private bool slash2Active;
    [SerializeField] private bool slash3Active;

    [SerializeField] private bool slash1Done;
    [SerializeField] private bool slash2Done;
    [SerializeField] private bool slash3Done;

    private bool reset; // Helps to trigger the reset when the combo window expires

    private bool onCooldown;

    [SerializeField] private float comboWindowTime = 1f; // The amount of time you have before the combo is reset
    [SerializeField] private float resetCountdown = 0f;      // Acts as a timer

    void Start()
    {
        slashCol = slashbox.GetComponent<Collider2D>();      //Get slashbox collider component
        slashRend = slashbox.GetComponent<SpriteRenderer>(); //Get slashbox sprite renderer Component
        anim = GetComponent<Animator>();            //Get Animator Component

        slashCol.enabled = false;

        // Have another script on the slashbox that deals with what happens when someone is within the collider
    }

    // Update is called once per frame
    void Update()
    {
        if(resetCountdown > 0f)
        {
            resetCountdown -= Time.deltaTime;
        }
        else if(resetCountdown <= 0f && !reset)
        {
            reset = true;
            StartCoroutine(DoPostComboDelay());
        }
        
        if (!GetComponent<PlayerCrouch>().IsCrouching)  // We do nothing when we crouch
        {
            if (Input.GetButtonDown("Slash") && !onCooldown)
            {
                if (!slash1Active && !slash1Done)                  // If slash 1 isn't active
                {
                    StartCoroutine(DoSlash1());
                }
                else if(slash1Done && !slash2Active && !slash2Done && resetCountdown > 0)    // If slash 2 isn't active, slash 1 is done, and we're inside our window
                {
                    StartCoroutine(DoSlash2());
                }
                else if (slash2Done && !slash3Active && resetCountdown > 0)    // If slash 3 isn't active, slash 2 is done, and we're inside our window
                {
                    StartCoroutine(DoSlash3());
                }
            }
            else if (!activeCoroutine)
            {
                slashCol.enabled = false;
                slashRend.color = Color.clear;
            }
        }
    }

    

    // Ok, so I want to do a timed sequence
    // If slash 1 isn't active, start the sequence
    // Once slash 1 is done, set a bool for slash1Done
    // If slash 1 is done, resetCountdown = timeBetweenSlashes;

    // If slash1Done && resetCountdown > 0

    // Else if 

    IEnumerator DoSlash1()
    {
        activeCoroutine = true;
        slash1Active = true;

        Debug.Log("Slash 1");
        slashCol.enabled = true;
        slashRend.color = Color.white;
        yield return new WaitForSeconds(.1f);

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
        slashRend.color = Color.cyan;
        yield return new WaitForSeconds(.1f);

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
        slashRend.color = Color.red;
        yield return new WaitForSeconds(.1f);

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
}
